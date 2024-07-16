namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class AssignmentDTO 
    { 
        public required int UserId { get; set; }
        public required int RoleId { get; set; }
        public required int UnitId { get; set; }
        public required string UnitName { get; set; }
        public required string RoleName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set;}
        public required string Email { get; set; }

    }

}
