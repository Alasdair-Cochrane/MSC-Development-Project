using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public required string FirstName { get; set; } 

        [Required]
        [MaxLength(20)]
        public required string LastName { get; set; } 

        [EmailAddress]
        [MaxLength(50)]
        public required string Email { get; set; } 

        public ICollection<UserAssignment>? Assignments { get; set; }
    }
}
