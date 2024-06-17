using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class Unit : IEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(50)]
        public string? Building { get; set; }
        [MaxLength(50)]
        public string? Room { get; set; }
        [MaxLength(500)]
        public string? Address { get; set;}
        public int? ParentId { get; set; }
        public Unit? Parent { get; set; }

    }
}
