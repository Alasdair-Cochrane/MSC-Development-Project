using Microsoft.EntityFrameworkCore.Update;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services
{
    

    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IUserRepository _userRepository;
        public UnitService(IUnitRepository repository, IUserRepository userRepository)
        {
            _unitRepository = repository;
            _userRepository = userRepository;
        }

        public async Task<UnitDTO?> AddAsync(UnitDTO newEntry, int userId)
        {
            try
            {
                var added = await _unitRepository.AddAsync(newEntry.ToEntity());
                if (added == null)
                {
                    throw new UnitException("Database insertion failed");
                }

                //assign the requestor as admin of the unit they created
                await _userRepository.CreateAssignment(added.Id, 1, userId);

                return added.ToDTO();
            }
            catch (Exception ex)
            {
                throw new UnitException(ex.Message, ex);
            }

        }
        public async Task<IEnumerable<UnitDTO>> GetRootUnitsAsync()
        {
            var units = await _unitRepository.
                GetAllTopLevelUnitsAsync();
            var dtos = units.Select(x => x.ToDTO()).ToList();
            return dtos;
        }

        public async Task DeleteAsync(int id, int userId)
        {
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(userId, id))
            {
                throw new UnauthorisedOperationException($"User : {userId} is not authorised to update unit: {id}");
            }
            await _unitRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UnitDTO>> GetAllAsync()
        {
            var list = await _unitRepository.GetAllAsync();
            return list.Select(x => x.ToDTO());
        }



        public async Task<UnitDTO?> GetAsync(int id)
        {
            var found = await _unitRepository.GetAsync(id);
            if (found == null)
            {
                return null;
            }
            return found.ToDTO();
        }

        public async Task<UnitDTO?> UpdateAsync(UnitDTO updatedEntry, int userId)
        {
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(userId, updatedEntry.Id.Value))
            {
                throw new UnauthorisedOperationException($"User : {userId} is not authorised to update unit: {updatedEntry.Id}");
            }

            var updated = await _unitRepository.UpdateAsync(updatedEntry.ToEntity());
            if (updated == null)
            {
                return null;
            }
            return updated.ToDTO();
        }

        public async Task<UnitDTO?> FindByName(string name)
        {
            var unit = await _unitRepository.FindByName(name);
            if(unit == null)
            {
                return null;
            }
            return unit.ToDTO();
        }

        public async Task<IEnumerable<Unit>> GetChildrenById(int id)
        {
            return await _unitRepository.GetChildrenofUnit(id);
        }
        public async Task<IEnumerable<Unit>> GetParentsById(int id)
        {
            return await _unitRepository.GetParentsofUnit(id);
        }

    }
    public interface IUnitService
    {
        Task<UnitDTO?> AddAsync(UnitDTO newEntry, int userId);
        Task DeleteAsync(int id, int userId);
        Task<IEnumerable<UnitDTO>> GetAllAsync();
        Task<UnitDTO?> GetAsync(int id);
        Task<IEnumerable<Unit>> GetChildrenById(int id);
        Task<IEnumerable<Unit>> GetParentsById(int id);
        Task<UnitDTO?> UpdateAsync(UnitDTO updatedEntry, int userId);
        public Task<IEnumerable<UnitDTO>> GetRootUnitsAsync();
        Task<UnitDTO?> FindByName(string name);
    }


}
