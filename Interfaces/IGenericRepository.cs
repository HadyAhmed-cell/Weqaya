using VirtualClinic.Entities;

namespace VirtualClinic.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}