using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class EquipmentModelDTO
    {
        public int Id { get; set; }
        public required string Model_Number { get; set; }
        public required string Model_Name { get; set; }
        public required string Manufacturer { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public int? Length { get; set; }
        public int? Depth { get; set; }
        public required string Category { get; set; }
    }



}
