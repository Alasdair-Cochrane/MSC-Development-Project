import { EquipmentModel } from "./EquipmentModel";

export class Item {
    constructor(model, iD, serialNumber, localName, barcode, DOR, DOC, Condition, image, status, unit, purchaseOrder, unitID) {
        this.id = iD;
        this.serialNumber = serialNumber;
        this.localName = localName,
        this.barcode = barcode; 
        this.date_of_reciept = DOR;
        this.date_of_commissioning = DOC;
        this.condition_on_reciept = Condition;
        this.imageUrl = image;
        this.model = model;
        this.currentStatus = status;
        this.unitName = unit;
        this.purchaseOrder = purchaseOrder;
        this.unitID = unitID
    }
}

export function getSearchFields(){
    return {"Serial Number" : "SerialNumber" , 
         "Local Name" :"LocalName" , 
         "Barcode" : "Barcode", 
         "Unit Name" : "UnitName",
         "Model Name" : "ModelName", 
         "Model Number" : "ModelNumber",
         "Manufacturer" : "Manufacturer",
         "Category" : "Category" }
}

