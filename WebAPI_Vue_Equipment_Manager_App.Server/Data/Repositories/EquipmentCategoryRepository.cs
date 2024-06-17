using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Services;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    /*
    Since EquipmentType entities will be added/removed very infrequently and will be small in number,
    It will  be more efficient to cache them in memory rather than access the database for every query. 
     */
    public class EquipmentCategoryRepository : ICategoryRepository<EquipmentModelCategory>
    {
        private readonly MainDbContext _context;
        private readonly IEntityCache<EquipmentModelCategory> _equipmentTypesCache;
        public EquipmentCategoryRepository(MainDbContext context, IEntityCache<EquipmentModelCategory> cache)
        {
            _context = context;
            _equipmentTypesCache = cache;

            if (_equipmentTypesCache.CachedItems == null)
            {
                _equipmentTypesCache.CachedItems = context.EquipmentTypes.ToList<EquipmentModelCategory>();
            }
        }

        private void Refresh()
        {
            _equipmentTypesCache.CachedItems = _context.EquipmentTypes.ToList();
        }

        public IQueryable<EquipmentModelCategory> GetAllQueryable()
        {
            return _equipmentTypesCache.CachedItems.AsQueryable();
        }

        public IEnumerable<EquipmentModelCategory> GetAll()
        {
            return _equipmentTypesCache.CachedItems;
        }


        public EquipmentModelCategory Add(EquipmentModelCategory type)
        {
            EquipmentModelCategory newEntry = _context.Add(type).Entity;
            return newEntry;
        }
        public void Delete(int id)
        {
            var toBeDeleted = GetById(id);
            if (toBeDeleted != null)
            {
                _context.EquipmentTypes.Remove(toBeDeleted);
                Save();
            }
            
        }

        public EquipmentModelCategory GetById(int id)
        {
            return _equipmentTypesCache.CachedItems.First(type => type.Id == id);
        }

        public EquipmentModelCategory Update(EquipmentModelCategory type)
        {
            EquipmentModelCategory updatedType = _context.EquipmentTypes.Update(type).Entity;
            return updatedType;
        }

        public void Save()
        {
            _context.SaveChanges();
            Refresh();
        }

        public string FindNameById(int id)
        {
            return _equipmentTypesCache.CachedItems.First(x => x.Id == id).Name;
        }

        public EquipmentModelCategory FindOrCreateByName(string name)
        {
            var type = _equipmentTypesCache.CachedItems.First(x => x.Name == name);
            if (type == null)
            {
                var newType = Add(new EquipmentModelCategory { Name = name });
                Save();
                return newType;
            }

            return type;
        }

        public EquipmentModelCategory? FindByName(string name)
        {
            var type = _equipmentTypesCache.CachedItems.First(x => x.Name == name);
            return type;
        }

        public Task<EquipmentModelCategory?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
