using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public interface IUserRepository
    {
        Task<UserAssignment> CreateAssignment(int unitId, int roleId, int userId);
        Task Delete(int unitId, int roleId, int userId);
        Task<IEnumerable<UserAssignment>> GetUserAssignmentsbyEmailAsync(string email);
        Task<IEnumerable<AssignmentDTO>> GetUserAssingmentsDTObyEmailAsync(string email);
        Task<IEnumerable<AssignmentDTO>> GetUserAssingmentsDTObyIdAsync(int userId);
        Task<int> GetUserIdByEmail(string email);
        Task<UserAssignment> Update(int unitId, int roleId, int userId);
    }
}