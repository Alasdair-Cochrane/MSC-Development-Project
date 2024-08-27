namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class AssignmentDTO 
    { 
        public required int UserId { get; set; }
        public required int RoleId { get; set; }
        public required int UnitId { get; set; }
        public string? UnitName { get; set; }
        public string? RoleName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public string? Email { get; set; }

    }

}
