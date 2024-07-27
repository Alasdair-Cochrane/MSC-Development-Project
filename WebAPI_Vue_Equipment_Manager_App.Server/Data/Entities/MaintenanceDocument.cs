using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    [PrimaryKey(nameof(MaintenanceId), nameof(DocumentId))]

    public class MaintenanceDocument : IDocumentRelation
    {
        public int Id { get; set; }

        public int MaintenanceId {  get; set; }
        public int DocumentId { get; set; }

        public Maintenance Maintenance { get; set; } = null!;
        public Document Document { get; set; } = null!;
    }
}
