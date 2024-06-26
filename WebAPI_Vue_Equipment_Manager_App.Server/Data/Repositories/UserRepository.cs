using Microsoft.EntityFrameworkCore;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class UserRepository
    {
        private readonly MainDbContext _context;

        public UserRepository(MainDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckUserHasRoleInUnit(int userId, int roleId, int unitId)
        {
            var assignment = await _context.Assignments.AnyAsync(x =>
            x.UserId == userId && 
            x.UnitId == unitId && 
            x.RoleId == roleId);
            return assignment;
        }

        public async Task<UserDTO?> GetUserDTOAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDTO>> GetUsersInUnit(int unitId)
        {
            throw new NotImplementedException();
        }



    }
}
