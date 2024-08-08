using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class ItemSimpleDTO
    {
        public int? Id { get; set; }
        public required string SerialNumber { get; set; }
        public int? ModelId { get; set; }
        public int? UnitId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string LocalName { get; set; } = string.Empty;
        public DateTime? Date_of_reciept { get; set; }
        public DateTime? Date_of_commissioning { get; set; }
        public string? Condition_on_reciept { get; set; }
        public int? Current_Status_ID { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class ItemDTO : ItemSimpleDTO
    {
        [JsonPropertyOrder(1)]
        public required EquipmentModelDTO Model { get; set; }

        [JsonPropertyOrder(2)]
        public required string CurrentStatus { get; set; }
        public string? UnitName { get; set; }

        [JsonPropertyOrder(3)]

        public  UnitDTO? Unit { get; set; }

        [JsonPropertyOrder(4)]
        public ICollection<ItemDocument>? Documents { get; set; }

        public ICollection<MaintenanceDTO>? Maintenances { get; set; }

        public ICollection<ItemNote>? Notes { get; set; }
    }

    public class StatusQuantity
    {
        public required int StatusId { get; set; }
        public required string StatusName { get; set; }
        public required int ItemQuantity { get; set; }
        public int Order {  get; set; }
    }

}
