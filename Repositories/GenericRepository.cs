using Microsoft.EntityFrameworkCore;
using VirtualClinic.Data;
using VirtualClinic.Entities;
using VirtualClinic.Interfaces;

namespace VirtualClinic.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        => _context.Set<T>().Add(entity);

        public void Delete(T entity)
         => _context.Set<T>().Remove(entity);

        public async Task<IEnumerable<T>> GetAllAsync()
        => await _context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);

        public void Update(T entity)
        => _context.Set<T>().Update(entity);
    }
}