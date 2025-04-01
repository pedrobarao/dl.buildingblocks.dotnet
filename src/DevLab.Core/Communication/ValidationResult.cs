namespace DevLab.Core.Communication;

public class ValidationResult
{
    private readonly List<Error> _errors = new();
    public IReadOnlyCollection<Error> Errors => _errors;
    public bool IsValid => !Errors.Any();
    public bool IsInvalid => !IsValid;

    public void AddError(Error error)
    {
        _errors.Add(error);
    }

    public void AddErrorRange(List<Error> errors)
    {
        _errors.AddRange(errors);
    }
}