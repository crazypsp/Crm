using System.Reflection;

namespace Crm.Api.Admin.Infrastructure
{
    public static class EntityMap
    {
        // Neden: Entity alanları farklı isimdeyse (Name vs Title) set etmeye çalışıp yoksa atlarız.
        public static void TrySet<T>(T entity, params (string Name, object? Value)[] assignments)
        {
            var t = typeof(T);

            foreach (var (name, value) in assignments)
            {
                var p = t.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (p is null || !p.CanWrite) continue;

                try
                {
                    var target = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    object? v = value;

                    if (v is not null && target.IsEnum && v is string s && Enum.TryParse(target, s, true, out var ev))
                        v = ev;

                    if (v is not null && target != v.GetType())
                        v = Convert.ChangeType(v, target);

                    p.SetValue(entity, v);
                }
                catch
                {
                    // MVP: Alan uyuşmazlığı sistemin derlenmesini/çalışmasını engellemesin.
                }
            }
        }

        public static object? TryGet(object entity, params string[] names)
        {
            var t = entity.GetType();
            foreach (var n in names)
            {
                var p = t.GetProperty(n, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (p is null || !p.CanRead) continue;
                return p.GetValue(entity);
            }
            return null;
        }

        public static string? TryGetString(object entity, params string[] names) => TryGet(entity, names)?.ToString();
    }
}
