using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly MainDbContext _context;

        public AssignmentRepository(MainDbContext mainDbContext)
        {
            _context = mainDbContext;
        }

        public async Task<IEnumerable<AssignmentDTO>> GetUserAssignmentsForUnitAsync(int unitId)
        {
            var assignments = await _context.Assignments.
                Where(x => x.UnitId == unitId).
                AsSplitQuery().
                AsNoTracking().
                Include(x => x.User).
                Include(x => x.Unit).
                Include(x => x.Role).
                Select(x => new AssignmentDTO
                {
                    RoleId = x.RoleId,
                    UnitId = x.UnitId,
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Email = x.User.Email,
                    UnitName = x.Unit.Name,
                    RoleName = x.Role.Name,
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

        public async Task<AssignmentDTO?> CreateAssignment(UserAssignment assignment)
        {
            var result = _context.Assignments.Add(assignment).Entity;
            await _context.SaveChangesAsync();
            var dto = await  _context.Assignments.Where(x => x.UnitId == assignment.UnitId && x.UserId == assignment.UserId && x.RoleId == assignment.RoleId).
                Select(x=> new AssignmentDTO
                {
                    RoleId = x.RoleId,
                    UnitId = x.UnitId,
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    RoleName= x.Role.Name,
                    UnitName = x.Unit.Name,
                }).FirstOrDefaultAsync();
            return dto;
        }

        public async Task<AssignmentDTO?> Update(UserAssignment assignement)
        {
            var assignment = _context.Assignments.Update(assignement).Entity;
            await _context.SaveChangesAsync();
            var dto = await _context.Assignments.Where(x => x.UnitId == assignment.UnitId && x.UserId == assignment.UserId && x.RoleId == assignment.RoleId).
                Select(x => new AssignmentDTO
                {
                    RoleId = x.RoleId,
                    UnitId = x.UnitId,
                    UserId = x.UserId,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    RoleName = x.Role.Name,
                    UnitName = x.Unit.Name,
                }).FirstOrDefaultAsync();
            return dto;
        }

        public async Task Delete(UserAssignment assignment)
        {
            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
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


        public async Task<IEnumerable<int>> GetAssignedRootUnitIds(int userId)
        {
            var unitIDs = await _context.Assignments.
                Where(x => x.UserId == userId).
                Where(x => x.Unit.ParentId == null).
                Select(x => x.UnitId).ToListAsync();
            return unitIDs;
        }




    }
}
