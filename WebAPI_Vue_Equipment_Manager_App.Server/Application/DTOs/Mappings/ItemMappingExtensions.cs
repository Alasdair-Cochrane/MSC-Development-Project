using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings
{
    public static class ItemMappingExtensions
    {
        public static ItemDTO ToDTO(this Item item)
        {
            if(item.EquipmentModel == null)
            {
                throw new ArgumentNullException("item Model");
            }
            if(item.StatusCategory == null)
            {
                throw new ArgumentNullException("item status");
            }

            ItemDTO dto = new ItemDTO
            {
                Id = item.Id,
                SerialNumber = item.SerialNumber,
                Model = item.EquipmentModel.ToDTO(),
                UnitId = item.Unit.Id,
                UnitName = item.Unit.Name,
                LocalName = item.LocalName,
                Barcode = item.Barcode,

                Date_Of_Reciept = item.Date_Of_Reciept,
                Date_Of_Acceptance_Test = item.Date_Of_Acceptance_Test,
                Date_Of_Activation = item.Date_Of_Activation,
                New_On_Reciept = item.New_On_Reciept,
                Current_Status = item.StatusCategory.Name,
                ImageUrl = item.Image,
            };
            return dto;
        }

        public static ItemWithPurchaseDTO ToDTOWithPurchase(this Item item)
        {
            ItemWithPurchaseDTO dto = new ItemWithPurchaseDTO
            {
                Id = item.Id,
                SerialNumber = item.SerialNumber,
                Model = item.EquipmentModel.ToDTO(),
                UnitId = item.Unit.Id,
                UnitName = item.Unit.Name,
                LocalName = item.LocalName,
                Barcode = item.Barcode,

                Date_Of_Reciept = item.Date_Of_Reciept,
                Date_Of_Acceptance_Test = item.Date_Of_Acceptance_Test,
                Date_Of_Activation = item.Date_Of_Activation,
                New_On_Reciept = item.New_On_Reciept,
                Current_Status = item.StatusCategory.Name,
                ImageUrl = item.Image,

                Purchase_Price = item.Purchase_Price,
                PurchaseOrder = item.Purchase_Order,

            };
            return dto;
        }

        public static Item ToEntity(this ItemDTO item,int categoryID, decimal price = 0, int PurchaseOrder = 0)
        {
            Item entity = new Item
            {
                Id = item.Id,
                SerialNumber = item.SerialNumber,
                ModelId = item.Model.Id,
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
