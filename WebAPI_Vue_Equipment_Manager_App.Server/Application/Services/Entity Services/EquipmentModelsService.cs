using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    public class EquipmentModelsService : IEquipmentModelService
    {
        IEquipmentModelRepository _repository;
        ICategoryRepository<EquipmentModelCategory> _typeRepository;
        IUnitRepository _unitRepository;

        public EquipmentModelsService(IEquipmentModelRepository repo, ICategoryRepository<EquipmentModelCategory> types, IUnitRepository unitRepository)
        {
            _repository = repo;
            _typeRepository = types;
            _unitRepository = unitRepository;
        }

        public async Task<EquipmentModelDTO?> AddAsync(EquipmentModelDTO model)
        {
            EquipmentModelCategory category = _typeRepository.FindOrCreateByName(model.Category);

            EquipmentModel newModel = model.ToEntity(category.Id);
            var added = await _repository.AddAsync(newModel);
            if(added == null)
            {
                return null;
            }
            return added.ToDTO();
        }

        public async Task<IEnumerable<EquipmentModelDTO>> GetAllAsync()
        {
            IEnumerable<EquipmentModel> models = await _repository.GetAllAsync();
            return models.Select(x => x.ToDTO());
        }


        public async Task<EquipmentModelDTO?> UpdateAsync(EquipmentModelDTO model)
        {
            var category = _typeRepository.FindOrCreateByName(model.Category);
            var newModel = await _repository.UpdateAsync(model.ToEntity(category.Id));
            if (newModel == null) return null;
            return newModel.ToDTO();
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync(int userId)
        {
            var units = await _unitRepository.GetAllRelevantUnitsToUserAsync(userId, false);
            var uIds = units.Select(u => u.Id);

            var userCategories = await _repository.GetUserCategoriesAsync(uIds);
            HashSet<string> categories = new HashSet<string>(userCategories);
            categories.UnionWith(SeedData.EquipmentModelCategories);
            return categories.ToArray();
        }

        public async Task AddMany(IEnumerable<EquipmentModelDTO> models)
        {
            List<EquipmentModel> entities = new List<EquipmentModel>();
           foreach (var model in models)
            {
                model.CategoryId = _typeRepository.FindOrCreateByName(model.Category).Id;
                entities.Add(model.ToEntity());
            }
           await _repository.AddManyAsync(entities);
        }
    }



    public interface IEquipmentModelService
    {

        public Task<IEnumerable<EquipmentModelDTO>> GetAllAsync();
        public Task<EquipmentModelDTO?> AddAsync(EquipmentModelDTO entity);
        public Task<EquipmentModelDTO?> UpdateAsync(EquipmentModelDTO entity);
    Task<IEnumerable<string>> GetCategoriesAsync(int userId);
        Task AddMany(IEnumerable<EquipmentModelDTO> models);
    }

}
