using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{


    public class UserService : IUserService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly UserManager<User> _userManager;
        private User? _currentUser;

        public UserService(IAssignmentRepository assignmentRepository, IUnitRepository unitRepository, UserManager<User> manager)
        {
            _assignmentRepository = assignmentRepository;
            _unitRepository = unitRepository;
            _userManager = manager;
        }



        public async Task<IEnumerable<UnitDTO>> GetRelevantUnits(int userId)
        {
            var roots = await _unitRepository.GetAllAssignedRootsAsync(userId);
            List<UnitDTO> unitDTOs = new List<UnitDTO>();
            foreach(var root in roots)
            {
                unitDTOs.Add(await _unitRepository.GetDTOWithChildrenAsync(root.Id));
            }
            return unitDTOs;
        }

        public async Task<IEnumerable<AssignmentDTO>> GetUserAssignmentsDTObyEmailAsync(string email)
        {
            var assignments = await _assignmentRepository.GetUserAssingmentsDTObyEmailAsync(email);
            return assignments;
        }

        public async Task<IEnumerable<AssignmentDTO>> GetAssignmentsDTObyUserIdAsync(int userId)
        {
            var assignments = await _assignmentRepository.GetUserAssingmentsDTObyIdAsync(userId);
            return assignments;
        }

        public async Task<UserAssignment> AssignUser(int creatorId, int userId, int roleId, int unitId)
        {
            //check that the user requesting is an admin of the unit or any of the units parents
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(creatorId, unitId))
            {
                throw new UnauthorisedOperationException($"User : {userId} is not authorised to assign users for unit: {unitId}");
            }

            //it is unneccessary to assign a user as admin to a unit that are already an admin of or of its parent
            if (roleId == 1)
            {
                if (await _unitRepository.CheckUserIsAdminInParentOfUnit(userId, unitId))
                {
                    throw new UnauthorisedOperationException($"User : {userId} is already admin for unit: {unitId} or one of its units parents");
                }
            }

            var assignment = await _assignmentRepository.CreateAssignment(unitId, roleId, userId);

            return assignment;
        }

        public async Task<UserAssignment> AssignUserPublicOnCreate(int userId, int unitId)
        {
            return await _assignmentRepository.CreateAssignment(unitId, 3, userId);
        }

        public async Task<UserAssignment> UpdateAssignment(int updatorId, UserAssignment assignment)
        {
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(updatorId, assignment.UnitId))
            {
                throw new UnauthorisedOperationException($"User : {assignment.UserId} is not authorised to assign users for unit: {assignment.UnitId}");
            }
            var updatedAssignment = await _assignmentRepository.Update(assignment);

            return assignment;
        }

        public async Task DeleteAssignmentAsync(int deletorId, UserAssignment assignment)
        {
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(deletorId, assignment.UnitId))
            {
                throw new UnauthorisedOperationException($"User : {assignment.UserId} is not authorised to delete assignments for unit: {assignment.UnitId}");
            }
            await _assignmentRepository.Delete(assignment);
        }

        public async Task<User> GetCurrentUserAsync(HttpContext context)
        {
            if(_currentUser == null)
            {
                _currentUser = await _userManager.GetUserAsync(context.User);
                if(_currentUser == null)
                {
                    throw new UnauthorisedOperationException("Could not obtain current user from request");
                }
            }
            return _currentUser;
        }

        public async Task<UserDetailsDTO> GetUserDetailsAsync(User user)
        {
            var assignments = await _assignmentRepository.GetUserAssingmentsDTObyIdAsync(user.Id);
            var unitsDTO = new List<UnitDTO>();
            var units = await _unitRepository.GetAllRelevantUnitsToUserAsync(user.Id);

            foreach ( var unit in units.ToList() )
            {
                unitsDTO.Add(unit.ToDTO());
            }

            string email = user.Email ?? "";
            var dto = new UserDetailsDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = email,
                Assignments = assignments?.ToList(),
                Units = unitsDTO.ToList(),
            };
            return dto;
        }

    }
    public interface IUserService
    {
        Task<UserAssignment> AssignUser(int creatorId, int userID, int roleId, int unitId);
        Task<UserAssignment> AssignUserPublicOnCreate(int userId, int unitId);
        Task DeleteAssignmentAsync(int deletorId, UserAssignment assignment);
        Task<IEnumerable<AssignmentDTO>> GetAssignmentsDTObyUserIdAsync(int id);
        Task<User> GetCurrentUserAsync(HttpContext context);
        Task<IEnumerable<UnitDTO>> GetRelevantUnits(int userId);
        Task<IEnumerable<AssignmentDTO>> GetUserAssignmentsDTObyEmailAsync(string email);
        Task<UserDetailsDTO> GetUserDetailsAsync(User user);
        Task<UserAssignment> UpdateAssignment(int updatorId, UserAssignment assignment);
    }



}
