using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    //new means it must have a parameterless constructuctor
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

            if (_categoriesCache.CachedItems.IsNullOrEmpty())
            {
                _categoriesCache.CachedItems = _contextSet.ToList();
            }
        }

        public async Task<IEnumerable<T>> GetAllWithNavPropertiesAsync()
        {
            return await GetAllAsync();
        }
        public override async Task<IEnumerable<T>> GetAllAsync() {
            return await Task.Run(() =>
            {
                return _categoriesCache.CachedItems.ToList();
            }) ; 
        }

        public override async Task<T?> AddAsync(T type)
        {
            T newEntry = _contextSet.Add(type).Entity;
           _categoriesCache.Add(newEntry);
            await _context.SaveChangesAsync();
            return newEntry;
        }

        private T Add(T type)
        {
            T newEntry = _contextSet.Add(type).Entity;
            _categoriesCache.Add(newEntry);
            _context.SaveChanges();
            return newEntry;
        }
        public async override Task<bool> DeleteAsync(int id)
        {
            var toBeDeleted = _categoriesCache.CachedItems.First(type => type.Id == id);
            if (toBeDeleted != null)
            {
                _contextSet.Remove(toBeDeleted);
                _categoriesCache.Remove(toBeDeleted);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async override Task<T?> GetAsync(int id)
        {
            return await Task.Run(() => { 
                return _categoriesCache.CachedItems.First(type => type.Id == id); }
            );
        }

        public async Task<T?> GetWithNavPropertiesAsync(int id)
        {
            return await GetAsync(id);
        }

        public async override Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
            _categoriesCache.CachedItems = _contextSet.ToList();
            return;
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

