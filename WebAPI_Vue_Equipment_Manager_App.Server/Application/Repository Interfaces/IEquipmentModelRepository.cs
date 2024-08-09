using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces
{
    public interface IEquipmentModelRepository : IRepository<EquipmentModel>
    {
        public Task<EquipmentModel> UpsertbyModelNumberAsync(EquipmentModel model);
        Task<IEnumerable<string>> GetUserCategoriesAsync(IEnumerable<int> unitIds);
        Task AddManyAsync(IEnumerable<EquipmentModel> models);
    }
}
