using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories.Interfaces
{
    public interface ICategoryRepository<T>  where T : Category
    {
        Task<T?> GetByIdAsync(int id);
        T? GetById(int id);
        IQueryable<T> GetAllQueryable();
        IEnumerable<T> GetAll();
        T Add(T entity);
        void Delete(int id);
        T Update(T entity);
        void Save();
        T? FindByName(string name);
        T FindOrCreateByName(string name);        
    }
}