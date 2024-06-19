using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class Item : IEntity
    {
        public int Id { get; set; }
        [Required]
        public required string SerialNumber { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        public int UnitId { get; set; }
        public string? Barcode { get; set; }
        [MaxLength(50)]
        public string? LocalName { get; set; }
        public DateOnly? Date_Of_Reciept { get; set; }
        public DateOnly? Date_Of_Acceptance_Test { get; set; }
        public DateOnly? Date_Of_Activation { get; set; }
        public bool? New_On_Reciept { get; set; }
        public int ItemStatusCategoryId { get; set; }
        [MaxLength(100)]
        public string? Image { get; set; }
        public decimal? Purchase_Price { get; set; }
        public int? Purchase_Order {  get; set; }

        //Navigation Properties

        public EquipmentModel EquipmentModel { get; set; } = null!;
        public  Unit Unit { get; set; } = null!;
        public ICollection<ItemDocument>? Documents { get;}
        public ICollection<ItemNote>? Notes { get;}
        public ItemStatusCategory StatusCategory { get; set; } = null!;

    }
    
}
