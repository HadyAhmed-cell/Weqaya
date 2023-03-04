using System.Collections;
using VirtualClinic.Data;
using VirtualClinic.Entities;
using VirtualClinic.Interfaces;

namespace VirtualClinic.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private Hashtable _repositaries;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
            => await _context.SaveChangesAsync();

        public IGenericRepository<TEntity> Repositary<TEntity>() where TEntity : BaseEntity
        {
            if ( _repositaries is null )
            {
                _repositaries = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if ( !_repositaries.ContainsKey(type) )
            {
                var repository = new GenericRepository<TEntity>(_context);
                _repositaries.Add(type, repository);
            }

            return (IGenericRepository<TEntity>)_repositaries[type];
        }
    }
}