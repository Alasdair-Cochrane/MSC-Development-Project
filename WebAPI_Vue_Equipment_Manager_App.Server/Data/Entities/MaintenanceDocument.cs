using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    [PrimaryKey(nameof(MaintenanceId), nameof(ItemDocumentId))]

    public class MaintenanceDocument
    {
        [Key]
        public int MaintenanceId {  get; set; }
        [Key]
        public int ItemDocumentId { get; set; }

        public required Maintenance Maintenance { get; set; }
        public required ItemDocument ItemDocument { get; set; }
    }
}
