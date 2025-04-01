using DevLab.Core.Communication;

namespace DevLab.Core.Specifications;

public abstract class Specification<T>(string errorMessage) : ISpecification<T>
{
    public abstract bool IsSatisfiedBy(T entity);

    public string ErrorMessage { get; protected set; } = errorMessage;

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

    public ISpecification<T> And(ISpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }

    public ISpecification<T> Or(ISpecification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }

    public ISpecification<T> Not()
    {
        return new NotSpecification<T>(this);
    }
}