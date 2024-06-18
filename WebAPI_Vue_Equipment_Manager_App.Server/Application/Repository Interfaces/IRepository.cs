using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        public T Add(T newModel);
        public Task<T?> UpdateAsync(T updatedModel);
        public bool Delete(int id);
        public Task<T?> GetAsync(int id);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task SaveAsync();
    }
}
