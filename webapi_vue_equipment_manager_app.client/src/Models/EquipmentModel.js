
export class EquipmentModel {
    constructor(id, ModelNumber, ModelName, Description, Manufacturer, Weight, Height, Length, Width, Category) {
        this.id = id;
        this.modelNumber = ModelNumber;
        this.modelName = ModelName;
        this.manufacturer = Manufacturer;
        this.weight = Weight;
        this.height = Height;
        this.length = Length;
        this.width = Width;
        this.category = Category;
    }
}