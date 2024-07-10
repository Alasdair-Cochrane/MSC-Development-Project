<script setup>
    import { ref, onMounted } from "vue"
    import { Item } from '@/Models/Item';
    import {EquipmentModel } from '@/Models/EquipmentModel'
    import { addItem, queryLabelImage} from '@/Services/ItemService'
    import { store, UpdateModels } from "@/Store/Store";
    import { IsMobile } from "@/Services/DeviceService";
    import {useToast} from 'primevue/usetoast'
    import InterpretImage from "@/components/InterpretImage.vue";


const item = ref(new Item(new EquipmentModel()))
var mobile = ref(false);
const condOptions = ["New", "Used"]
const toast = useToast();
const emit = defineEmits('itemSaved')
const selectedImage = ref()
const labelImage = ref()
const showScanner = ref(false)



onMounted(() => {
    mobile.value = IsMobile();
    item.value.condition_on_reciept = condOptions[0]
})

const imageUploaded = ref()

const  isNotEmpty = (model) =>{
    if(model === null || model === "" || model === undefined) {
        return false;
    }
    else{
        return true;
    }
}
const requiredMessage = "Field Is Required"

const validateSubmission = () =>
{
    if(
        isNotEmpty(item.value.serialNumber) &&
        isNotEmpty(item.value.unitName) &&
        isNotEmpty(item.value.model.modelNumber) &&
        isNotEmpty(item.value.model.modelName) &&
        isNotEmpty(item.value.category) &&
        isNotEmpty(item.value.currentStatus)
    ) return true;
    else {
        return false
    }
}


function clear(){
    item.value = new Item(new EquipmentModel())
}

async function save(){
    if(!validateSubmission()) {
        return;
    }
    const response = await addItem(item.value, selectedImage.value);
    if(response.successful){
        toast.add({severity:'success', summary: 'Operation Successful', life: 2000 })
        const newItem = response.item
        item.value.serialNumber="";
        item.value.localName="";
        item.value.barcode="";
        emit("itemSaved",newItem)
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

function imageSelected(event){
    selectedImage.value = event.files[0]
    console.log("image selected")
}

function labelImageSelected(event){
    labelImage.value = event.files[0]
}
async function scanImage(){
    if(labelImage.value)
    var response = await queryLabelImage(labelImage.value)
    console.log(response)
    if(response.isValidLabel){
        item.value = new Item(new EquipmentModel(null,
        response.item.modelNumber,
        response.item.modelName,
        null,
        response.item.manufacturer,
        response.item.weight,
        response.item.height,
        response.item.length,
        response.item.width,
        response.item.category),
        null,
        response.item.serialNumber)
        modelName.value = response.item.modelName,
        modelNumber.value = response.item.modelNumber
    }
    
}


const searchedModels = ref([new EquipmentModel()])
const modelName = ref("")
const modelNumber = ref("")

async function searchModelName(event){
    item.value.model.modelName = event.query
    if(store.Models.length === 0) {await UpdateModels()}
    searchedModels.value = store.Models.filter(x => x.modelName.startsWith(event.query))
}
async function searchModelNumber(event){
    item.value.model.modelNumber = event.query
    if(store.Models.length === 0) {await UpdateModels()}
    searchedModels.value = store.Models.filter(x => x.modelNumber.startsWith(event.query))
}

function modelNameSelected(event){
    item.value.model = event.value
    modelNumber.value = item.value.model.modelNumber
}

function modelNumberSelected(event){
    item.value.model = event.value
    modelName.value = item.value.model.modelName
}
</script>
<template>
    <Dialog v-model:visible="showScanner" modal header="Add New Item" :closable=false>
        <InterpretImage @cancelled="showScanner = false"></InterpretImage>
    </Dialog>
<div class="grid-nogutter container">
<div class="col-12 md:col-7">
        <Button @click="showScanner = true" label="scan"></Button>
    <form >
        <div class="input-group">
            <div class="input-field">
                <label for="serial">Serial Number</label>
                <!-- <label class="validation-warning" v-if="!isNotEmpty(item.serialNumber) ">{{ requiredMessage }}</label> -->
                <InputText id="serial" size="small" v-model="item.serialNumber" :invalid="!isNotEmpty(item.serialNumber)"/>
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
                    <label for="serial">Local Name</label>
                    <InputText id="serial" size="small" v-model="item.localName"/>
                </div>
                
                <div class="input-field">
            <label for="owner">Owner</label>
            <!-- <label class="validation-warning" v-if="!isNotEmpty(item.unitName) ">{{ requiredMessage }}</label> -->
            <Select id="owner" size="small" v-model="item.unitName" showClear 
            :options="store.Units" :invalid="!isNotEmpty(item.unitName)"/>
        </div>
        </div>
        
        <div class="input-group">
        <div class="input-field">
            <label for="modelName">Model Name</label>
            <!-- <label class="validation-warning" v-if="!isNotEmpty(modelName) ">{{ requiredMessage }}</label> -->
            <AutoComplete v-model="modelName" :suggestions="searchedModels" optionLabel="modelName" @complete="searchModelName" @option-select="modelNameSelected" :invalid="!isNotEmpty(modelName)" />
        </div>
        <div class="input-field">
            <label for="modelNum">Model Number</label>
            <!-- <label class="validation-warning" v-if="!isNotEmpty(modelNumber) ">{{ requiredMessage }}</label> -->
            <AutoComplete v-model="modelNumber" :suggestions="searchedModels" optionLabel="modelNumber" @complete="searchModelNumber" @option-select="modelNumberSelected"  :invalid="!isNotEmpty(modelNumber)"/>

        </div>
        </div>  
        <div class="input-group">
        <div class="input-field">
            <label for="Manufacturer">Manufacturer</label>
            <!-- <label class="validation-warning" v-if="!isNotEmpty(item.model.manufacturer) ">{{ requiredMessage }}</label> -->
            <InputText id="manufacturer" size="small" v-model="item.model.manufacturer"  :invalid="!isNotEmpty(item.model.manufacturer)"/>
        </div>
        <div class="input-field ">
            <label for="category">Category</label>
            <!-- <label class="validation-warning" v-if="!isNotEmpty(item.model.category) ">{{ requiredMessage }}</label> -->
            <InputText id="category" size="small" v-model="item.model.category" :invalid="!isNotEmpty(item.model.category)"/>
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
                <DatePicker showIcon icon-display="input" v-model="item.date_of_reciept"/>
            </div>
            <div class="input-field">
            <label for="condition">Condition on Reciept</label>
            <SelectButton id="condition" size="small" v-model="item.condition_on_reciept" :options="condOptions"/>
            </div>
        </div>
        <div class="input-field">
        <label for="commisionDate">Date of Commissioning</label>
        <DatePicker showIcon icon-display="input" v-model="item.date_of_commissioning"/>
        </div>
        <div class="input-field">
            <label for="status">Current Status</label>
            <!-- <label class="validation-warning" v-if="!isNotEmpty(item.currentStatus) ">{{ requiredMessage }}</label> -->
            <InputText id="status" size="small" v-model="item.currentStatus" :invalid="!isNotEmpty(item.currentStatus)"/>
        </div>
        <div class="card image-upload">
            <label>Upload Image</label>
                <FileUpload  
                ref="imageUploaded" 
                mode="basic" 
                name="imageFile[]" 
                accept="image/*" 
                :maxFileSize="1000000"
                :custom-upload="true"
                @select="imageSelected"                
                />
        </div>
        <div class="submit-btns mobile" >
            <Button icon="pi pi-save" label="Save" class="s-btn" @click="save"></Button>
            <Button icon="pi pi-plus" label="New" class="s-btn" @click="addNew"></Button>
            <Button icon="pi pi-eraser" label="Clear" severity="danger" class="s-btn" @click="clear"></Button>
        </div>
    </form>
    </div>

    <div  class="col-12 md:col-5 right">
        <Image v-bind:src="item.imageUrl" width="250" preview></Image>
        

    </div>
    
</div>

</template>
<style scoped>

.validation-warning{
    font-size: small;
    color: red !important;
}


.input-field{
    display: grid;
    grid-template-columns: 1fr;
    grid-template-rows: 1fr, 1fr;
    width: fit-content;
    margin-right: 0.5rem;
    margin-top: 0.5rem;

}
.input-field label{
    color: black;
}

.input-group{
    /* border: black 2px solid; */
    display: flex;
    flex-wrap: wrap;
    justify-content: flex-start;
    gap:1rem;
}
form{
    display: flex;
    flex-direction: column;
}


.num-input{
    max-width: 128px;
}
.submit-btns{
    display: flex;
    justify-content: space-around;
    flex-wrap: wrap;
    position: sticky;
    top:0;
    padding: 0.5rem;
    gap:0.5rem;
    z-index: 98;

}
.submit-btns Button{
    height: 3rem;
    flex: 1;
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
    margin-block: 2rem;
    display:flex;
    flex-wrap: wrap;
    justify-content: space-between;
}
.image-upload label{
    width:100%;
}
.image-upload .p-button{
    flex:1;
    max-width: 100px;
    font-size: smaller;
}

Button {
    container-type: inline-size;
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