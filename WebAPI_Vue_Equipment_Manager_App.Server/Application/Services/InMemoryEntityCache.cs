using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    /*
     This is a singleton (Handled by the DI container) that serves as a cache for the 'type' entities
    which will only ever have a small number of rows and will not be added, deleted or updated very often if at all.
     */
    public class InMemoryEntityCache<T> : IEntityCache<T>
    {
        public ICollection<T> CachedItems { get; set; }
        public void Add(T entity) {
            CachedItems.Add(entity);
        }
        public void Remove(T entity)
        {
            CachedItems.Remove(entity);
        }
    }

    public interface IEntityCache<T>
    {
        public ICollection<T> CachedItems { get; set; }

        public void Add(T entity);
        public void Remove(T entity);
    }

}
