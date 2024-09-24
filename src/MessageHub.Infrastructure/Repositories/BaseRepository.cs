using MessageHub.Domain.Entities;
using MessageHub.Infrastructure.Database;

namespace MessageHub.Infrastructure.Repositories
{
    public class BaseRepository<T> where T : Entity, new()
    {
        protected readonly MessageDbContext _context;

        public BaseRepository(MessageDbContext context)
        {
            _context = context;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task<T> InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

    }
}