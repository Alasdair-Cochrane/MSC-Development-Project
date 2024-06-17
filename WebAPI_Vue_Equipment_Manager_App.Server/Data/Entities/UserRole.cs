using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class UserRole : IEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
    }
}
