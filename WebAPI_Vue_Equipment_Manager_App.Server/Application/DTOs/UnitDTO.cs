using System.ComponentModel.DataAnnotations;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class UnitDTO
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public string? Building { get; set; }
        public string? Room { get; set; }
        public string? Address { get; set; }
        public int? ParentId { get; set; }

        public List<AssignmentDTO>? AssigedUsers { get; set; }
    }
}

