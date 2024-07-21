using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public interface IAssignmentRepository
    {
        Task<UserAssignment> CreateAssignment(int unitId, int roleId, int userId);
        Task Delete(UserAssignment assignment);
        Task<IEnumerable<UserAssignment>> GetUserAssignmentsbyEmailAsync(string email);
        Task<IEnumerable<AssignmentDTO>> GetUserAssignmentsForUnitAsync(int unitId);
        Task<IEnumerable<AssignmentDTO>> GetUserAssingmentsDTObyEmailAsync(string email);
        Task<IEnumerable<AssignmentDTO>> GetUserAssingmentsDTObyIdAsync(int userId);
        Task<UserAssignment> Update(UserAssignment assignement);
    }
}