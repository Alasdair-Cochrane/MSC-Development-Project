namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Queries
{
    public class ItemQuery
    {
        public string? SerialNumber { get; set; }
        public string? LocalName { get; set; }
        public string? Barcode { get; set; }
        public string? ModelName { get; set; }
        public string? ModelNumber { get; set; }
        public string? Manufacturer { get; set; }
        public string? Category { get; set; }
        public int? StatusCategoryId { get; set; }
        public string? StatusCategoryName { get; set; }
        public string? UnitName { get; set; }
        public int? UnitId { get; set; }

    }
}
