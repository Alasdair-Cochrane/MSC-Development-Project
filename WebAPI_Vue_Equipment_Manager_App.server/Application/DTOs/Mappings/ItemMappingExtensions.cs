using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings
{
    public static class ItemMappingExtensions
    {
        public static ItemDTO ToDTO(this Item item)
        {
            if (item.EquipmentModel == null)
            {
                throw new ArgumentNullException("item Model");
            }
            ItemDTO dto = new ItemDTO
            {
                Id = item.Id,
                SerialNumber = item.SerialNumber,
                ModelId = item.ModelId,
                UnitId = item.UnitId,
                LocalName = item.LocalName ?? "",
                Barcode = item.Barcode ?? "",

                Date_of_reciept = item.Date_Of_Reciept,
                Date_of_commissioning = item.Date_Of_Commissioning,
                Condition_on_reciept = item.Condition_On_Reciept,
                Current_Status_ID = item.ItemStatusCategoryId,
                ImageUrl = item.Image ?? "",

                Model = item.EquipmentModel.ToDTO(),
                Unit = item.Unit.ToDTO() ?? null,
                UnitName = item.Unit.Name,
                CurrentStatus = item.StatusCategory.Name,
                StatusColorHex = item.StatusCategory.ColorHex ?? "",
                Documents = item.Documents ?? [],
                Maintenances = item.Maintenances.ToDTO() ?? [],
                Notes = item.Notes ?? [],
                DateCreated = item.DateCreated,

            };
            return dto;
        }


        public static ItemSimpleDTO ToSimpleDTO(this Item item)
        {
            var dto = new ItemSimpleDTO { 
                Id = item.Id,
                SerialNumber = item.SerialNumber,
                ModelId = item.ModelId,
                UnitId = item.UnitId,
                LocalName = item.LocalName ?? "",
                Barcode = item.Barcode ?? "",

               Date_of_reciept = item.Date_Of_Reciept,
               Date_of_commissioning = item.Date_Of_Commissioning,

                Condition_on_reciept = item.Condition_On_Reciept,
                Current_Status_ID = item.ItemStatusCategoryId,
                ImageUrl = item.Image ?? "",
            };
            return dto;

        }

        public static Item ToEntity(this ItemDTO item,int categoryID, decimal price = 0, int PurchaseOrder = 0)
        {
            int modelID;
            if (item.Model == null || item.Model.Id == 0)
            {
                modelID = item.ModelId ?? 0;
            }
            else
            {
                modelID = item.Model.Id ?? 0;
            }

            Item entity = new Item
            {
                Id = item.Id ?? 0,
                SerialNumber = item.SerialNumber,
                ModelId = modelID,
                UnitId = item.UnitId ?? 0,
                LocalName = item.LocalName,
                Barcode = item.Barcode,
                Date_Of_Reciept = item.Date_of_reciept,
                Date_Of_Commissioning = item.Date_of_commissioning,
                Condition_On_Reciept = item.Condition_on_reciept,
                ItemStatusCategoryId = categoryID,
                Image = item.ImageUrl,

                Purchase_Price = price * 100,
                Purchase_Order = PurchaseOrder,
            };
            return entity;
        }

        public static ItemDTO ToItemDTOFromPost(this ItemPostDTO post)
        {
            EquipmentModelDTO model = new EquipmentModelDTO {
                ModelName = post.ModelName,
                ModelNumber = post.ModelNumber,
                Category = post.Category,
                Manufacturer = post.Manufacturer,
                Weight = post.Weight,
                Length = post.Length,
                Height = post.Height,
                Width   = post.Width,
            };

            ItemDTO item = new ItemDTO {

                SerialNumber = post.SerialNumber,
                LocalName = post.LocalName ?? "",
                Barcode = post.Barcode ?? "",
                Date_of_reciept = post.Date_of_reciept,
                Date_of_commissioning = post.Date_of_commissioning,
                Condition_on_reciept = post.Condition_on_reciept,
                Current_Status_ID = post.Current_Status_ID,
                Model = model,
                UnitId = post.UnitId,
                CurrentStatus = post.CurrentStatus,
                ImageUrl = post.ImageURL,
                
            };
            return item;


        }
        public static ItemExportDTO ToExportDTO(this Item item)
        {

            ItemExportDTO dto = new ItemExportDTO
            {
                Id = item.Id,
                SerialNumber = item.SerialNumber,
                LocalName = item.LocalName ?? "",
                Barcode = item.Barcode ?? "",
                Status = item.StatusCategory.Name,

                ModelName = item.EquipmentModel.ModelName,
                ModelNumber = item.EquipmentModel.ModelNumber,
                Manufacturer = item.EquipmentModel.Manufacturer,
                Category = item.EquipmentModel.Category.Name,

                UnitId = item.UnitId,
                UnitName = item.Unit.Name,
                Building = item.Unit.Building,
                Room = item.Unit.Room,
                Address = item.Unit.Address,

                Date_of_reciept = item.Date_Of_Reciept,
                Date_of_commissioning = item.Date_Of_Commissioning,
                Condition_on_reciept = item.Condition_On_Reciept,
            };
            return dto;
        }
    }
}
