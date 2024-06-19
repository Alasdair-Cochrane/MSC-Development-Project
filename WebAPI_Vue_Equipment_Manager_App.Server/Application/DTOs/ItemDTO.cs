using System.ComponentModel.DataAnnotations;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public required string SerialNumber { get; set; }
        public required EquipmentModelDTO Model { get; set; }
        public int UnitId { get; set; }
        public required string UnitName { get; set; }    
        public string? Barcode { get; set; }
        public string? LocalName { get; set; }
        public DateOnly? Date_Of_Reciept { get; set; }
        public DateOnly? Date_Of_Acceptance_Test { get; set; }
        public DateOnly? Date_Of_Activation { get; set; }
        public bool? New_On_Reciept { get; set; }
        public required string Current_Status { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class ItemWithPurchaseDTO : ItemDTO
    {
        
        public decimal? Purchase_Price { get; set; }
        public int? PurchaseOrder {  get; set; }
    }
}
