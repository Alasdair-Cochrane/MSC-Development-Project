using System.ComponentModel.DataAnnotations;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class UserRegisterDTO
    {
        public required string  FirstName { get; set; }
        public required string  LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }

    }

    public class LoginDTO
    {
        public required string Email { get; set; }
        public required  string Password { get; set; }
    }
    
}
