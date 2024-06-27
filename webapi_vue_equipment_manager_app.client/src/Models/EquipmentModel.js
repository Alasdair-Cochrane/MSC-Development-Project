
export class EquipmentModel {
    constructor(id, ModelNumber, ModelName, Description, Manufacturer, Weight, Height, Length, Depth, Category) {
        this.id = id;
        this.model_Number = ModelNumber;
        this.model_Name = ModelName;
        this.manufacturer = Manufacturer;
        this.weight = Weight;
        this.height = Height;
        this.length = Length;
        this.depth = Depth;
        this.category = Category;

    }
}