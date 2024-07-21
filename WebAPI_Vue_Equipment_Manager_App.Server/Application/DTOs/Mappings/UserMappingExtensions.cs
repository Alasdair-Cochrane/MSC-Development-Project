using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings
{
    public static class UserMappingExtensions
    {
        public static UserAssignment ToEntity(this AssignmentDTO dto)
        {
            UserAssignment userAssignment = new UserAssignment
            {
                RoleId = dto.RoleId,
                UnitId = dto.UnitId,
                UserId = dto.UserId,
            };
            return userAssignment;
        }
        public static AssignmentDTO ToDto(this UserAssignment entity) {
            var dto = new AssignmentDTO
            {
                RoleId = entity.RoleId,
                UnitId = entity.UnitId,
                UserId = entity.UserId,
            };
            if(entity.Role != null)
            {
                dto.RoleName = entity.Role.Name;
            }
            if(entity.Unit != null)
            {
                dto.UnitName = entity.Unit.Name;
            }
            if(entity.User != null)
            {
                dto.FirstName = entity.User.FirstName;
                dto.LastName = entity.User.LastName;
                dto.Email = entity.User.Email;
            }
            return dto;  
        }


        public static UserDetailsDTO ToDto(this User entity) {

            UserDetailsDTO dto = new UserDetailsDTO
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email ?? "",
            };
        
            if(entity.Assignments != null)
            {
                dto.Assignments = entity.Assignments.Select(x => x.ToDto()).ToList();
            }
            return dto;
        }
    }
}
