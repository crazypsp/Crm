using System.Reflection;

namespace Crm.Api.Work.Infrastructure
{
    public static class EntityMap
    {
        // Neden: Entity alan adları kesinleşmeden controller’ların derlenebilir ve çalışabilir kalması.
        // İleride entity alan adları netleşince bu sınıf kaldırılıp strongly-typed map’e geçilir.

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

                    // Enum alanlara int geldiğinde otomatik çevir.
                    if (v is not null && target.IsEnum && v is int i)
                        v = Enum.ToObject(target, i);

                    if (v is not null && target == typeof(Guid) && v is string s && Guid.TryParse(s, out var g))
                        v = g;

                    if (v is not null && target != v.GetType())
                        v = Convert.ChangeType(v, target);

                    p.SetValue(entity, v);
                }
                catch
                {
                    // Neden: MVP’de “alan yoksa/uyuşmazsa atla” yaklaşımıyla sürtünmeyi azaltırız.
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

        public static DateTimeOffset TryGetCreatedAt(object entity)
            => TryGet(entity, "CreatedAt") is DateTimeOffset dto ? dto : DateTimeOffset.UtcNow;

        public static DateTimeOffset? TryGetDto(object entity, params string[] names)
        {
            var v = TryGet(entity, names);
            if (v is null) return null;
            if (v is DateTimeOffset dto) return dto;
            if (DateTimeOffset.TryParse(v.ToString(), out var parsed)) return parsed;
            return null;
        }

        public static int? TryGetInt(object entity, params string[] names)
        {
            var v = TryGet(entity, names);
            if (v is null) return null;
            if (v is int i) return i;
            if (int.TryParse(v.ToString(), out var parsed)) return parsed;
            return null;
        }

        public static bool? TryGetBool(object entity, params string[] names)
        {
            var v = TryGet(entity, names);
            if (v is null) return null;
            if (v is bool b) return b;
            if (bool.TryParse(v.ToString(), out var parsed)) return parsed;
            return null;
        }
    }
}
