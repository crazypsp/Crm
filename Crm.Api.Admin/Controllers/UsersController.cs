using Crm.Api.Admin.Contracts;
using Crm.Api.Admin.Infrastructure;
using Crm.Data;
using Crm.Entities.Identity;
using Crm.Entities.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    public sealed class UsersController : ControllerBase
    {
        private readonly CrmDbContext _db;
        private readonly UserManager<ApplicationUser> _users;
        private readonly RoleManager<ApplicationRole> _roles;

        public UsersController(CrmDbContext db, UserManager<ApplicationUser> users, RoleManager<ApplicationRole> roles)
        {
            _db = db;
            _users = users;
            _roles = roles;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest req, CancellationToken ct)
        {
            // Neden: Admin panelinden kullanıcı oluşturma (personel/firma/bayi vb.).
            if (!await _roles.RoleExistsAsync(req.Role))
            {
                // Neden: Rol yoksa runtime’da “role assign” patlar; burada otomatik oluşturuyoruz.
                await _roles.CreateAsync(new ApplicationRole { Name = req.Role });
            }

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = req.Email,
                UserName = req.UserName,
                EmailConfirmed = true // MVP: doğrulama akışını sonra ekleriz.
            };

            var create = await _users.CreateAsync(user, req.Password);
            if (!create.Succeeded)
                return BadRequest(create.Errors.Select(e => e.Description));

            await _users.AddToRoleAsync(user, req.Role);

            // Üyelik (Tenant/Company bağlamı)
            // Neden: “kullanıcı hangi tenant/firma içinde hangi yetkide?” sorusunun cevabı.
            if (req.TenantId is not null)
            {
                var tenantExists = await _db.Tenants.AsNoTracking()
                    .AnyAsync(t => t.Id == req.TenantId.Value && !t.IsDeleted, ct);

                if (!tenantExists) return BadRequest("TenantId bulunamadı.");

                if (req.CompanyId is not null)
                {
                    var companyOk = await _db.Companies.AsNoTracking()
                        .AnyAsync(c => c.Id == req.CompanyId.Value && c.TenantId == req.TenantId.Value && !c.IsDeleted, ct);

                    if (!companyOk) return BadRequest("CompanyId bulunamadı veya tenant ile uyumsuz.");
                }

                var membership = new UserMembership
                {
                    Id = Guid.NewGuid(),
                    TenantId = req.TenantId.Value,
                    CreatedAt = DateTimeOffset.UtcNow,
                    IsDeleted = false
                };

                EntityMap.TrySet(membership,
                    ("UserId", user.Id),
                    ("CompanyId", req.CompanyId),
                    ("Role", req.Role),            // entity enum/string olabilir; TrySet dener
                    ("MembershipRole", req.Role),  // alternatif isim
                    ("IsActive", true)
                );

                _db.UserMemberships.Add(membership);
                await _db.SaveChangesAsync(ct);
            }

            return Ok(new { id = user.Id });
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var user = await _users.FindByIdAsync(id.ToString());
            if (user is null) return NotFound();

            var roles = (await _users.GetRolesAsync(user)).ToList();

            return Ok(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> List([FromQuery] string? q)
        {
            // Neden: Admin paneli kullanıcı araması.
            var query = _users.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(u => u.Email!.Contains(q) || u.UserName!.Contains(q));

            var users = await query.OrderBy(u => u.UserName).Take(100).ToListAsync();

            // Rol bilgisi için tek tek GetRolesAsync gerekir; MVP’de kabul edilebilir.
            var list = new List<UserDto>();
            foreach (var u in users)
            {
                var roles = (await _users.GetRolesAsync(u)).ToList();
                list.Add(new UserDto { Id = u.Id, Email = u.Email, UserName = u.UserName, Roles = roles });
            }

            return Ok(list);
        }
    }
}
