using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public interface IAssignmentRepository
    {
        Task<AssignmentDTO?> CreateAssignment(UserAssignment assignment);
        Task Delete(UserAssignment assignment);
        Task<IEnumerable<int>> GetAssignedRootUnitIds(int userId);
        Task<IEnumerable<AssignmentDTO>> GetUserAssignmentsForUnitAsync(int unitId);
        Task<IEnumerable<AssignmentDTO>> GetUserAssingmentsDTObyIdAsync(int userId);
        Task<AssignmentDTO?> Update(UserAssignment assignement);
    }
}