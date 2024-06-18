using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces
{
    public interface IEquipmentModelRepository : IRepository<EquipmentModel>
    {
        public Task<IEnumerable<EquipmentModel>> GetAllWithNavPropertiesAsync();

        public Task<EquipmentModel?> GetWithNavPropertiesAsync(int id);
    }
}
