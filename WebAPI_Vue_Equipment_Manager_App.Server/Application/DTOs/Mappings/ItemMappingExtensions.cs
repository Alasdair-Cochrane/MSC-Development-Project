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

                Date_Of_Reciept = item.Date_Of_Reciept,
                Date_Of_Acceptance_Test = item.Date_Of_Acceptance_Test,
                Date_Of_Activation = item.Date_Of_Activation,
                New_On_Reciept = item.New_On_Reciept,
                Current_Status_ID = item.ItemStatusCategoryId,
                ImageUrl = item.Image ?? "",

                Model = item.EquipmentModel.ToDTO(),
                UnitName = item.Unit.Name,
                CurrentStatus = item.StatusCategory.Name
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

                Date_Of_Reciept = item.Date_Of_Reciept,
                Date_Of_Acceptance_Test = item.Date_Of_Acceptance_Test,
                Date_Of_Activation = item.Date_Of_Activation,
                New_On_Reciept = item.New_On_Reciept,
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
                modelID = item.ModelId;
            }
            else
            {
                modelID = item.Model.Id;
            }

            Item entity = new Item
            {
                Id = item.Id,
                SerialNumber = item.SerialNumber,
                ModelId = modelID,
                UnitId = item.UnitId,
                LocalName = item.LocalName,
                Barcode = item.Barcode,
                Date_Of_Reciept = item.Date_Of_Reciept,
                Date_Of_Acceptance_Test = item.Date_Of_Acceptance_Test,
                Date_Of_Activation = item.Date_Of_Activation,
                New_On_Reciept = item.New_On_Reciept,
                ItemStatusCategoryId = categoryID,
                Image = item.ImageUrl,

                Purchase_Price = price * 100,
                Purchase_Order = PurchaseOrder,
            };
            return entity;
        }
    }
}
