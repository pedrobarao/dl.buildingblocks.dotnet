namespace DevLab.Core.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}