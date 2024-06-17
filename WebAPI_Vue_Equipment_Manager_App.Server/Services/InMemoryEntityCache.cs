using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Services
{
    /*
     This is a singleton (Handled by the DI container) that serves as a cache for the 'type' entities
    which will only ever have a small number of rows and will not be added, deleted or updated very often if at all.
     */
    public class InMemoryEntityCache<T> : IEntityCache<T>
    {
        public IEnumerable<T> CachedItems { get; set; }
    }

    public interface IEntityCache<T> {
        public IEnumerable<T> CachedItems { get; set; }
    }

    public class EquipmentCategoryCache : IEntityCache<EquipmentModelCategory>
    {
        public IEnumerable<EquipmentModelCategory> CachedItems { get; set; }
    }
    public class MaintenanceCategoryCache : IEntityCache<EquipmentModelCategory>
    {
        public IEnumerable<EquipmentModelCategory> CachedItems { get; set; }
    }

}
