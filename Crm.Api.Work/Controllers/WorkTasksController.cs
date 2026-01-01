using Crm.Api.Work.Contracts;
using Crm.Api.Work.Infrastructure;
using Crm.Data;
using Crm.Entities.Work;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Work.Controllers
{
    [ApiController]
    [Route("api/work/tasks")]
    public sealed class WorkTasksController : ControllerBase
    {
        private readonly CrmDbContext _db;

        public WorkTasksController(CrmDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<PagedResult<TaskDto>>> List(
            [FromQuery] Guid tenantId,
            [FromQuery] Guid? companyId,
            [FromQuery] int? status,
            [FromQuery] DateTimeOffset? dueFrom,
            [FromQuery] DateTimeOffset? dueTo,
            [FromQuery] Guid? assigneeUserId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            CancellationToken ct = default)
        {
            // Neden: Görev listesi büyür; sayfalama şart.
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 200 ? 50 : pageSize;

            var q = _db.WorkTasks.AsNoTracking()
                .Where(x => x.TenantId == tenantId && !x.IsDeleted);

            // CompanyId filtre: property adı farklı olabilir, EF.Property ile okuyoruz.
            if (companyId is not null)
                q = q.Where(x => EF.Property<Guid>(x, "CompanyId") == companyId);

            // Status filtre: entity enum/int olabilir. EF.Property<int> kullanıyoruz.
            if (status is not null)
                q = q.Where(x => EF.Property<int>(x, "Status") == status);

            if (dueFrom is not null)
                q = q.Where(x => EF.Property<DateTimeOffset?>(x, "DueDate") >= dueFrom);

            if (dueTo is not null)
                q = q.Where(x => EF.Property<DateTimeOffset?>(x, "DueDate") <= dueTo);

            // Assignee filtre: Assignment tablosundan taskId listesi alırız.
            // Neden: Task üzerinde “AssigneeId” tek alan değil, çoklu atama var.
            if (assigneeUserId is not null)
            {
                // TaskId alan adı farklı olabilir: TaskId / WorkTaskId
                // UserId alan adı farklı olabilir: UserId / AssigneeUserId
                var baseA = _db.WorkTaskAssignments.AsNoTracking()
                    .Where(a => a.TenantId == tenantId && !a.IsDeleted);

                // UserId filtrelerini alternatif isimlerle denemek için: önce listeyi çekiyoruz (MVP performans yeterli).
                // Production’da expression builder ile tek query yapılır.
                List<WorkTaskAssignment> byUser = new();

                byUser = await EfFallback.ToListWithGuidFilterAsync(baseA, assigneeUserId.Value, ct, "UserId", "AssigneeUserId");

                var taskIds = new HashSet<Guid>();

                // TaskId alan adları için iki deneme
                foreach (var a in byUser)
                {
                    var tid = EntityMap.TryGet(a, "TaskId", "WorkTaskId");
                    if (tid is Guid g) taskIds.Add(g);
                    else if (Guid.TryParse(tid?.ToString(), out var parsed)) taskIds.Add(parsed);
                }

                q = q.Where(t => taskIds.Contains(t.Id));
            }

            var total = await q.CountAsync(ct);

            var tasks = await q
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            // Atamalar: bu sayfadaki task’lar için assignment’ları çekip DTO’ya ekleriz.
            var taskIdList = tasks.Select(t => t.Id).ToList();

            var baseAssignments = _db.WorkTaskAssignments.AsNoTracking()
                .Where(a => a.TenantId == tenantId && !a.IsDeleted);

            // TaskId alan adı TaskId veya WorkTaskId olabilir; ikisini de deneyeceğiz.
            // İlk deneme:
            List<WorkTaskAssignment> assignments = new();
            assignments = await TryGetAssignmentsForTasksAsync(baseAssignments, taskIdList, ct);

            var dto = tasks.Select(t =>
            {
                var assignees = assignments
                    .Where(a =>
                    {
                        var tidObj = EntityMap.TryGet(a, "TaskId", "WorkTaskId");
                        return Guid.TryParse(tidObj?.ToString(), out var tid) && tid == t.Id;
                    })
                    .Select(a =>
                    {
                        var uidObj = EntityMap.TryGet(a, "UserId", "AssigneeUserId");
                        return Guid.TryParse(uidObj?.ToString(), out var uid) ? uid : Guid.Empty;
                    })
                    .Where(x => x != Guid.Empty)
                    .Distinct()
                    .ToList();

                return new TaskDto
                {
                    Id = t.Id,
                    TenantId = t.TenantId,
                    CompanyId = ReadCompanyId(t),
                    Title = EntityMap.TryGetString(t, "Title", "Name", "Subject"),
                    Description = EntityMap.TryGetString(t, "Description", "Notes"),
                    DueDate = EntityMap.TryGetDto(t, "DueDate", "Deadline"),
                    Status = EntityMap.TryGetInt(t, "Status"),
                    Priority = EntityMap.TryGetInt(t, "Priority"),
                    IsActive = EntityMap.TryGetBool(t, "IsActive"),
                    CreatedAt = EntityMap.TryGetCreatedAt(t),
                    AssigneeUserIds = assignees
                };
            }).ToList();

            return Ok(new PagedResult<TaskDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = dto
            });

            static Guid ReadCompanyId(WorkTask t)
            {
                // CompanyId property’si entity’de standart ise bu satır zaten çalışır; değilse EF.Property için query aşamasında filtreledik.
                // DTO doldurmak için reflection ile okumayı deneriz.
                var v = EntityMap.TryGet(t, "CompanyId");
                return Guid.TryParse(v?.ToString(), out var g) ? g : Guid.Empty;
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskDto>> Get(Guid id, [FromQuery] Guid tenantId, CancellationToken ct)
        {
            var task = await _db.WorkTasks.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted, ct);

            if (task is null) return NotFound();

            // Atamalar
            var baseAssignments = _db.WorkTaskAssignments.AsNoTracking()
                .Where(a => a.TenantId == tenantId && !a.IsDeleted);

            var assignments = await TryGetAssignmentsForTasksAsync(baseAssignments, new List<Guid> { id }, ct);

            var assignees = assignments.Select(a =>
            {
                var uidObj = EntityMap.TryGet(a, "UserId", "AssigneeUserId");
                return Guid.TryParse(uidObj?.ToString(), out var uid) ? uid : Guid.Empty;
            }).Where(x => x != Guid.Empty).Distinct().ToList();

            var dto = new TaskDto
            {
                Id = task.Id,
                TenantId = task.TenantId,
                CompanyId = Guid.TryParse(EntityMap.TryGet(task, "CompanyId")?.ToString(), out var cg) ? cg : Guid.Empty,
                Title = EntityMap.TryGetString(task, "Title", "Name", "Subject"),
                Description = EntityMap.TryGetString(task, "Description", "Notes"),
                DueDate = EntityMap.TryGetDto(task, "DueDate", "Deadline"),
                Status = EntityMap.TryGetInt(task, "Status"),
                Priority = EntityMap.TryGetInt(task, "Priority"),
                IsActive = EntityMap.TryGetBool(task, "IsActive"),
                CreatedAt = EntityMap.TryGetCreatedAt(task),
                AssigneeUserIds = assignees
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest req, CancellationToken ct)
        {
            // Neden: Görev kaydı; personel ataması ayrı tabloda tutulur (çoklu atama ihtiyacı).
            var task = new WorkTask
            {
                Id = Guid.NewGuid(),
                TenantId = req.TenantId,
                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            EntityMap.TrySet(task,
                ("CompanyId", req.CompanyId),
                ("Title", req.Title),
                ("Name", req.Title),
                ("Subject", req.Title),
                ("Description", req.Description),
                ("Notes", req.Description),
                ("DueDate", req.DueDate),
                ("Deadline", req.DueDate),
                ("Status", req.Status),
                ("Priority", req.Priority),
                ("IsActive", true)
            );

            _db.WorkTasks.Add(task);

            // Atamalar
            foreach (var uid in req.AssigneeUserIds.Distinct())
            {
                var a = new WorkTaskAssignment
                {
                    Id = Guid.NewGuid(),
                    TenantId = req.TenantId,
                    IsDeleted = false,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                // Neden: FK alan adları farklı olabilir diye çoklu isim set ediyoruz.
                EntityMap.TrySet(a,
                    ("TaskId", task.Id),
                    ("WorkTaskId", task.Id),
                    ("UserId", uid),
                    ("AssigneeUserId", uid),
                    ("AssignedAt", DateTimeOffset.UtcNow)
                );

                _db.WorkTaskAssignments.Add(a);
            }

            await _db.SaveChangesAsync(ct);

            return Ok(new { id = task.Id });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest req, CancellationToken ct)
        {
            var task = await _db.WorkTasks
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == req.TenantId && !x.IsDeleted, ct);

            if (task is null) return NotFound();

            EntityMap.TrySet(task,
                ("Title", req.Title),
                ("Name", req.Title),
                ("Subject", req.Title),
                ("Description", req.Description),
                ("Notes", req.Description),
                ("DueDate", req.DueDate),
                ("Deadline", req.DueDate),
                ("Status", req.Status),
                ("Priority", req.Priority),
                ("IsActive", req.IsActive),
                ("UpdatedAt", DateTimeOffset.UtcNow)
            );

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }

        [HttpPost("{id:guid}/assign")]
        public async Task<IActionResult> Assign(Guid id, [FromBody] AssignUsersRequest req, CancellationToken ct)
        {
            // Neden: Görev oluşturulduktan sonra da personel değişebilir.
            var taskExists = await _db.WorkTasks.AsNoTracking()
                .AnyAsync(x => x.Id == id && x.TenantId == req.TenantId && !x.IsDeleted, ct);

            if (!taskExists) return NotFound("Görev bulunamadı.");

            foreach (var uid in req.UserIds.Distinct())
            {
                var a = new WorkTaskAssignment
                {
                    Id = Guid.NewGuid(),
                    TenantId = req.TenantId,
                    IsDeleted = false,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                EntityMap.TrySet(a,
                    ("TaskId", id),
                    ("WorkTaskId", id),
                    ("UserId", uid),
                    ("AssigneeUserId", uid),
                    ("AssignedAt", DateTimeOffset.UtcNow)
                );

                _db.WorkTaskAssignments.Add(a);
            }

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }

        [HttpDelete("{id:guid}/assign/{userId:guid}")]
        public async Task<IActionResult> Unassign(Guid id, Guid userId, [FromQuery] Guid tenantId, CancellationToken ct)
        {
            // Neden: Atama kaldırma operasyonu soft-delete ile yapılır; audit için önemlidir.
            var baseQ = _db.WorkTaskAssignments
                .Where(a => a.TenantId == tenantId && !a.IsDeleted);

            // TaskId alternatifleri ve UserId alternatifleri nedeniyle, önce listeyi çekip memory’de eşleştiriyoruz (MVP).
            var candidates = await baseQ.ToListAsync(ct);

            var match = candidates.FirstOrDefault(a =>
            {
                var tidObj = EntityMap.TryGet(a, "TaskId", "WorkTaskId");
                var uidObj = EntityMap.TryGet(a, "UserId", "AssigneeUserId");

                var tidOk = Guid.TryParse(tidObj?.ToString(), out var tid) && tid == id;
                var uidOk = Guid.TryParse(uidObj?.ToString(), out var uid) && uid == userId;
                return tidOk && uidOk;
            });

            if (match is null) return NotFound("Atama bulunamadı.");

            match.IsDeleted = true;
            match.DeletedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid tenantId, CancellationToken ct)
        {
            // Neden: Görev kayıtları muhasebe/CRM süreçlerinde audit için saklanır; soft delete uygundur.
            var task = await _db.WorkTasks
                .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == tenantId && !x.IsDeleted, ct);

            if (task is null) return NotFound();

            task.IsDeleted = true;
            task.DeletedAt = DateTimeOffset.UtcNow;

            await _db.SaveChangesAsync(ct);
            return Ok(new { ok = true });
        }

        private static async Task<List<WorkTaskAssignment>> TryGetAssignmentsForTasksAsync(
            IQueryable<WorkTaskAssignment> baseAssignments,
            List<Guid> taskIds,
            CancellationToken ct)
        {
            // Neden: TaskId alanı TaskId veya WorkTaskId olabilir; ikisini de deniyoruz.
            try
            {
                return await baseAssignments
                    .Where(a => taskIds.Contains(EF.Property<Guid>(a, "TaskId")))
                    .ToListAsync(ct);
            }
            catch (InvalidOperationException)
            {
                return await baseAssignments
                    .Where(a => taskIds.Contains(EF.Property<Guid>(a, "WorkTaskId")))
                    .ToListAsync(ct);
            }
        }
    }
}
