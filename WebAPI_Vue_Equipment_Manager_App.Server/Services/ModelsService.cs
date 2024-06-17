using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.DTOs.Mappings;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Services
{
    public class ModelsService : IModelService
    {
        IBaseRepository<EquipmentModel> _repository;
        ICategoryRepository<EquipmentModelCategory> _typeRepository;

        public ModelsService(IBaseRepository<EquipmentModel> repo, ICategoryRepository<EquipmentModelCategory> types) {
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

        public IEnumerable<EquipmentModelDTO> GetAll()
        {
            var models = _repository.GetAllQueryable().Include(x => x.Category).
                Select(x => x.ToDTO());
            return models;
        }

        public EquipmentModelDTO? GetByiD(int id)
        {
            var element = _repository.GetById(id);
            if (element == null) return null;
            return element.ToDTO();
        }

        public EquipmentModelDTO? Update(EquipmentModelDTO model)
        {
            var category = _typeRepository.FindOrCreateByName(model.Category);
            var newModel = _repository.Update(model.ToEntity(category.Id));
            if(newModel == null) return null;
            _repository.Save();
            newModel.Category = category;
            return newModel.ToDTO();
        }
    }

    public interface IModelService {

        public EquipmentModelDTO? GetByiD(int id);
        public IEnumerable<EquipmentModelDTO> GetAll();
        public EquipmentModelDTO Add(EquipmentModelDTO entity);
        public EquipmentModelDTO? Update(EquipmentModelDTO entity);
        public void Delete(int id);   
    
    }

}
