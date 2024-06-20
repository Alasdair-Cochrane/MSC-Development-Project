using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _repository;
        private readonly ICategoryRepository<MaintenanceCategory> _categoryRepository;

        public MaintenanceService(IMaintenanceRepository repository, ICategoryRepository<MaintenanceCategory> categoryRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;

        }
        public async Task<MaintenanceDTO?> AddAsync(MaintenanceDTO newEntry)
        {
            int categoryId = _categoryRepository.FindOrCreateByName(newEntry.CategoryName).Id;
            var toAdd = newEntry.ToEntity(categoryId);
            var added = await _repository.AddAsync(toAdd);
            if (added == null)
            {
                return null;
            }
            return added.ToDTO();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<MaintenanceDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();

            return list.Select(x => x.ToDTO());
        }

        public async Task<IEnumerable<MaintenanceDTO>> GetAllForItemAsync(int itemId)
        {
            var itemList = await _repository.GetAllByItemIdAsync(itemId);
            return itemList.Select(x => x.ToDTO());
        }

        public async Task<MaintenanceDTO?> GetAsync(int id)
        {
            var found = await _repository.GetAsync(id);
            if (found == null)
            {
                return null;
            }
            return found.ToDTO();
        }

        public async Task<MaintenanceDTO?> UpdateAsync(MaintenanceDTO updatedEntry)
        {
            int categoryId = _categoryRepository.FindOrCreateByName(updatedEntry.CategoryName).Id;
            var toUpdate = updatedEntry.ToEntity(categoryId);
            var updated = await _repository.UpdateAsync(toUpdate);
            if (updated == null)
            {
                return null;
            }
            return updated.ToDTO();
        }
    }

        public interface IMaintenanceService {
        public Task<MaintenanceDTO?> AddAsync(MaintenanceDTO newEntry);
        public Task<MaintenanceDTO?> UpdateAsync(MaintenanceDTO updatedEntry);
        public Task DeleteAsync(int id);
        public Task<MaintenanceDTO?> GetAsync(int id);
        public Task<IEnumerable<MaintenanceDTO>> GetAllAsync();
        public Task<IEnumerable<MaintenanceDTO>> GetAllForItemAsync(int itemId);

    }

}
