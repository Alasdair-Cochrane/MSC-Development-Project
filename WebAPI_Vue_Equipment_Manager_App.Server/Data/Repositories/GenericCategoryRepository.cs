using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class GenericCategoryRepository<T> : ICategoryRepository<T> where T : Category, new()
    {
        private readonly DbSet<T> _context;
        private readonly DbContext _dbContext;
        private readonly IEntityCache<T> _categories;
        public GenericCategoryRepository(MainDbContext context, IEntityCache<T> cache)
        {
            _context = context.Set<T>();
            _categories = cache;
            _dbContext = context;

            if (_categories.CachedItems == null)
            {
                _categories.CachedItems = _context.ToList();
            }
        }

        private void Refresh()
        {
            _categories.CachedItems = _context.ToList();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _categories.CachedItems.AsQueryable();
        }

        public IEnumerable<T> GetAll()
        {
            return _categories.CachedItems;
        }


        public T Add(T type)
        {
            T newEntry = _context.Add(type).Entity;
            Save();
            return newEntry;
        }
        public void Delete(int id)
        {
            var toBeDeleted = GetById(id);
            if (toBeDeleted != null)
            {
                _context.Remove(toBeDeleted);
                Save();
            }
        }

        public T GetById(int id)
        {
            return _categories.CachedItems.First(type => type.Id == id);
        }

        public T Update(T type)
        {
            T updatedType = _context.Update(type).Entity;
            Save();
            return updatedType;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
            Refresh();
        }

        public string FindNameById(int id)
        {
            return _categories.CachedItems.First(x => x.Id == id).Name;
        }

        public T FindOrCreateByName(string name)
        {
            var type = _categories.CachedItems.FirstOrDefault(x => x.Name == name);
            if (type == null)
            {
                var newType = Add(new T { Name = name });
                Save();                
                return newType;
            }
            return type;
        }

        public T? FindByName(string name)
        {
            var type = _categories.CachedItems.FirstOrDefault(x => x.Name == name);
            return type;
        }

        public Task<T?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

