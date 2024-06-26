namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class AssignmentSimpleDTO
    {
        public required string UnitName { get; set; }
        public required string RoleName { get; set; }

    }
    public class AssignmentDTO : AssignmentSimpleDTO 
    { 
        public required int UserId { get; set; }
        public required int RoleId { get; set; }
        public required int UnitId { get; set; }
    
    
    }

}
