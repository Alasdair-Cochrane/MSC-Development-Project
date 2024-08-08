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
        private readonly IUserRepository _userRepository;
        private User? _currentUser;

        public UserService(IAssignmentRepository assignmentRepository, 
            IUnitRepository unitRepository, 
            UserManager<User> manager,
            IUserRepository userRepository)
        {
            _assignmentRepository = assignmentRepository;
            _unitRepository = unitRepository;
            _userManager = manager;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDetailsDTO>> GetAllPublicUsers(int userId)
        {
            //get all the units that the user belongs to
            var rootUnits = await _unitRepository.GetAllRelevantUnitIdsToUserAsync(userId,false);

            //get all the users who are assigned to those units
            HashSet<User> users = new HashSet<User>();
            foreach(var unit in rootUnits)
            {
                users.UnionWith(await _unitRepository.GetAllAssignedUsersAsync(unit, userId ));
            }
            IEnumerable<UserDetailsDTO> dtos = users.Select(x => x.ToDto());

            return dtos;
        }

        public async Task<IEnumerable<UnitDTO>> GetRelevantUnits(int userId)
        {
            var roots = await _unitRepository.GetAllAssignedRootsAsync(userId);
            List<UnitDTO> unitDTOs = new List<UnitDTO>();
            foreach(var root in roots)
            {
                unitDTOs.Add(await _unitRepository.GetDTOWithChildrenAsync(root));
            }
            return unitDTOs;
        }

        public async Task<IEnumerable<AssignmentDTO>> GetAssignmentsDTObyUserIdAsync(int userId)
        {
            var assignments = await _assignmentRepository.GetUserAssingmentsDTObyIdAsync(userId);
            return assignments;
        }

        public async Task<AssignmentDTO> AssignUserAsync(int creatorId, UserAssignment newAssignment)
        {
            //check that the user requesting is an admin of the unit or any of the units parents
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(creatorId, newAssignment.UnitId))
            {
                throw new UnauthorisedOperationException($"User : {newAssignment.UserId} is not authorised to assign users for unit: {newAssignment.UnitId}");
            }

            //it is unneccessary to assign a user as admin to a unit that are already an admin of or of its parent
            if (newAssignment.RoleId == 1)
            {
                if (await _unitRepository.CheckUserIsAdminInParentOfUnit(newAssignment.UserId, newAssignment.UnitId))
                {
                    throw new UnauthorisedOperationException($"User : {newAssignment.UserId} is already admin for unit: {newAssignment.UnitId} or one of its units parents");
                }
            }

            var assignment = await _assignmentRepository.CreateAssignment(newAssignment);

            return assignment;
        }



        public async Task<AssignmentDTO> AssignUserPublicOnCreate(int userId, int unitId)
        {
            return await _assignmentRepository.CreateAssignment(new UserAssignment { UnitId = unitId, RoleId = 3, UserId = userId });
        }

        public async Task<AssignmentDTO> UpdateAssignment(int updatorId, UserAssignment assignment)
        {
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(updatorId, assignment.UnitId))
            {
                throw new UnauthorisedOperationException($"User : {assignment.UserId} is not authorised to assign users for unit: {assignment.UnitId}");
            }
            
           if(!await _assignmentRepository.CheckUnitHasAdminAfterAssignmentUpdate(assignment))
            {
                throw new UnauthorisedOperationException($"Unit : {assignment.UnitId} will have no admin if user : {assignment.UserId} is reassigned");

            }

            var updatedAssignment = await _assignmentRepository.Update(assignment);
            if(updatedAssignment == null) {
                {
                    throw new DataInsertionException("Updated Assignment returned null");
                } }

            return updatedAssignment;
        }

        public async Task DeleteAssignmentAsync(int deletorId, UserAssignment assignment)
        {
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(deletorId, assignment.UnitId))
            {
                throw new UnauthorisedOperationException($"User : {assignment.UserId} is not authorised to delete assignments for unit: {assignment.UnitId}");
            }

            if(! await _assignmentRepository.CheckUnitHasAdminAfterAdminDelete(assignment))
            {
                throw new UnauthorisedOperationException($"Unit : {assignment.UnitId} will have no admin if user : {assignment.UserId} is deleted");
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
            var units = await _unitRepository.GetAllRelevantUnitsToUserAsync(user.Id, false);

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

        public async Task<IEnumerable<RoleDTO>> GetAllRoles()
        {
            return await _userRepository.GetRoles();
        }

        public async Task<bool> CheckUserIsPrivateOrAdminIncParent(int userId, int unitId)
        {
                return await _unitRepository.CheckUserHasRoleInParentOfUnit(userId, new int[] {1,2} , unitId);
        }

    }
    public interface IUserService
    {
        Task<AssignmentDTO> AssignUserAsync(int creatorId, UserAssignment assignment);
        Task<AssignmentDTO> AssignUserPublicOnCreate(int userId, int unitId);
        Task DeleteAssignmentAsync(int deletorId, UserAssignment assignment);
        Task<IEnumerable<UserDetailsDTO>> GetAllPublicUsers(int userId);
        Task<IEnumerable<AssignmentDTO>> GetAssignmentsDTObyUserIdAsync(int id);
        Task<User> GetCurrentUserAsync(HttpContext context);
        Task<IEnumerable<UnitDTO>> GetRelevantUnits(int userId);
        Task<UserDetailsDTO> GetUserDetailsAsync(User user);
        Task<AssignmentDTO> UpdateAssignment(int updatorId, UserAssignment assignment);
        public Task<IEnumerable<RoleDTO>> GetAllRoles();
        Task<bool> CheckUserIsPrivateOrAdminIncParent(int userId, int unitId);
    }



}
