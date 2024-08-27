using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    [PrimaryKey(nameof(ModelId), nameof(UnitId), nameof(CategoryId))]
    public class MaintenanceFrequency
    {
        [Key]
        public int UnitId { get; set; }
        [Key]
        public int ModelId { get; set; }
        [Key]
        public int CategoryId { get; set; }
       
        public required int Frquency { get; set; }
        
        //Navigation Properties

        public required Unit Unit { get; set; }
        public required EquipmentModel Model { get; set; }
        public required MaintenanceCategory Category { get; set; }
    }
}
