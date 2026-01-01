using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Work.Infrastructure
{
    public static class EfFallback
    {
        /// <summary>
        /// Neden: Bazı entity'lerde FK alan adları TaskId / WorkTaskId gibi değişebilir.
        /// EF.Property isim uyuşmazlığında runtime exception verir. Burada alternatif isimleri deneriz.
        /// </summary>
        public static async Task<List<T>> ToListWithGuidFilterAsync<T>(
            IQueryable<T> baseQuery,
            Guid value,
            CancellationToken ct,
            params string[] possiblePropertyNames)
            where T : class
        {
            foreach (var name in possiblePropertyNames)
            {
                try
                {
                    var q = baseQuery.Where(x => EF.Property<Guid>(x, name) == value);
                    return await q.ToListAsync(ct);
                }
                catch (InvalidOperationException)
                {
                    // property adı yoksa bir sonrakini dene
                }
            }

            return new List<T>();
        }
    }
}
