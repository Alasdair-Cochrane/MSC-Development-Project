using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class EquipmentModel : IEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Model_Number { get; set; }
        [MaxLength(50)]
        public required string Model_Name { get; set;}
        [MaxLength(255)]
        public string? Description { get; set; }
        [MaxLength(50)]
        public required string Manufacturer { get; set;}
        public int? Weight { get; set;}
        public int? Height { get; set;}
        public int? Length { get; set;}
        public int? Depth { get; set;}
        public int? ListPrice { get; set;}        
        public int CategoryId { get; set;}

        [ForeignKey(nameof(CategoryId))]
        public EquipmentModelCategory Category { get; set; } = null!;
        public ICollection<EquipmentModelDocument> Documents { get; set; } = null!;
    }
 


}
