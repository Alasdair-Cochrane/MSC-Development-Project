<script setup>
    import { ref, onMounted } from "vue"
    import { Item } from '@/Models/Item';
    import {EquipmentModel } from '@/Models/EquipmentModel'
    import { addItem} from '@/Services/ItemService'
    import { store, UpdateCategories, UpdateModels } from "@/Store/Store";
    import { IsMobile } from "@/Services/DeviceService";
    import {useToast} from 'primevue/usetoast'
    import InterpretImage from "@/components/InterpretImage.vue";
import CaptureImage from "./CaptureImage.vue";


const item = ref(new Item(new EquipmentModel()))
var mobile = ref(false);
const condOptions = ["New", "Used"]
const toast = useToast();
const emit = defineEmits(['itemSaved'])
const selectedImage = ref()
const imageDisplay = ref()
const showScanner = ref(false)
const showCamera = ref(false)
const selectedUnit = ref()

const showIsValid = ref(false)


onMounted(() => {
    mobile.value = IsMobile();
    item.value.condition_on_reciept = condOptions[0]
})

const  isNotEmpty = (modelField) =>{
    if(modelField === null || modelField === "" || modelField === undefined) {
        return false;
    }
    else{
        return true;
    }
}

const validateSubmission = () =>
{   
    showIsValid.value = true
    if(
        isNotEmpty(item.value.serialNumber) &&
        isNotEmpty(item.value.unitID) &&
        isNotEmpty(item.value.model.modelNumber) &&
        isNotEmpty(item.value.model.modelName) &&
        isNotEmpty(item.value.model.category) &&
        isNotEmpty(item.value.currentStatus)
    ) {
        showIsValid.value = false
        return true;
    }
    else {
        return false
    }
}


function clear(){
    item.value = new Item(new EquipmentModel())
    modelName.value = ""
    modelNumber.value = ""
    selectedUnit.value = null
}

async function save(){
    console.log(modelName.value)
    item.value.model.modelName = modelName?.value;
    item.value.model.modelNumber = modelNumber?.value;
    item.value.unitID = selectedUnit?.value.id

    if(!validateSubmission()) {
        toast.add({severity:'error', summary: 'Required Fields Empty', life: 2000 })
        console.log("item Invalid");
        return;
    }

    //Format Dates
    if(item.value.date_of_commissioning){
        let cDate = new Date(item.value.date_of_commissioning)
        cDate = cDate.toISOString()
        item.value.date_of_commissioning = cDate
    }
    if(item.value.date_of_reciept){
        let rDate = new Date(item.value.date_of_reciept)
        rDate = rDate.toISOString()
        item.value.date_of_reciept = rDate
    }

    const response = await addItem(item.value, selectedImage.value);

    if(response.successful){
        toast.add({severity:'success', summary: 'Operation Successful', life: 2000 })
        const newItem = response.newItem
        item.value.serialNumber="";
        item.value.localName="";
        item.value.barcode="";
        console.log("Save Successful " + JSON.stringify(newItem))
        emit('itemSaved',newItem)

        console.log(modelName.value)

        return true;
    }
    else{
        toast.add({severity:'error', summary: 'Operation unsuccessfull', detail: response.error.title, life: 2000 })
        return false;
    }
}

async function addNew(){
    if(await save()){
    item.value = new Item(new EquipmentModel())
    modelName.value = ""
    modelNumber.value = ""
    }
}

function assignImage(Image){
    selectedImage.value = Image
    imageDisplay.value.src = URL.createObjectURL(Image)
    showCamera.value = false
}

function imageScanConfirmed(scannedItem){
    item.value = scannedItem;
    modelName.value = scannedItem.model.modelName;
    modelNumber.value = scannedItem.model.modelNumber;
    showScanner.value = false;
}

const searchedModels = ref([new EquipmentModel()])
const modelName = ref("")
const modelNumber = ref("")

async function searchModelName(event){
    if(store.Models.length === 0) {await UpdateModels()}
    searchedModels.value = store.Models.filter(x => x.modelName.toLowerCase().includes(event.query.toLowerCase()))
}
async function searchModelNumber(event){
    if(store.Models.length === 0) {await UpdateModels()}
    searchedModels.value = store.Models.filter(x => x.modelNumber.toLowerCase().includes(event.query.toLowerCase()))
}

const searchedCategories = ref()
async function searchCategory(event){
    item.value.model.category = event.query
    if(store.ModelCategories.length === 0) {await UpdateCategories()}
    searchedCategories.value = store.ModelCategories.filter(x => x.toLowerCase().includes(event.query.toLowerCase()))
}

function modelNameSelected(event){
    item.value.model = event.value
    modelNumber.value = item.value.model.modelNumber
    modelName.value = item.value.model.modelName

}

function modelNumberSelected(event){
    item.value.model = event.value
    modelName.value = item.value.model.modelName
    modelNumber.value = item.value.model.modelNumber
    console.log(modelNumber.value)
}
</script>
<template>
<!--AI image scanner-->
    <Dialog v-model:visible="showScanner" modal header="Image" :closable=false :style="{ width: '400px' }" :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
        <InterpretImage @cancelled="showScanner = false" @confirmed="imageScanConfirmed"></InterpretImage>
    </Dialog>

<div class="container">
    <form >
        <Button @click="showScanner = true" label="Scan From Image" id="btn-scan" icon="pi pi-camera"></Button>
        <div class="input-group">
            <div class="input-field">
                <label for="serial">Serial Number</label>
                <small class="validation-warning" v-if="!isNotEmpty(item.serialNumber)">Required</small>
                <InputText id="serial" size="small" v-model="item.serialNumber" 
                    :invalid="showIsValid && !isNotEmpty(item.serialNumber)"/>

            </div>
            <div class="input-field">
                    <label for="serial">Barcode</label>
                    <InputGroup>
                        <InputText id="serial" size="small" v-model="item.barcode"/>
                        <Button icon="pi pi-barcode"> </Button>
                    </InputGroup>
                </div>
        </div>   
            <div class="input-group">
                <div class="input-field">
            <label for="owner">Owner</label>
            <small class="validation-warning" v-if="!isNotEmpty(selectedUnit) ">Required</small>
            <Select id="owner" size="small" v-model="selectedUnit" showClear 
            :options="store.Units" :invalid="showIsValid && !isNotEmpty(selectedUnit) "
            optionLabel="name"/>
        </div>
                <div class="input-field">
                    <label for="serial">Local Name</label>
                    <InputText id="serial" size="small" v-model="item.localName"/>
                </div>
                
       
        </div>
        <div class="input-field">
            <label for="status">Current Status</label>
            <small class="validation-warning" v-if="!isNotEmpty(item.currentStatus) ">Required</small>
            <InputText id="status" size="small" v-model="item.currentStatus" :invalid="showIsValid && !isNotEmpty(item.currentStatus)"/>
        </div>
        
        <div class="input-group">

        <div class="input-field">
            <label for="modelName">Model Name</label>
           <label class="validation-warning" v-if="!isNotEmpty(modelName) ">Required</label>
            <AutoComplete v-model="modelName" :suggestions="searchedModels" 
                optionLabel="modelName" @complete="searchModelName" 
                    @option-select="modelNameSelected" :invalid=" showIsValid && !isNotEmpty(modelName)" />
        </div>
        <div class="input-field">
            <label for="modelNum">Model Number</label>
            <small class="validation-warning" v-if="!isNotEmpty(modelNumber) ">Required</small>
            <AutoComplete v-model="modelNumber" :suggestions="searchedModels" optionLabel="modelNumber" 
            @complete="searchModelNumber" @option-select="modelNumberSelected"  :invalid="showIsValid && !isNotEmpty(modelNumber)"/>

        </div>
        </div>  
        <div class="input-group">
        <div class="input-field">
            <label for="Manufacturer">Manufacturer</label>
            <small class="validation-warning" v-if="!isNotEmpty(item.model.manufacturer) ">Required</small>
            <InputText id="manufacturer" size="small" v-model="item.model.manufacturer"  :invalid="showIsValid && !isNotEmpty(item.model.manufacturer)"/>
        </div>
        <div class="input-field ">
            <label for="category">Category</label>
            <small class="validation-warning" v-if="!isNotEmpty(item.model.category) ">Required</small> 
            <AutoComplete id="category" v-model="item.model.category" :suggestions="searchedCategories" dropdown
            @complete="searchCategory" 
            @option-select="(e) => item.model.category = e.value"
             :invalid="showIsValid && !isNotEmpty(item.model.category)" />
        </div>
        </div>      
        <div class="input-field">
                <label for="weight">Weight</label>
                <InputGroup class="num-input">
                    <InputGroupAddon>kg</InputGroupAddon>
                    <InputNumber id="weight" size="small" v-model="item.model.weight" :minFractionDigits="2"/>             
                </InputGroup>
            </div>      
            <div class="input-group">
                
            <div class="input-field">
                <label for="length">Length</label>
                <InputGroup class="num-input">
                    <InputGroupAddon>mm</InputGroupAddon>
                    <InputNumber id="length" size="small" v-model="item.model.length"/>             
                </InputGroup>
            </div>  
            <div class="input-field">
                <label for="depth">Depth</label>
                <InputGroup class="num-input">
                    <InputGroupAddon>mm</InputGroupAddon>
                    <InputNumber id="depth" size="small" v-model="item.model.width"/>             
                </InputGroup>
            </div>

            <div class="input-field">
                <label for="height">Height</label>
                <InputGroup class="num-input">
                    <InputGroupAddon>mm</InputGroupAddon>
                    <InputNumber id="height" size="small" v-model="item.model.height"/>             
                </InputGroup>
            </div>
        </div>
 
        <div class="input-group">
            <div class="input-field">
                <label for="recieptDate">Date of Reciept</label>
                <DatePicker showIcon icon-display="input" v-model="item.date_of_reciept" date-format="dd/mm/yy"/>
            </div>
            <div class="input-field">
                <label for="condition">Condition on Reciept</label>
                <SelectButton id="condition" size="small" v-model="item.condition_on_reciept" :options="condOptions"/>
            </div>
        </div>
        <div class="input-field">
        <label for="commisionDate">Date of Commissioning</label>
        <DatePicker showIcon icon-display="input" v-model="item.date_of_commissioning" date-format="dd/mm/yy"/>
        </div>
        
        <div class="card image-upload">
            <Button label="image" @click="showCamera = true" icon="pi pi-camera"></Button>
            <Dialog v-model:visible="showCamera" modal header="Choose Image" :style="{ width: '450px' }" :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
                <CaptureImage @imageConfirmed="assignImage" @cancelled="showCamera = false"/>
            </Dialog>
            <div class="img-container" v-show="selectedImage"><img ref="imageDisplay" v-bind:src="imageDisplay" alt="item-image"/></div>
        </div>
        <div class="submit-btns mobile" >
            <Button icon="pi pi-save" label="Save" class="s-btn" @click="save"></Button>
            <Button icon="pi pi-plus" label="New" class="s-btn" @click="addNew"></Button>
            <Button icon="pi pi-eraser" label="Clear" severity="danger" class="s-btn" @click="clear"></Button>
        </div>
    </form> 
</div>

</template>
<style scoped>
.container{
    display: flex;
    justify-content: center;
    flex: 1;
}
input{
    max-height: 35px;
    height: 35px;

}
.p-inputnumber-input{
    max-height: 35px;

}
.p-inputtext{
    max-height: 35px;

}
.p-button-icon-only{
    max-height: 35px;
}

.validation-warning{
    font-size: small;
    color: red !important;
}

.input-field{
    display: flex;
    flex-direction: column;
    justify-content: flex-end;
    width: fit-content;
    margin-top: 0.5rem;
    min-width: 150px;

}
.input-field label{
    color: black;
}

.input-group{
    /* border: black 2px solid; */
    display: flex;
    flex-wrap: wrap;
    gap:1rem;
}
form{
    display: flex;
    flex-direction: column;
    justify-content: center;
    width: fit-content;
}

img{
    max-width: 400px;
    max-height: 400px;
}
.img-container{
    box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
    border-radius: 10px;
    padding: 5px;
    margin: 1rem;
}

.num-input{
    max-width: 128px;
}
.submit-btns{
    display: flex;
    justify-content: center;
    flex-wrap: wrap;
    position: sticky;
    top:0;
    padding: 0.5rem;
    gap:0.5rem;
    z-index: 98;
    margin-top: 1rem;

}
.submit-btns Button{
    max-height: 3rem;
    flex: 1;
    max-width: 150px;
}

.container{
    flex: 1;
    display: flex;
    flex-wrap: wrap;
    padding: 1rem;
    
}

.s-btn :deep(.p-button-label){
    font-size: smaller;
    @container(width < 30px){
        display: none;
    }
}

.s-btn :deep(.p-button){
    padding: 0;
}

.p-fileupload :deep(.p-button){
    font-size: smaller;
    flex: 1;
    container-type: inline-size;
    max-width: 150px;
 
    }    

.p-fileupload :deep(.p-button-label){
        @container(width < 50px){
        display: none;
}
}

.p-fileupload-basic{
    justify-content: flex-start;
    width: 50%;
}
.image-upload{
    display:flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    gap: 1rem;
    flex: 1;
}

.image-upload .p-button{
    max-width: 200px;
    width: 100px;
    font-size: smaller;
}

Button {
    container-type: inline-size;
}
#btn-scan{
    max-width: 300px;
    width: 300px;
    min-width: 200px;
    justify-self: center;
    align-self: center;
}

.p-fileupload :deep(.p-fileupload-header){
    padding: 0.4rem;
    display: flex;
    justify-content: space-evenly
}

.mobile{
    position: sticky;
    top:0;
}
</style>