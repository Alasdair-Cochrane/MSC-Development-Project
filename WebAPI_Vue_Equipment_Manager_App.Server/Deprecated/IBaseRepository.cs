using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Deprecated
{
    [Obsolete("generic no longer used")]
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        T? GetById(int id);
        IQueryable<T> GetAllQueryable();
        IEnumerable<T> GetAll();
        T Add(T entity);
        void Delete(int id);
        T? Update(T entity);

        void Save();
    }
}
