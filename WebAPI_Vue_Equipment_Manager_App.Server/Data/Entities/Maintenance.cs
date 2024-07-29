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
        public string Description { get; set; } = string.Empty;
        public required DateTime Date_Completed { get; set; }
        public required int MaintenanceCategoryId { get; set; }
        public MaintenanceCategory Category { get; set; } = null!;

    }
    

}
