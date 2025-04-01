using DevLab.Core.DomainObjects;

namespace DevLab.Core.Data;

/// <summary>
/// Generic interface for repositories implementing the repository pattern.
/// </summary>
/// <typeparam name="T">The type of the entity that implements <see cref="IAggregateRoot"/>.</typeparam>
public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    /// <summary>
    /// Gets the unit of work associated with the repository.
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}