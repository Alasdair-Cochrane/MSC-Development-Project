using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities.Abstract_Classes;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories.Interfaces;

namespace WebAPI_Vue_Equipment_Manager_App.Server.DTOs.Mappings
{
    public static class ModelMappingExtensions
    {
        public static EquipmentModelDTO ToDTO(this EquipmentModel entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if(entity.Category == null)
            {
                throw new ArgumentNullException(nameof(entity.Category));
            }
                
            return new EquipmentModelDTO() {
                Id = entity.Id,
                Model_Number = entity.Model_Number,
                Model_Name = entity.Model_Name,
                Description = entity.Description,
                Manufacturer = entity.Manufacturer,
                Weight = entity.Weight,
                Height = entity.Height,
                Length = entity.Length,
                Depth = entity.Depth,
                Category = entity.Category.Name      
            };
        }

        public static EquipmentModelWithPriceDTO ToDTOWithPrice(this EquipmentModel entity)
        {
            return new EquipmentModelWithPriceDTO
            {
                Id = entity.Id,
                Model_Number = entity.Model_Number,
                Model_Name = entity.Model_Name,
                Description = entity.Description,
                Manufacturer = entity.Manufacturer,
                Weight = entity.Weight,
                Height = entity.Height,
                Length = entity.Length,
                Depth = entity.Depth,
                Category = entity.Category.Name,
                ListPrice = (int)entity.ListPrice      
            };

        }

        public static EquipmentModelWithDocumentsDTO ToDTOWithDocuments(this EquipmentModel entity)
        {
            return new EquipmentModelWithDocumentsDTO
            {
                Id= entity.Id,
                Model_Number = entity.Model_Number,
                Model_Name = entity.Model_Name,
                Description = entity.Description,
                Manufacturer = entity.Manufacturer,
                Weight = entity.Weight,
                Height = entity.Height,
                Length = entity.Length,
                Depth = entity.Depth,
                Category = entity.Category.Name,
                Documents = entity.Documents
            };
        }

        public static EquipmentModel ToEntity(this EquipmentModelDTO dto, int categoryId, int price = 0)
        {
            return new EquipmentModel {
                Id = dto.Id,
                Model_Number = dto.Model_Number,
                Model_Name = dto.Model_Name,
                Description = dto.Description,
                Manufacturer = dto.Manufacturer,
                Weight = dto.Weight,
                Height = dto.Height,
                Length = dto.Length,
                Depth = dto.Depth,
                ListPrice = price,
                CategoryId = categoryId,
            };

        }
    }
}
