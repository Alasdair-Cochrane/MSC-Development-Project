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
        private readonly IUserRepository _userRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly UserManager<User> _userManager;
        private User? _currentUser;

        private IEnumerable<Unit>? _usersUnits;

        public UserService(IUserRepository userRepository, IUnitRepository unitRepository, UserManager<User> manager)
        {
            _userRepository = userRepository;
            _unitRepository = unitRepository;
            _userManager = manager;
        }



        //getting all units is a potentially expensive operation (involves multiple recursive operations) so it is best to do it once and cache the results per request
        public async Task<IEnumerable<Unit>> GetRelevantUnits(int userId)
        {
            if (_usersUnits.IsNullOrEmpty())
            {
                _usersUnits = await _unitRepository.GetAllRelevantUnitsToUser(userId);
                return _usersUnits;
            }
            else
            {
                return _usersUnits = null!;
            }
        }

        public async Task<IEnumerable<AssignmentDTO>> GetUserAssignmentsbyEmailAsync(string email)
        {
            var assignments = await _userRepository.GetUserAssingmentsDTObyEmailAsync(email);
            return assignments;
        }

        public async Task<UserAssignment> AssignUser(int creatorId, int userID, int roleId, int unitId)
        {
            //check that the user requesting is an admin of the unit or any of the units parents
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(creatorId, unitId))
            {
                throw new UnauthorisedOperationException($"User : {userID} is not authorised to assign users for unit: {unitId}");
            }

            //it is unneccessary to assign a user as admin to a unit that are already an admin of or of its parent
            if (roleId == 1)
            {
                if (await _unitRepository.CheckUserIsAdminInParentOfUnit(userID, unitId))
                {
                    throw new UnauthorisedOperationException($"User : {userID} is already admin for unit: {unitId} or one of its units parents");
                }
            }

            var assignment = await _userRepository.CreateAssignment(unitId, roleId, unitId);

            return assignment;
        }

        public async Task<UserAssignment> AssignUserPublicOnCreate(int userId, int unitId)
        {
            return await _userRepository.CreateAssignment(unitId, 3, userId);
        }

        public async Task<UserAssignment> UpdateAssignment(int updatorId, int userId, int roleId, int unitId)
        {
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(updatorId, unitId))
            {
                throw new UnauthorisedOperationException($"User : {userId} is not authorised to assign users for unit: {unitId}");
            }
            var assignment = await _userRepository.Update(userId, roleId, unitId);

            return assignment;
        }

        public async Task DeleteAssignment(int deletorId, int userId, int roleId, int unitId)
        {
            if (!await _unitRepository.CheckUserIsAdminInParentOfUnit(deletorId, unitId))
            {
                throw new UnauthorisedOperationException($"User : {userId} is not authorised to delete assignments for unit: {unitId}");
            }
            await _userRepository.Delete(userId, roleId, unitId);
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
            var assignments = await _userRepository.GetUserAssingmentsDTObyIdAsync(user.Id);
            var unitsDTO = new List<UnitDTO>();
            var units = await _unitRepository.GetAllRelevantUnitsToUser(user.Id);

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
        Task DeleteAssignment(int deletorId, int userId, int roleId, int unitId);
        Task<User> GetCurrentUserAsync(HttpContext context);
        Task<IEnumerable<Unit>> GetRelevantUnits(int userId);
        Task<IEnumerable<AssignmentDTO>> GetUserAssignmentsbyEmailAsync(string email);
        Task<UserDetailsDTO> GetUserDetailsAsync(User user);
        Task<UserAssignment> UpdateAssignment(int updatorId, int userId, int roleId, int unitId);
    }



}
