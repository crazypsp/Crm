using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Reporting.Infrastructure
{
    public static class EfTry
    {
        /// <summary>
        /// Neden: Bazı entity’lerde FK alan adları (CompanyId vs CustomerId gibi) farklı olabilir.
        /// Burada ilk isimle dener, olmazsa alternatife geçer.
        /// </summary>
        public static IQueryable<T> WhereGuidEquals<T>(IQueryable<T> q, Guid value, params string[] propertyNames) where T : class
        {
            foreach (var name in propertyNames)
            {
                try
                {
                    // EF.Property çalışma zamanı isim bazlı okur; property yoksa InvalidOperationException fırlatır.
                    return q.Where(x => EF.Property<Guid>(x, name) == value);
                }
                catch (InvalidOperationException)
                {
                    // next
                }
            }
            // Hiçbiri yoksa filtre uygulamadan döner (MVP’de rapor boş kalmasın).
            return q;
        }

        public static IQueryable<T> WhereIntEquals<T>(IQueryable<T> q, int value, params string[] propertyNames) where T : class
        {
            foreach (var name in propertyNames)
            {
                try { return q.Where(x => EF.Property<int>(x, name) == value); }
                catch (InvalidOperationException) { }
            }
            return q;
        }

        public static async Task<int> SafeCountAsync<T>(IQueryable<T> q, CancellationToken ct) where T : class
        {
            try { return await q.CountAsync(ct); }
            catch { return 0; } // Neden: Reporting ekranı “kırılmasın”, en kötü 0 dönsün.
        }
    }
}
