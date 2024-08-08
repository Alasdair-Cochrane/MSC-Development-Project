namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class ItemExportDTO
    {
        public int? Id { get; set; }
        public string? SerialNumber { get; set; }
        public string? Barcode { get; set; } 
        public string? LocalName { get; set; } 
        public string? ModelName { get; set; }
        public string? ModelNumber { get; set; }
        public string? Manufacturer {  get; set; }
        public string? Category { get; set; }
        public string?   Status { get; set; }
        public int? UnitId { get; set; }
        public string? UnitName { get; set; }
        public string? Building {  get; set; }
        public string? Room { get; set; }
        public string? Address { get; set; }
        public DateTime? Date_of_reciept { get; set; }
        public DateTime? Date_of_commissioning { get; set; }
        public string? Condition_on_reciept { get; set; }
    }
}
