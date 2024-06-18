using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Data;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;


//Notes - Generic Repositories appear to be controversial
//however - for specific queiries i can extend them into entity-specific repositories
//A generic repository allows me to avoid repeading the same database access code for each entity type
//most of the operations will be CRUD anyway

namespace WebAPI_Vue_Equipment_Manager_App.Server.Deprecated
{
    [Obsolete("generic no longer used")]
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
    {
        private readonly MainDbContext _context;
        private readonly DbSet<T> _entities;

        public BaseRepository(MainDbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public virtual IQueryable<T> GetAllQueryable()
        {
            return _entities.AsQueryable();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }


        public virtual T Add(T entity)
        {
            T newEntry = _context.Add(entity).Entity;
            _context.SaveChanges();
            return newEntry;
        }
        public virtual void Delete(int id)
        {
            var toBeDeleted = GetById(id);
            if (toBeDeleted != null)
            {
                _entities.Remove(toBeDeleted);
                Save();
            }
        }
        public virtual T? GetById(int id)
        {
            T? foundEntity = _entities.Find(id);
            return foundEntity;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public virtual T? Update(T newEntity)
        {
            var existingEntity = _entities.Find(newEntity.Id);
            if (existingEntity == null) return null;
            _entities.Entry(existingEntity).CurrentValues.SetValues(newEntity);
            return newEntity;
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}
