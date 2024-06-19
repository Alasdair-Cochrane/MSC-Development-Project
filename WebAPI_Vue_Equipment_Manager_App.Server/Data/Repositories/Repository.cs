
using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly MainDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(MainDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async virtual Task<T> AddAsync(T newModel)
        {
            var added = _context.Add(newModel).Entity;
            await _context.SaveChangesAsync();
            return added;
        }

        public async virtual Task<bool> DeleteAsync(int id)
        {
            var toBeDeleted = await _dbSet.FindAsync(id);
            if (toBeDeleted == null)
            {
                return false;
            }
            _context.Remove(toBeDeleted);
            await _context.SaveChangesAsync();
            return true;
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }


        public async virtual Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual Task SaveAsync()
        {
           return  _context.SaveChangesAsync();
        }

        public async virtual Task<T?> UpdateAsync(T updatedModel)
        {
            var existingEntity = await _dbSet.FindAsync(updatedModel.Id);
            if (existingEntity == null) return null;
            _dbSet.Entry(existingEntity).CurrentValues.SetValues(updatedModel);
            await SaveAsync();
            return updatedModel;
        }
    }
}
