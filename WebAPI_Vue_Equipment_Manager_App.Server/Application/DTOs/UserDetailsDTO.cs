namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class UserDetailsDTO
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public  List<AssignmentDTO>? Assignments { get; set; }
        public List<UnitDTO>? Units { get; set; }
    
    }

}
