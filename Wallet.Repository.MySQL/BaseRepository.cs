using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Framework.Entities;
using Core.Framework.Repository;
using Microsoft.EntityFrameworkCore;

namespace Wallet.Repository.MySQL
{
    public abstract class BaseRepository<T> : IRepositoryAsync<T>
        where T : class, IEntity
    {
        protected readonly DbContext _context;

        protected DbSet<T> Entity
        { get { return _context.Set<T>(); } }

        protected BaseRepository(DbContext context)
        {
            _context = context;
            _context.Database.Migrate();
        }

        public IQueryable<T> Get()
        {
            try
            {
                return Entity;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IQueryable<T> Get(Func<T, bool> predicate)
        {
            try
            {
                return Entity.Where(predicate).AsQueryable();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public T Find(params object[] keys)
        {
            try
            {
                return Entity.Find(keys);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<T> FindAsync(params object[] keys)
        {
            try
            {
                return await Entity.FindAsync(keys);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Add(T entity)
        {
            try
            {
                entity.CreatedAt = DateTime.Now;
                entity.UpdatedAt = DateTime.Now;
                Entity.Add(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                entity.CreatedAt = DateTime.Now;
                entity.UpdatedAt = DateTime.Now;
                await Entity.AddAsync(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Update(T entity)
        {
            try
            {
                entity.UpdatedAt = DateTime.Now;
                Entity.Update(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Remove(T entity)
        {
            try
            {
                Entity.Remove(entity);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Remove(Func<T, bool> predicate)
        {
            try
            {
                Entity.Where(predicate)
                      .ToList().ForEach(remove => _context.Set<T>().Remove(remove));
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
