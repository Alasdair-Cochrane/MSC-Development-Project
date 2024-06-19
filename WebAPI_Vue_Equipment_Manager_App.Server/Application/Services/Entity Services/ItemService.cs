using System.Diagnostics;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
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

        public ItemService(IItemRepository itemRepository, ICategoryRepository<ItemStatusCategory> categories)
        {
            _ItemRepository = itemRepository;
            _statusRepository = categories;
        }

        public async Task<ItemDTO> AddAsync(ItemDTO item)
        {
            int categoryID = _statusRepository.FindOrCreateByName(item.CurrentStatus).Id;
            Item newItem = item.ToEntity(categoryID);
            var added = await _ItemRepository.AddAsync(newItem);
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
    }

    public interface IItemService
    {
        public Task<ItemDTO?> GetByIdAsync(int id);
        public Task<IEnumerable<ItemDTO>> GetAllAsync();
        public Task<ItemDTO?> UpdateAsync(ItemDTO item);
        public Task DeleteByIdAsync(int id);
        public Task<ItemDTO> AddAsync(ItemDTO item);

    }
}
