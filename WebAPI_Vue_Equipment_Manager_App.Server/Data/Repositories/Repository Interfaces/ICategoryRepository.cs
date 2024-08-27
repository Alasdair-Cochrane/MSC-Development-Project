using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces
{
    public interface ICategoryRepository<T> : IRepository<T> where T : Category
    {
        T? FindByName(string name);
        T FindOrCreateByName(string name);
    }
}