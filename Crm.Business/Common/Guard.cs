public static class Guard
{
    // Yeni API
    public static void NotNull(object? value, string paramName)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);
    }

    public static void NotEmpty(Guid value, string paramName)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"{paramName} cannot be empty.", paramName);
    }

    public static void NotEmpty(string? value, string paramName)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);
        if (value.Length == 0)
            throw new ArgumentException($"{paramName} cannot be empty.", paramName);
    }

    public static void NotBlank(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be blank.", paramName);
    }

    public static void NotEmpty<T>(IEnumerable<T>? value, string paramName)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);
        if (!value.Any())
            throw new ArgumentException($"{paramName} cannot be empty.", paramName);
    }

    public static void Against(bool condition, string message)
    {
        if (condition)
            throw new ArgumentException(message);
    }

    public static void InRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be between {min} and {max}.");
    }

    public static void GreaterThan(decimal value, decimal min, string paramName)
    {
        if (value <= min)
            throw new ArgumentException($"{paramName} must be greater than {min}.", paramName);
    }

    // Geriye dönük uyum API
    public static void AgainstNull(object? value, string paramName)
        => NotNull(value, paramName);

    public static void AgainstNullOrWhiteSpace(string? value, string paramName)
        => NotBlank(value, paramName);
}