namespace DevLab.Core.Communication;

/// <summary>
///     Represents the result of a validation operation.
/// </summary>
public class ValidationResult
{
    private readonly List<Error> _errors = new();

    /// <summary>
    ///     Gets the collection of validation errors.
    /// </summary>
    public IReadOnlyCollection<Error> Errors => _errors;

    /// <summary>
    ///     Gets a value indicating whether the validation result is valid.
    /// </summary>
    public bool IsValid => !Errors.Any();

    /// <summary>
    ///     Gets a value indicating whether the validation result is invalid.
    /// </summary>
    public bool IsInvalid => !IsValid;

    /// <summary>
    ///     Adds a validation error to the result.
    /// </summary>
    /// <param name="error">The error to add.</param>
    public void AddError(Error error)
    {
        _errors.Add(error);
    }

    /// <summary>
    ///     Adds a validation error to the result with a message and an optional code.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="code">The optional error code.</param>
    public void AddError(string message, string? code = null)
    {
        _errors.Add(new Error(message, code));
    }

    /// <summary>
    ///     Adds a range of validation errors to the result.
    /// </summary>
    /// <param name="errors">The list of errors to add.</param>
    public void AddErrorRange(List<Error> errors)
    {
        _errors.AddRange(errors);
    }
}