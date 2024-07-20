using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDbContext _context;
        private readonly IUnitRepository _unitRepository;

        public UserRepository(MainDbContext context, IUnitRepository unitRepository)
        {
            _context = context;
            _unitRepository = unitRepository;
        }



        public async Task<IEnumerable<AssignmentDTO>> GetUserAssingmentsDTObyIdAsync(int userId)
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


        public async Task<IEnumerable<AssignmentDTO>> GetUserAssingmentsDTObyEmailAsync(string email)
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

        public async Task<IEnumerable<UserAssignment>> GetUserAssignmentsbyEmailAsync(string email)
        {
            var userId = await _context.Users.Where(x => x.Email == email).
                Select(x => x.Id).
                FirstOrDefaultAsync();

            var assignments = await _context.Assignments.Where(
                x => x.UserId == userId).ToListAsync();
            return assignments;
        }

        public async Task<UserAssignment> CreateAssignment(int unitId, int roleId, int userId)
        {
            var assignment = _context.Assignments.Add(new UserAssignment
            {
                UserId = userId,
                RoleId = roleId,
                UnitId = unitId,
            }).Entity;
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<UserAssignment> Update(int unitId, int roleId, int userId)
        {
            var assignment = _context.Assignments.Update(new UserAssignment
            {
                UserId = userId,
                RoleId = roleId,
                UnitId = unitId,
            }).Entity;
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task Delete(int unitId, int roleId, int userId)
        {
            _context.Assignments.Remove(new UserAssignment
            {
                UserId = userId,
                RoleId = roleId,
                UnitId = unitId,
            });
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUserIdByEmail(string email)
        {
            int id = await _context.Users.Where(x => x.Email == email).Select(x => x.Id).FirstOrDefaultAsync();
            return id;
        }




    }
}
