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
            int categoryID = _statusRepository.FindOrCreateByName(item.Current_Status).Id;
            var added = await _ItemRepository.AddAsync(item.ToEntity(categoryID));
            return added.ToDTO();
        }

        public async Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ItemRepository>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ItemDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemDTO> UpdateAsync(ItemDTO item)
        {
            throw new NotImplementedException();
        }
    }

    public interface IItemService
    {
        public Task<ItemDTO> GetByIdAsync(int id);
        public Task<IEnumerable<ItemRepository>> GetAllAsync();
        public Task<ItemDTO> UpdateAsync(ItemDTO item);
        public Task DeleteByIdAsync(int id);
        public Task<ItemDTO> AddAsync(ItemDTO item);

    }
}
