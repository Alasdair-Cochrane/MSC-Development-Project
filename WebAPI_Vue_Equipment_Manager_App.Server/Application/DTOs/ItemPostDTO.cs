using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Web.Http.Controllers;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class ItemPostDTO
    {
        public required string SerialNumber { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string LocalName { get; set; } = string.Empty;
        public DateTime? Date_of_reciept { get; set; }
        public DateTime? Date_of_commissioning { get; set; }
        public string? Condition_on_reciept { get; set; }
        public int? Current_Status_ID { get; set; }
        public required string CurrentStatus { get; set; }

        public required int UnitId { get; set; }

        public required string ModelNumber { get; set; }
        public required string ModelName { get; set; }
        public required string Manufacturer { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public required string Category { get; set; }

        public string? ImageURL { get; set; }
    }
}

