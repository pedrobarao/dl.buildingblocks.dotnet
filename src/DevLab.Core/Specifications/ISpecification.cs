using DevLab.Core.Communication;
    
    namespace DevLab.Core.Specifications;
    
    /// <summary>
    /// Defines a specification pattern interface for evaluating entities.
    /// </summary>
    /// <typeparam name="T">The type of entity that the specification applies to.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets the error message associated with the specification.
        /// </summary>
        string ErrorMessage { get; }
    
        /// <summary>
        /// Determines whether the specified entity satisfies the specification.
        /// </summary>
        /// <param name="entity">The entity to evaluate.</param>
        /// <returns>true if the entity satisfies the specification; otherwise, false.</returns>
        bool IsSatisfiedBy(T entity);
    
        /// <summary>
        /// Validates the specified entity against the specification.
        /// </summary>
        /// <param name="entity">The entity to validate.</param>
        /// <returns>A ValidationResult containing any validation errors.</returns>
        ValidationResult Validate(T entity);
    
        /// <summary>
        /// Combines the current specification with another specification using a logical AND operation.
        /// </summary>
        /// <param name="specification">The specification to combine with.</param>
        /// <returns>A new specification that represents the logical AND of the two specifications.</returns>
        ISpecification<T> And(ISpecification<T> specification);
    
        /// <summary>
        /// Combines the current specification with another specification using a logical OR operation.
        /// </summary>
        /// <param name="specification">The specification to combine with.</param>
        /// <returns>A new specification that represents the logical OR of the two specifications.</returns>
        ISpecification<T> Or(ISpecification<T> specification);
    
        /// <summary>
        /// Negates the current specification.
        /// </summary>
        /// <returns>A new specification that represents the negation of the current specification.</returns>
        ISpecification<T> Not();
    }