
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

        public virtual T Add(T newModel)
        {
            return _context.Add(newModel).Entity;
        }

        public virtual bool Delete(int id)
        {
            var toBeDeleted = _dbSet.Find(id);
            if (toBeDeleted == null)
            {
                return false;
            }
            _context.Remove(toBeDeleted);
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
            return updatedModel;
        }
    }
}
