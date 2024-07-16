using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces
{
    public interface IUnitRepository : IRepository<Unit>
    {
        public Task<int> FindByName(string name);
        public Task<IEnumerable<int>> GetChildrenIDsOfUnit(int parentId);
        public Task<IEnumerable<int>> GetParentIDsofUnit(int childId);

    }
}
