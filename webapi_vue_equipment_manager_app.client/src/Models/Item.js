
export class Item {
    constructor(ID, SerialNumber, localName, ModelID, UnitID, Barcode, LocalName, DOR, DOAT, DOA, Condition, statusID, image, model, status, unit) {
        this.Id = ID;
        this.SerialNumber = SerialNumber;
        this.LocalName = localName,
        this.Barcode = Barcode;
        this.ModelId = ModelID;
        this.UnitID = UnitID;  
        this.Date_Of_Reciept = DOR;
        this.Date_Of_Commissioning = DOAT;
        this.Condition = Condition;
        this.Current_Status_ID = statusID;
        this.ImageUrl = image;
        this.Model = model;
        this.CurrentStatus = status;
        this.UnitName = unit;
    }


}