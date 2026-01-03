namespace Crm.Business.Common;

/// <summary>
/// Neden: Business katmanındaki tüm doğrulama çağrılarını tek noktada standardize etmek.
/// 
/// Bu sınıf "geriye dönük uyum" için iki isim setini de içerir:
/// - NotNull / NotEmpty / NotBlank
/// - AgainstNull / AgainstNullOrWhiteSpace
/// 
/// Böylece projede farklı dosyalarda farklı isimlerle yapılan guard çağrıları build'i bozmaz.
/// </summary>
public static class Guard
{
    // ----------------------------
    // Yeni/okunaklı API (Not*)
    // ----------------------------

    public static void NotNull(object? value, string name)
    {
        if (value is null)
            throw new ArgumentNullException(name);
    }

    public static void NotEmpty(Guid value, string name)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"{name} cannot be empty.", name);
    }

    public static void NotEmpty(string? value, string name)
    {
        if (value is null)
            throw new ArgumentNullException(name);

        if (value.Length == 0)
            throw new ArgumentException($"{name} cannot be empty.", name);
    }

    public static void NotBlank(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{name} cannot be blank.", name);
    }

    public static void NotEmpty<T>(IEnumerable<T>? value, string name)
    {
        if (value is null)
            throw new ArgumentNullException(name);

        if (!value.Any())
            throw new ArgumentException($"{name} cannot be empty.", name);
    }

    public static void Against(bool condition, string message)
    {
        if (condition)
            throw new ArgumentException(message);
    }

    // -----------------------------------------
    // Geriye dönük uyum API (Against*)
    // -----------------------------------------

    /// <summary>
    /// Neden: Eski kodların kullandığı isim (AgainstNull). NotNull ile aynı iş.
    /// </summary>
    public static void AgainstNull(object? value, string paramName)
        => NotNull(value, paramName);

    /// <summary>
    /// Neden: Eski kodların kullandığı isim (AgainstNullOrWhiteSpace). NotBlank ile aynı iş.
    /// </summary>
    public static void AgainstNullOrWhiteSpace(string? value, string paramName)
        => NotBlank(value, paramName);

    
}
