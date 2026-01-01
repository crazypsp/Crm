using System.Reflection;

namespace Crm.Api.Import.Infrastructure
{
    public static class EntityMap
    {
        // Neden: Entity property adı farklıysa (Path vs StoragePath) atla, derleme/çalışma kırılmasın.
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

                    if (v is not null && target.IsEnum && v is int i)
                        v = Enum.ToObject(target, i);

                    if (v is not null && target != v.GetType())
                        v = Convert.ChangeType(v, target);

                    p.SetValue(entity, v);
                }
                catch { }
            }
        }
    }
}
