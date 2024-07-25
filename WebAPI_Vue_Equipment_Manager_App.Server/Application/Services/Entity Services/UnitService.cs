using Microsoft.EntityFrameworkCore.Update;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IAssignmentRepository _assignmentRepository;
        public UnitService(IUnitRepository repository, IAssignmentRepository assignmentRepository)
        {
            _unitRepository = repository;
            _assignmentRepository = assignmentRepository;
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
                //if the user is already an admin of a parent unit do not reassign them
                if(await _unitRepository.CheckUserIsAdminInParentOfUnit(userId, added.Id))
                {
                    return added.ToDTO();
                }
                //assign the requestor as admin of the unit they created
                await _assignmentRepository.CreateAssignment(new UserAssignment { UnitId = added.Id, RoleId = 1, UserId = userId });

                return added.ToDTO();
            }
            catch (Exception ex)
            {
                throw new UnitException(ex.Message, ex);
            }

        }
        public async Task<IEnumerable<UnitDTO>> GetPublicUnitsAsync()
        {
            var units = await _unitRepository.
                GetAllPublicUnitsAsync();
            var dtos = units.Select(x => x.ToDTO()).ToList();
            return dtos;
        }

        public async Task DeleteAsync(int id, int userId)
        {
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(userId, id))
            {
                throw new UnauthorisedOperationException($"User : {userId} is not authorised to update unit: {id}");
            }
            Unit? unit = await _unitRepository.GetAsync(id);
            if(unit == null)
            {
                return;
            }
            //if it has no children then just delete
            if( unit.Children.IsNullOrEmpty())
            {
                await _unitRepository.DeleteAsync(id);
                return;
            }
            //if it has children and a parent - then assign its children to its parent
            //Make sure that the user is assigned the admin of the children
            //EF CORE will track the changes to the children and save when savechanges in DeleteAsync is called
                var tasks = new List<Task>();
                foreach(var child in unit.Children)
                {
                    if(unit.ParentId != null || unit.ParentId != 0)
                    {
                        child.ParentId = unit.ParentId;

                    }
                    tasks.Add( _assignmentRepository.CreateAssignmentDelayedSaveAsync(new UserAssignment { UserId = userId, UnitId = child.Id, RoleId = 1 }));
                }

                await Task.WhenAll(tasks);

                await _unitRepository.DeleteAsync(id);
                return;
        }

        public async Task<IEnumerable<UnitDTO>> GetAllAsync(int userId)
        {
            var units = await _unitRepository.GetAllRelevantUnitsToUserAsync(userId);
            return units.Select(x => x.ToDTO());
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
            if (unit == null)
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

        public async Task<IEnumerable<AssignmentDTO>> GetUserAssignments(int unitId)
        {
            return await _unitRepository.GetUserAssignmentsForUnitAsync(unitId);
        }

        public async Task<IEnumerable<UnitDTO>> GetAdminRoleUnits(int userId)
        {
            return await _unitRepository.GetAllAdminRoleUnits(userId);
        }

    }
    public interface IUnitService
    {
        Task<UnitDTO?> AddAsync(UnitDTO newEntry, int userId);
        Task DeleteAsync(int id, int userId);
        Task<IEnumerable<UnitDTO>> GetAllAsync(int userId);
        Task<UnitDTO?> GetAsync(int id);
        Task<IEnumerable<Unit>> GetChildrenById(int id);
        Task<IEnumerable<Unit>> GetParentsById(int id);
        Task<UnitDTO?> UpdateAsync(UnitDTO updatedEntry, int userId);
        public Task<IEnumerable<UnitDTO>> GetPublicUnitsAsync();
        Task<UnitDTO?> FindByName(string name);
        Task<IEnumerable<AssignmentDTO>> GetUserAssignments(int unitId);
        Task<IEnumerable<UnitDTO>> GetAdminRoleUnits(int userId);
    }


}
