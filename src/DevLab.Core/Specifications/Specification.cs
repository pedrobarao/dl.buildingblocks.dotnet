using DevLab.Core.Communication;

namespace DevLab.Core.Specifications;

/// <inheritdoc />
public abstract class Specification<T>(string errorMessage) : ISpecification<T>
{
    /// <inheritdoc />
    public abstract bool IsSatisfiedBy(T entity);

    /// <inheritdoc />
    public string ErrorMessage { get; protected set; } = errorMessage;

    /// <inheritdoc />
    public virtual ValidationResult Validate(T entity)
    {
        var isValid = IsSatisfiedBy(entity);
        var validationResult = new ValidationResult();

        if (isValid) return validationResult;

        validationResult.AddErrorRange([
            new Error(typeof(T).Name, ErrorMessage)
        ]);

        return validationResult;
    }

    /// <inheritdoc />
    public ISpecification<T> And(ISpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }

    /// <inheritdoc />
    public ISpecification<T> Or(ISpecification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }

    /// <inheritdoc />
    public ISpecification<T> Not()
    {
        return new NotSpecification<T>(this);
    }
}