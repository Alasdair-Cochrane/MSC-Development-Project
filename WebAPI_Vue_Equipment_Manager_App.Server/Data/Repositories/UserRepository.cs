using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class UserRepository
    {
        private readonly MainDbContext _context;
        private readonly IUnitRepository _unitRepository;

        public UserRepository(MainDbContext context, IUnitRepository unitRepository)
        {
            _context = context;
            _unitRepository = unitRepository;
        }

        public async Task<bool> CheckUserHasRoleInUnit(int userId, int roleId, int unitId)
        {
            var assignment = await _context.Assignments.AllAsync(x =>
            x.UserId == userId && 
            x.UnitId == unitId && 
            x.RoleId == roleId);
            return assignment;
        }

        //checks whether a user is an admin of the unit or of any of the unit's parents
        public async Task<bool> CheckUserIsAdminInParentOfUnit(int userId, int unitId)
        {
            var adminRoleUnits = await GetUnitIdsWithUserRole(userId, 1);

            if(adminRoleUnits.IsNullOrEmpty()) { return false; }

            var children = await _unitRepository.GetParentIDsofUnit(unitId);

            return children.Any(x => adminRoleUnits.Contains(x));

        }

        
        //Returns those units where the specified user has been assigned the specified role
        public async Task<IEnumerable<int>> GetUnitIdsWithUserRole(int userId, int roleId)
        {
            var units = await _context.Assignments.
                Where(x => x.UserId == userId && x.RoleId == roleId).
                Select(x => x.UnitId).
                ToListAsync();
            return units;
        }

        public async Task<IEnumerable<AssignmentDTO>> GetUserAssingmentsbyId(int userId)
        {
            var assignments = await _context.Assignments.Where(
                x => x.UserId == userId).
                Select(x => new AssignmentDTO
                {
                    UserId = x.UserId,
                    RoleId = x.RoleId,
                    UnitId = x.UnitId,
                    UnitName = x.Unit.Name,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email ?? "",
                    RoleName = x.Role.Name ?? ""
                }).ToListAsync();
            return assignments;
        }


        public async Task<IEnumerable<AssignmentDTO>> GetUserAssingmentsbyEmail(string email)
        {
            var userId = await _context.Users.Where(x => x.Email == email).
                Select(x => x.Id).
                FirstOrDefaultAsync();

            var assignments = await _context.Assignments.Where(
                x => x.UserId == userId).
                Select(x => new AssignmentDTO
                {
                    UserId = x.UserId,
                    RoleId = x.RoleId,
                    UnitId = x.UnitId,
                    UnitName = x.Unit.Name,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email ?? "",
                    RoleName = x.Role.Name ?? ""
                }).ToListAsync();
            return assignments;
        }

        public async Task<IEnumerable<UserDetailsDTO>> GetUsersInUnit(int unitId)
        {
            var users = await _context.Assignments.Where(x => x.UnitId == unitId).
                Select(x => new UserDetailsDTO
                {
                    Id = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email ?? "",
                    Role = x.Role.Name,
                }).ToListAsync();

            return users;
        }



    }
}
