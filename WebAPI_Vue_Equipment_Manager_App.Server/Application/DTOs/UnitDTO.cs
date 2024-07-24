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
        public required bool IsPublic { get; set; }

        public IEnumerable<AssignmentDTO>? AssigedUsers { get; set; }
        public IEnumerable<UnitDTO>? Children { get; set; }

    }
}

