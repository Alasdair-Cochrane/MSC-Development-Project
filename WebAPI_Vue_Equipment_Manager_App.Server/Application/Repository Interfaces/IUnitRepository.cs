using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces
{
    public interface IUnitRepository : IRepository<Unit>
    {
        Task<bool> CheckUserHasRoleInUnit(int userId, IEnumerable<int> roleId, int unitId);
        Task<bool> CheckUserIsAdminInParentOfUnit(int userId, int unitId);
        public Task<Unit?> FindByName(string name);
        Task<IEnumerable<Unit>> GetAllRelevantUnitsToUserAsync(int userId);
        Task<IEnumerable<int>> GetAllUnitIdsWhereUserIsAdmin(int userId);
        public Task<IEnumerable<int>> GetChildrenIDsOfUnit(int parentId);
        Task<IEnumerable<Unit>> GetChildrenofUnit(int parentId);
        public Task<IEnumerable<int>> GetParentIDsofUnit(int childId);
        Task<IEnumerable<Unit>> GetParentsofUnit(int childId);
        Task<IEnumerable<int>> GetUnitIdsWithUserRole(int userId, int roleId);
        public Task<IEnumerable<Unit>> GetAllPublicUnitsAsync();
        Task<Unit?> GetRootUnitAsync(int unitId);
        Task<IEnumerable<User>> GetAllAssignedUsersAsync(int unitId, int userId);
        Task<IEnumerable<AssignmentDTO>> GetUserAssignmentsForUnitAsync(int unitId);
        Task<IEnumerable<UnitDTO>> GetAllAdminRoleUnits(int userId);
        Task<IEnumerable<Unit>> GetAllAssignedRootsAsync(int userId);
        Task<UnitDTO> GetDTOWithChildrenAsync(Unit unit);
        Task<bool> CheckUserHasRoleInParentOfUnit(int userId, IEnumerable<int> roleIds, int unitId);
        Task<IEnumerable<int>> GetAllRelevantUnitIdsToUserAsync(int userId);
    }
}
