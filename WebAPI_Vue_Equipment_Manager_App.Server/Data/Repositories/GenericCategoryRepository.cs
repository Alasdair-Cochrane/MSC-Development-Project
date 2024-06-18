using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class GenericCategoryRepository<T> : Repository<T> , ICategoryRepository<T> where T : Category, new()
    {
        private readonly DbSet<T> _contextSet;
        private readonly DbContext _dbContext;
        private readonly IEntityCache<T> _categoriesCache;
        public GenericCategoryRepository(MainDbContext context, IEntityCache<T> cache) : base(context)
        {
            _contextSet = context.Set<T>();
            _categoriesCache = cache;
            _dbContext = context;

            if (_categoriesCache.CachedItems == null)
            {
                _categoriesCache.CachedItems = _contextSet.ToList();
            }
        }

        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            return  await Task.Run(() => _categoriesCache.CachedItems);
        }
        public IEnumerable<T> GetAll() { 
            return _categoriesCache.CachedItems; 
        }

        public override T Add(T type)
        {
            T newEntry = _contextSet.Add(type).Entity;
            return newEntry;
        }
        public override bool Delete(int id)
        {
            var toBeDeleted = GetById(id);
            if (toBeDeleted != null)
            {
                _contextSet.Remove(toBeDeleted);
                return true;
            }
            return false;
        }

        public T GetById(int id)
        {
            return _categoriesCache.CachedItems.First(type => type.Id == id);
        }

        public async override Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
            _categoriesCache.CachedItems = _contextSet.ToList();
            return;
        }

        public string FindNameById(int id)
        {
            return _categoriesCache.CachedItems.First(x => x.Id == id).Name;
        }

        public T FindOrCreateByName(string name)
        {
            var type = _categoriesCache.CachedItems.FirstOrDefault(x => x.Name == name);
            if (type == null)
            {
                var newType = Add(new T { Name = name });
                _dbContext.SaveChanges();
                _categoriesCache.CachedItems = _contextSet.ToList();
                return newType;
            }
            return type;
        }

        public T? FindByName(string name)
        {
            var type = _categoriesCache.CachedItems.FirstOrDefault(x => x.Name == name);
            return type;
        }

    }
}

