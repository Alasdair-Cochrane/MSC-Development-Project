
export class Item {
    constructor(ID, SerialNumber, localName, ModelID, UnitID, Barcode, LocalName, DOR, DOAT, DOA, NOR, statusID, image, model, status, unit) {
        this.Id = ID;
        this.SerialNumber = SerialNumber;
        this.LocalName = localName,
        this.Barcode = Barcode;
        this.ModelId = ModelID;
        this.UnitID = UnitID;  
        this.Date_Of_Reciept = DOR;
        this.Date_Of_Commissioning = DOAT;
        this.New_On_Reciept = NOR;
        this.Current_Status_ID = statusID;
        this.ImageUrl = image;
        this.Model = model;
        this.CurrentStatus = status;
        this.UnitName = unit;
    }


}