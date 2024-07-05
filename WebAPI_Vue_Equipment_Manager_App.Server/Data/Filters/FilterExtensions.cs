using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Filters
{
    public static class ItemFilters
    {
        public static IQueryable<Item> FilterSerialNumber(this IQueryable<Item> query, string value)
        {
            return query.Where(x => x.SerialNumber.StartsWith(value));
        }
        public static IQueryable<Item> FilterLocalName(this IQueryable<Item> query, string value)
        {
            return query.Where(x => x.LocalName != null && x.LocalName.StartsWith(value));
        }
        public static IQueryable<Item> FilterBarcode(this IQueryable<Item> query, string value)
        {
            return query.Where(x => x.Barcode != null && x.Barcode == value);
        }

        public static IQueryable<Item> FilterUnit(this IQueryable<Item> query, int unitId)
        {
            return query.Where(x => x.UnitId == unitId);
        }
        public static IQueryable<Item> FilterModelId(this IQueryable<Item> query, IEnumerable<int> modelIds)
        {
            return query.Where(x => modelIds.Contains(x.Id));
        }
    }
    public static class ModelFilters
    {
        public static IQueryable<EquipmentModel> FilterModelName(this IQueryable<EquipmentModel> query, string value)
        {
            return query.Where(x => x.ModelName.StartsWith(value));
        }
        public static IQueryable<EquipmentModel> FilterModelNumber(this IQueryable<EquipmentModel> query, string value)
        {
            return query.Where(x => x.ModelNumber.StartsWith(value));
        }
        public static IQueryable<EquipmentModel> FilterManufacturer(this IQueryable<EquipmentModel> query, string value)
        {
            return query.Where(x => x.Manufacturer.StartsWith(value));
        }
        public static IQueryable<EquipmentModel> FilterCategoryId(this IQueryable<EquipmentModel> query, IEnumerable<int> categoryId)
        {
            return query.Where(x => categoryId.Contains(x.CategoryId));
        }
        public static IQueryable<EquipmentModel> FilterCategoryName(this IQueryable<EquipmentModel> query, string value)
        {
            return query.Where(x => x.Category.Name.StartsWith(value));
        }


    }

}
