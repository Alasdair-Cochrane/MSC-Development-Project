using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces
{
    public interface IMaintenanceRepository : IRepository<Maintenance>
    {
        Task<IEnumerable<MaintenanceDTO>> GetAllInTimePeriodAsync(int days, IEnumerable<int> unitIds);
        public Task<IEnumerable<Maintenance>> GetAllByItemIdAsync(int id);
        Task<IEnumerable<string>> GetAllCategoryNamesAsync();
    }
}
