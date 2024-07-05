using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings
{
    public static class ModelMappingExtensions
    {
        public static EquipmentModelDTO ToDTO(this EquipmentModel entity)
        {
            
            if (entity.Category == null)
            {
                throw new ArgumentNullException(nameof(entity.Category));
            }

            return new EquipmentModelDTO()
            {
                Id = entity.Id,
                ModelNumber = entity.ModelNumber,
                ModelName = entity.ModelName,
                Manufacturer = entity.Manufacturer,
                Weight = entity.Weight,
                Height = entity.Height,
                Length = entity.Length,
                Width = entity.Width,
                Category = entity.Category.Name
            };
        }

        

        public static EquipmentModel ToEntity(this EquipmentModelDTO dto, int categoryId, int price = 0)
        {
            return new EquipmentModel
            {
                Id = dto.Id,
                ModelNumber = dto.ModelNumber,
                ModelName = dto.ModelName,
                Manufacturer = dto.Manufacturer,
                Weight = dto.Weight,
                Height = dto.Height,
                Length = dto.Length,
                Width = dto.Width,
                ListPrice = price,
                CategoryId = categoryId,
            };

        }
    }
}
