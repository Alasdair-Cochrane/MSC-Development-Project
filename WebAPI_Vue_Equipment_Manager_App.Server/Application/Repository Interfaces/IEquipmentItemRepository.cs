using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces
{
    public interface IEquipmentItemRepository : IRepository<Item>
    {
        public Task<IEnumerable<EquipmentModel>> GetAllWithNavPropertiesAsync();
        public Task<EquipmentModel?> GetWithNavPropertiesAsync(int id);
    }
}
