namespace DevLab.Core.Data;

/// <summary>
///     Interface that defines the unit of work.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Commits the changes made in the unit of work.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is true if the commit is successful;
    ///     otherwise, false.
    /// </returns>
    Task<bool> Commit();
}