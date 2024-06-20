using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class MaintenanceDTO
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public required string Provider_Name { get; set; }
        public DateTime Date_Completed { get; set; }
        public required string CategoryName { get; set; }
    }
}
