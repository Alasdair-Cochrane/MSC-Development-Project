using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _repository;
        public UnitService(IUnitRepository repository)
        {
            _repository = repository;
        }

        public async Task<UnitDTO?> AddAsync(UnitDTO newEntry)
        {
            var added = await _repository.AddAsync(newEntry.ToEntity());
            if(added == null)
            {
                return null;
            }
            return added.ToDTO();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UnitDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(x => x.ToDTO());
        }

      

        public async Task<UnitDTO?> GetAsync(int id)
        {
            var found = await _repository.GetAsync(id);
            if(found == null)
            {
                return null;
            }
            return found.ToDTO();
        }

        public async Task<UnitDTO?> UpdateAsync(UnitDTO updatedEntry)
        {
            var updated = await _repository.UpdateAsync(updatedEntry.ToEntity());
            if(updated == null)
            {
                return null;
            }
            return updated.ToDTO();
        }

        public async Task<IEnumerable<int>> GetChildrenById(int id)
        {
            return await _repository.GetChildrenIDsOfUnit(id);
        }
        public async Task<IEnumerable<int>> GetParentsById(int id)
        {
            return await _repository.GetParentIDsofUnit(id);
        }

    }

    public interface IUnitService
    {
            public Task<UnitDTO?> AddAsync(UnitDTO newEntry);
            public Task<UnitDTO?> UpdateAsync(UnitDTO updatedEntry);
            public Task DeleteAsync(int id);
            public Task<UnitDTO?> GetAsync(int id);
            public Task<IEnumerable<UnitDTO>> GetAllAsync();
            
            public Task<IEnumerable<int>> GetChildrenById(int id);
        public Task<IEnumerable<int>> GetParentsById(int id);

    }
}
