using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public interface IItemQueryBuilder
    {
        Task<IEnumerable<Item>?> QueryItems(ItemQuery queryObject, int userId);
    }
}