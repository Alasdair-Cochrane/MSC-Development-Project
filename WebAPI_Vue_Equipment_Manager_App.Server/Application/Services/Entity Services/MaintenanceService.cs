using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _repository;
        private readonly ICategoryRepository<MaintenanceCategory> _categoryRepository;
        private readonly IDocumentService _documentService;
        private readonly DocumentRepository _documentRepository;
        private readonly IUnitRepository _unitRepository;

        public MaintenanceService(IMaintenanceRepository repository, ICategoryRepository<MaintenanceCategory> categoryRepository, 
            IDocumentService documentService, DocumentRepository documentRepository,
            IUnitRepository unitRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _documentService = documentService;
            _documentRepository = documentRepository;
            _unitRepository = unitRepository;

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
            var result = await _repository.DeleteAsync(id);
            if (!result)
            {
                throw new Exception("Could not delete Maintenance");
            }
        }

        public async Task<IEnumerable<MaintenanceDTO>> GetAllAsync(int days, int userId)
        {
            var unitIds = await _unitRepository.GetAllRelevantUnitIdsToUserAsync(userId);
            var list = await _repository.GetAllAsync(days, unitIds);
            return list;
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

        public async Task<IEnumerable<string>> GetCategoryNamesAsync()
        {
            return await _repository.GetAllCategoryNamesAsync();
        }

        public async Task<MaintenanceDocument> CreateDocumentAsync(Document doc, int id)
        {
            var result = await _documentRepository.AddMaintenanceDocument(doc, id);
            return result;

        }
        public async Task<IEnumerable<MaintenanceDocument>> GetAllMaintenanceFilesForItemAsync(int itemId)
        {
           var maintenances = await _repository.GetAllByItemIdAsync(itemId);
           var records = await _documentRepository.GetAllDocumentsForMaintenance(maintenances.Select(x => x.Id));
           return records;
        }
        public async Task<IEnumerable<MaintenanceDocument>> GetDocumentsAsync(int id)
        {
            var documents = await _documentRepository.GetAllDocumentsForMaintenance(id);
            return documents;
        }

    }

        public interface IMaintenanceService {
        public Task<MaintenanceDTO?> AddAsync(MaintenanceDTO newEntry);
        public Task<MaintenanceDTO?> UpdateAsync(MaintenanceDTO updatedEntry);
        public Task DeleteAsync(int id);
        public Task<MaintenanceDTO?> GetAsync(int id);
        public Task<IEnumerable<MaintenanceDTO>> GetAllAsync(int days, int userId);
        public Task<IEnumerable<MaintenanceDocument>> GetAllMaintenanceFilesForItemAsync(int itemId);
        Task<IEnumerable<string>> GetCategoryNamesAsync();
        Task<MaintenanceDocument> CreateDocumentAsync(Document doc, int id);
        Task<IEnumerable<MaintenanceDTO>> GetAllForItemAsync(int itemId);
        Task<IEnumerable<MaintenanceDocument>> GetDocumentsAsync(int id);
    }

}
