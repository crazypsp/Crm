
namespace Crm.Business.Common
{
    public static class Guard
    {
        public static void NotEmpty(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new ValidationException($"{name} boş olamaz.");
        }

        public static void NotNull(object? value, string name)
        {
            if (value is null)
                throw new ValidationException($"{name} boş olamaz.");
        }

        public static void NotBlank(string? value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException($"{name} boş olamaz.");
        }
    }
}
