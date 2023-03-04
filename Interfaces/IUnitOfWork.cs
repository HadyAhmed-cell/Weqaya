using VirtualClinic.Entities;

namespace VirtualClinic.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> Repositary<TEntity>() where TEntity : BaseEntity;

        Task<int> Complete();
    }
}