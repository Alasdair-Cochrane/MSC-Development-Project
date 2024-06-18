using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Deprecated;
using WebAPI_Vue_Equipment_Manager_App.Server.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.DTOs.Mappings;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Services
{
    public class ModelsService : IModelService
    {
        IEquipmentModelRepository _repository;
        ICategoryRepository<EquipmentModelCategory> _typeRepository;

        public ModelsService(IEquipmentModelRepository repo, ICategoryRepository<EquipmentModelCategory> types) {
            _repository = repo;
            _typeRepository = types;      
        }

        public EquipmentModelDTO Add(EquipmentModelDTO model)
        {
            EquipmentModelCategory category = _typeRepository.FindOrCreateByName(model.Category);

            EquipmentModel newModel = model.ToEntity(category.Id);
            _repository.Add(newModel);
            newModel.Category = category;
            return newModel.ToDTO();
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public async Task<IEnumerable<EquipmentModelDTO>> GetAllAsync()
        {
            IEnumerable<EquipmentModel> models = await _repository.GetAllAsync();
            return models.Select(x => x.ToDTO());
        }

        public async Task<EquipmentModelDTO?> GetByiDAsync(int id)
        {
            var element = await _repository.GetAsync(id);
            if (element == null) return null;
            return element.ToDTO();
        }

        public async Task<EquipmentModelDTO?> UpdateAsync(EquipmentModelDTO model)
        {
            var category = _typeRepository.FindOrCreateByName(model.Category);
            var newModel = await _repository.UpdateAsync(model.ToEntity(category.Id));
            if(newModel == null) return null;
            await _repository.SaveAsync();
            newModel.Category = category;
            return newModel.ToDTO();
        }
    }

    public interface IModelService {

        public Task<EquipmentModelDTO?> GetByiDAsync(int id);
        public Task<IEnumerable<EquipmentModelDTO>> GetAllAsync();
        public EquipmentModelDTO Add(EquipmentModelDTO entity);
        public Task<EquipmentModelDTO?> UpdateAsync(EquipmentModelDTO entity);
        public void Delete(int id);   
    
    }

}
