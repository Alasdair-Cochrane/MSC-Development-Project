﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities
{
    public class Item : IEntity
    {
        public int Id { get; set; }
        [Required]
        public required string SerialNumber { get; set; }
        [Required]
        [ForeignKey(nameof(EquipmentModel))]
        public int ModelId { get; set; }
        [Required]
        public int UnitId { get; set; }
        public string? Barcode { get; set; }
        [MaxLength(50)]
        public string? LocalName { get; set; }
        public DateTime? Date_Of_Reciept { get; set; }
        public DateTime? Date_Of_Commissioning { get; set; }
        public string? Condition_On_Reciept { get; set; }
        public int ItemStatusCategoryId { get; set; }
        [MaxLength(200)]
        public string? Image { get; set; }
        public decimal? Purchase_Price { get; set; }
        public int? Purchase_Order {  get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        //Navigation Properties
        public EquipmentModel EquipmentModel { get; set; } = null!;
        public  Unit Unit { get; set; } = null!;
        public ICollection<ItemNote>? Notes { get;}
        public ItemStatusCategory StatusCategory { get; set; } = null!;

        public ICollection<ItemDocument> Documents { get; set; } = null!;
        public ICollection<Maintenance> Maintenances { get; set; } = null!;

    }
    
}
