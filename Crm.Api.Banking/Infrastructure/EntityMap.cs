using System.Reflection;
using System.Text.Json;

namespace Crm.Api.Banking.Infrastructure
{
    public static class EntityMap
    {
        // Neden: Entity alanları farklı isimlerde olabilir; controller derlensin diye property erişimini güvenli yapıyoruz.
        // Production’da entity alanları netleşince reflection kaldırılır.

        public static void TrySet<T>(T entity, params (string Name, object? Value)[] assignments)
        {
            var t = typeof(T);

            foreach (var (name, value) in assignments)
            {
                var p = t.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (p is null || !p.CanWrite) continue;

                // Nullable uyumu
                var targetType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;

                try
                {
                    object? converted = value;

                    if (value is not null && targetType.IsEnum && value is int i)
                        converted = Enum.ToObject(targetType, i);

                    if (value is not null && targetType == typeof(Guid) && value is string sGuid && Guid.TryParse(sGuid, out var g))
                        converted = g;

                    if (value is not null && targetType == typeof(string) && value is not string)
                        converted = value.ToString();

                    if (value is not null && targetType != (Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType))
                        converted = Convert.ChangeType(converted, targetType);

                    p.SetValue(entity, converted);
                }
                catch
                {
                    // Neden: “hatasız çalıştırma” için invalid dönüşümleri sessizce geçiyoruz.
                    // İleride domain alanları netleşince bu davranış strict hale getirilir.
                }
            }
        }

        public static object? TryGet(object entity, params string[] names)
        {
            var t = entity.GetType();
            foreach (var name in names)
            {
                var p = t.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (p is null || !p.CanRead) continue;
                return p.GetValue(entity);
            }
            return null;
        }

        public static string? TryGetString(object entity, params string[] names)
            => TryGet(entity, names)?.ToString();

        public static bool? TryGetBool(object entity, params string[] names)
        {
            var v = TryGet(entity, names);
            if (v is null) return null;
            if (v is bool b) return b;
            if (bool.TryParse(v.ToString(), out var parsed)) return parsed;
            return null;
        }

        public static Guid? TryGetGuid(object entity, params string[] names)
        {
            var v = TryGet(entity, names);
            if (v is null) return null;
            if (v is Guid g) return g;
            if (Guid.TryParse(v.ToString(), out var parsed)) return parsed;
            return null;
        }

        public static DateTimeOffset TryGetCreatedAt(object entity)
        {
            var v = TryGet(entity, "CreatedAt");
            return v is DateTimeOffset dto ? dto : DateTimeOffset.UtcNow;
        }

        public static string SerializeDefinition(Dictionary<string, string> def)
            => JsonSerializer.Serialize(def);

        public static Dictionary<string, string> DeserializeDefinition(string? json)
            => string.IsNullOrWhiteSpace(json)
                ? new Dictionary<string, string>()
                : (JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>());
    }
}
