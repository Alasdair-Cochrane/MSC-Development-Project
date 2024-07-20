using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces
{
    public interface IUnitRepository : IRepository<Unit>
    {
        Task<bool> CheckUserHasRoleInUnit(int userId, int roleId, int unitId);
        Task<bool> CheckUserIsAdminInParentOfUnit(int userId, int unitId);
        public Task<Unit?> FindByName(string name);
        Task<IEnumerable<Unit>> GetAllRelevantUnitsToUser(int userId);
        Task<IEnumerable<int>> GetAllUnitIdsWhereUserIsAdmin(int userId);
        public Task<IEnumerable<int>> GetChildrenIDsOfUnit(int parentId);
        Task<IEnumerable<Unit>> GetChildrenofUnit(int parentId);
        public Task<IEnumerable<int>> GetParentIDsofUnit(int childId);
        Task<IEnumerable<Unit>> GetParentsofUnit(int childId);
        Task<IEnumerable<int>> GetUnitIdsWithUserRole(int userId, int roleId);
        public Task<IEnumerable<Unit>> GetAllTopLevelUnitsAsync();
    }
}
