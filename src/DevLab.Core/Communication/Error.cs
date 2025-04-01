namespace DevLab.Core.Communication;

/// <summary>
///     Represents an error in a result, for demonstration purposes we use simple predefined errors.
/// </summary>
public sealed record Error(string Message, string? Code = null)
{
    public static readonly Error None = new("None", "No error");
    public static readonly Error NullValue = new("NullValue", "Value is null");

    public override string ToString()
    {
        return !string.IsNullOrEmpty(Code) ? $"{Code}: {Message}" : Message;
    }
}