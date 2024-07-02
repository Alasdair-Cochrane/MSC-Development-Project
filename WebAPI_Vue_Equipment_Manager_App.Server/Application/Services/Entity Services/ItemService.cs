using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text.Json;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _ItemRepository;
        private readonly ICategoryRepository<ItemStatusCategory> _statusRepository;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IEquipmentModelRepository _modelRepository;
        private readonly ICategoryRepository<EquipmentModelCategory> _modelCategories;
        private readonly IUnitRepository _unitRepository;
        private readonly IItemQueryBuilder _itemQueryBuilder;
        public ItemService(IItemRepository itemRepository, ICategoryRepository<ItemStatusCategory> categories,
            IMaintenanceService maintenanceService, IEquipmentModelRepository modelRepository, 
            ICategoryRepository<EquipmentModelCategory> modelCategories, IUnitRepository unitRepository,
            IItemQueryBuilder itemQueryBuilder)
        {
            _ItemRepository = itemRepository;
            _statusRepository = categories;
            _maintenanceService = maintenanceService;
            _modelRepository = modelRepository;
            _modelCategories = modelCategories;
            _unitRepository = unitRepository;
            _itemQueryBuilder = itemQueryBuilder;
        }

        public async Task<ItemDTO?> AddAsync(ItemDTO item)
        {
            int statusCategoryID = _statusRepository.FindOrCreateByName(item.CurrentStatus).Id;
            int modelCategoryID = _modelCategories.FindOrCreateByName(item.Model.Category).Id;

            EquipmentModel? model = await _modelRepository.FindOrCreate(item.Model.ToEntity(modelCategoryID));
            if(model == null)
            {
                string entity = JsonSerializer.Serialize(item);
                //throw an exception
                throw new DataInsertionException("failed to find or create model entry when adding item", entity);
            }
            int unitId = await _unitRepository.FindByName(item.UnitName);
            if(unitId == -1)
            {
                string entity = JsonSerializer.Serialize(item);

                throw new DataInsertionException("Specified admin unit of item does not exist", entity);
            }
            Item newItem = item.ToEntity(statusCategoryID);
            newItem.ModelId = model.Id;
            newItem.UnitId = unitId;
            var added = await _ItemRepository.AddAsync(newItem);
            if(added == null)
            {
                string entity = JsonSerializer.Serialize(item);

                throw new DataInsertionException("Failed to create item", entity);
            }
            return added.ToDTO();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var deleted = await _ItemRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ItemDTO>> GetAllAsync()
        {
            var all = await  _ItemRepository.GetAllAsync();
            return all.Select(x => x.ToDTO());         
        }

        public async Task<ItemDTO?> GetByIdAsync(int id)
        {
            var found = await _ItemRepository.GetAsync(id); 
            if(found == null)
            {
                return null;
            }
            return found.ToDTO();
        }

        public async Task<ItemDTO?> UpdateAsync(ItemDTO item)
        {
            int statusId = _statusRepository.FindOrCreateByName(item.CurrentStatus).Id;
            var updated = await _ItemRepository.UpdateAsync(item.ToEntity(statusId));
            if(updated == null)
            {
                return null;
            }
            return updated.ToDTO();
        }
        public async Task<IEnumerable<ItemDTO>> GetAllByUnitAsync(int UnitID)
        {
            var list = await _ItemRepository.GetAllByUnitIdAsync(UnitID);
            return list.Select(x => x.ToDTO());

        }

        public async Task SetImageUrl(int itemId, string url)
        {
           await  _ItemRepository.SetImageUrlAsync(itemId, url);
        }

        public async Task<string?> GetImageUrl(int id)
        {
            var url = await _ItemRepository.GetImageUrl(id);
            if (url == null) return null;
            return url;
        }

        public async Task<IEnumerable<ItemDTO>?> Search(ItemQuery query)
        {
            var items = await _itemQueryBuilder.QueryItems(query);
            List<ItemDTO>? result = new List<ItemDTO>();
            if (items.IsNullOrEmpty())
            {
                return null;
            }
            foreach(Item item in items)
            {
                result.Add(item.ToDTO());
            }
            return result;
        }

    }

    public interface IItemService
    {
        public Task<ItemDTO?> GetByIdAsync(int id);
        public Task<IEnumerable<ItemDTO>> GetAllAsync();
        public Task<ItemDTO?> UpdateAsync(ItemDTO item);
        public Task DeleteByIdAsync(int id);
        public Task<ItemDTO?> AddAsync(ItemDTO item);
        public Task<IEnumerable<ItemDTO>> GetAllByUnitAsync(int UnitID);
        public Task SetImageUrl(int itemId, string url);
        public Task<string?> GetImageUrl(int id);
        public Task<IEnumerable<ItemDTO>?> Search(ItemQuery query);

    }
}
