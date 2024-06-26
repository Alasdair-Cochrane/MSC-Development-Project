namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings
{
    public class UserDTO
    {
        public required string UserName { get; set; }
        public required List<AssignmentSimpleDTO> Assignments { get; set;}
    }
}
