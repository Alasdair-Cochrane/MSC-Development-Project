using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class Maintenance : IEntity
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        [MaxLength(50)]
        public required string Provider_Name { get; set; }
        public required DateTime Date_Completed { get; set; }
        public required int CategoryId { get; set; }
        public MaintenanceCategory Category { get; set; } = null!;

    }
    

}
