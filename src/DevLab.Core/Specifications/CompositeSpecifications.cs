using DevLab.Core.Communication;

namespace DevLab.Core.Specifications;

/// <summary>
///     Represents a specification that combines two specifications with a logical AND operation.
/// </summary>
/// <typeparam name="T">The type of entity that the specification applies to.</typeparam>
public class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right)
    : Specification<T>($"{left.ErrorMessage} e {right.ErrorMessage}")
{
    /// <summary>
    ///     Determines whether the specified entity satisfies both specifications.
    /// </summary>
    /// <param name="entity">The entity to evaluate.</param>
    /// <returns>true if the entity satisfies both specifications; otherwise, false.</returns>
    public override bool IsSatisfiedBy(T entity)
    {
        var leftIsSatisfied = left.IsSatisfiedBy(entity);
        var rightIsSatisfied = right.IsSatisfiedBy(entity);

        if (!leftIsSatisfied)
            ErrorMessage = left.ErrorMessage;
        else if (!rightIsSatisfied)
            ErrorMessage = right.ErrorMessage;

        return leftIsSatisfied && rightIsSatisfied;
    }

    /// <summary>
    ///     Validates the specified entity against both specifications.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <returns>A ValidationResult containing any validation errors.</returns>
    public override ValidationResult Validate(T entity)
    {
        var validationResult = new ValidationResult();

        var leftValidation = left.Validate(entity);
        var rightValidation = right.Validate(entity);

        validationResult.AddErrorRange(leftValidation.Errors.ToList());
        validationResult.AddErrorRange(rightValidation.Errors.ToList());

        return validationResult;
    }
}

/// <summary>
///     Represents a specification that combines two specifications with a logical OR operation.
/// </summary>
/// <typeparam name="T">The type of entity that the specification applies to.</typeparam>
public class OrSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    /// <summary>
    ///     Initializes a new instance of the <see cref="OrSpecification{T}" /> class.
    /// </summary>
    /// <param name="left">The left specification.</param>
    /// <param name="right">The right specification.</param>
    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        : base($"{left.ErrorMessage} ou {right.ErrorMessage}")
    {
        _left = left;
        _right = right;
    }

    /// <summary>
    ///     Determines whether the specified entity satisfies at least one of the specifications.
    /// </summary>
    /// <param name="entity">The entity to evaluate.</param>
    /// <returns>true if the entity satisfies at least one of the specifications; otherwise, false.</returns>
    public override bool IsSatisfiedBy(T entity)
    {
        var leftIsSatisfied = _left.IsSatisfiedBy(entity);
        var rightIsSatisfied = _right.IsSatisfiedBy(entity);

        if (!leftIsSatisfied && !rightIsSatisfied)
            ErrorMessage = $"{_left.ErrorMessage} ou {_right.ErrorMessage}";

        return leftIsSatisfied || rightIsSatisfied;
    }

    /// <summary>
    ///     Validates the specified entity against at least one of the specifications.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <returns>A ValidationResult containing any validation errors.</returns>
    public override ValidationResult Validate(T entity)
    {
        var leftIsSatisfied = _left.IsSatisfiedBy(entity);
        var rightIsSatisfied = _right.IsSatisfiedBy(entity);

        // If either side is satisfied, validation passes
        if (leftIsSatisfied || rightIsSatisfied)
            return new ValidationResult();

        // If both fail, return errors from both
        var validationResult = new ValidationResult();
        validationResult.AddErrorRange(new List<Error>
        {
            new(typeof(T).Name, $"{_left.ErrorMessage} ou {_right.ErrorMessage}")
        });

        return validationResult;
    }
}

/// <summary>
///     Represents a specification that negates another specification.
/// </summary>
/// <typeparam name="T">The type of entity that the specification applies to.</typeparam>
public class NotSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _specification;

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotSpecification{T}" /> class.
    /// </summary>
    /// <param name="specification">The specification to negate.</param>
    public NotSpecification(ISpecification<T> specification)
        : base($"Não ({specification.ErrorMessage})")
    {
        _specification = specification;
    }

    /// <summary>
    ///     Determines whether the specified entity does not satisfy the specification.
    /// </summary>
    /// <param name="entity">The entity to evaluate.</param>
    /// <returns>true if the entity does not satisfy the specification; otherwise, false.</returns>
    public override bool IsSatisfiedBy(T entity)
    {
        return !_specification.IsSatisfiedBy(entity);
    }

    /// <summary>
    ///     Validates the specified entity against the negated specification.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <returns>A ValidationResult containing any validation errors.</returns>
    public override ValidationResult Validate(T entity)
    {
        var isSatisfied = IsSatisfiedBy(entity);
        var validationResult = new ValidationResult();

        if (!isSatisfied)
            validationResult.AddErrorRange([new Error(ErrorMessage, typeof(T).Name)]);

        return validationResult;
    }
}