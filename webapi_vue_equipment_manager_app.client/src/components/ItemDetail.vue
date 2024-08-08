<script setup>
import {ref, watch} from 'vue'
import { DeleteItem, UpdateItem, UploadImage } from '@/Services/ItemService';
import FileDisplay from './FileDisplay.vue';
import { UploadItemFile } from '@/Services/FileService';
import { store } from '@/Store/Store';
import { FormatDate } from '@/Services/FormatService';

const emit = defineEmits(['editTrue', 'update', 'deleted'])
const selectedItem = defineModel('selectedItem')

const editMode = ref(false)

const changedItem = ref()
const uploadLoading = ref(false)
const saveLoading = ref(false)

const selectedUnit = ref()
const condOptions = ["New", "Used"]

const showImageCapture = ref(false)
const showBarcodeScanner = ref(false)

const showDelete = ref(false)
const deleteLoading = ref(false)
const deletionSuccesfull = ref(false)
const deleteErrorMessage = ref()
const deleteErrorOccured = ref(false)

const maxDate = ref(new Date())

const imgRef = ref()

const toggleEdit = () => {
    changedItem.value = selectedItem.value
    selectedUnit.value = selectedItem.value.unit
    editMode.value = !editMode.value
    emit('editTrue')
}

watch(selectedItem, ()=> {
    changedItem.value = selectedItem.value
    deleteErrorOccured.value = false
    selectedUnit.value = {name: selectedItem.value.unitName, id : selectedItem.value.unitId}})

async function uploadFile(file){
    uploadLoading.value = true
    let result = await UploadItemFile(file, selectedItem.value.id)
    if(result.successfull){
        selectedItem.value.documents.push(result.file)
    }
    uploadLoading.value = false
}

async function update() {
    saveLoading.value = true;
    if(!changedItem.value){
        saveLoading.value = false
        return
    }
    changedItem.value.unitName = selectedUnit.value.name
    changedItem.value.unitId = selectedUnit.value.id
    let response = await UpdateItem(changedItem.value)
    if(response.successful){
        selectedItem.value = response.item
        emit('update', selectedItem.value)
        editMode.value = false
    }
    saveLoading.value = false
}

async function deleteItem(){
    deleteLoading.value = true
    let response = await DeleteItem(selectedItem.value.id)
    if(response.successfull){   
        deletionSuccesfull.value = true
        emit('deleted')
    }
    else{
        deleteErrorOccured.value = true;
        deleteErrorMessage.value = response.message ?? response.error.title        
    }
    deleteLoading.value = false

}

async function setImage(image){
    let response = await UploadImage(selectedItem.value.id, image)
    if(response.successful){
        selectedItem.value.imageUrl = response.url
    }
    //selectedItem.value.imageUrl = URL.createObjectURL(image);
    showImageCapture.value = false
}
</script>

<template>

    <!--BARCODE SCANNER-->
<Dialog :visible="showBarcodeScanner" modal :closable="false">
    <BarcodeScanner @cancelled="showBarcodeScanner= false" @confirmed="(b) => {selectedItem.barcode = b; showBarcodeScanner = false}"></BarcodeScanner>
</Dialog>
<!--DELETE CONFIRM-->
<Dialog v-model:visible="showDelete" modal>
   <div class="deletion" v-show="!deletionSuccesfull">
       <strong>Confirm Deletion?</strong>
       <div>
            <Button label="Confirm" @click="deleteItem()" :loading="deleteLoading"></Button>
           <Button label="Cancel" @click="showDelete = false" severity="danger"></Button>
       </div>
       <span v-show="deleteErrorOccured"> Failed to Delete : {{ deleteErrorMessage }}</span>
   </div>
   <div v-show="deletionSuccesfull" class="deletion">
       <label>Deletetion Successfull</label>
       <Button label="Close" @click="deletionSuccesfull = false; showDelete = false"></Button>
   </div>
</Dialog>
<div class="container">
<div class="fields">
    <div class="field-group">
        <div class ="field-row">
            <div class="field">
                <label class="fieldName">Serial Number</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.serialNumber }}</label>
                <InputText v-if="editMode" v-model=changedItem.serialNumber></InputText>
            </div>
            <div class="field">
                <label class="fieldName">Local Name</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.localName  ?? "---" }}</label>
                <InputText v-if="editMode" v-model=changedItem.localName></InputText>
            </div>
        </div>
        <div class ="field-row">
            <div class="field">
            <label class="fieldName">Current Status</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.currentStatus }}</label>
            <Select  v-if="editMode" id="status" size="small" v-model="changedItem.currentStatus" :placeholder="changedItem.currentStatus"
            :options="store.Statuses"/>
        </div>
            <div class="field">
                <label class="fieldName">Barcode</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.barcode }}</label>
                <InputGroup v-if="editMode" >
                            <InputText id="serial" size="small" v-model="changedItem.barcode"/>
                            <Button icon="pi pi-barcode" @click="showBarcodeScanner= true"> </Button>
                </InputGroup>
            </div>
            
        </div>
    </div>
    <div class="field-group">
        <div class="field-row">
            <div class="field">
                <label class="fieldName">Owner</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.unit.name }}</label>
                <Select  v-if="editMode" id="owner" size="small" v-model="selectedUnit" :placeholder="selectedUnit.name" 
            :options="store.Units"
            optionLabel="name"/>
            </div>
            <div class="field">
                <label class="fieldName">Room</label>
                <label class="fieldValue" v-if="!editMode">{{ selectedItem.unit.room}}</label>
                <label class="fieldValue" v-if="editMode">{{ selectedUnit.room}}</label>

            </div>
        </div>
        <div class="field-row">
            <div class="field">
                <label class="fieldName">Building</label>
                <label class="fieldValue" v-if="!editMode">{{ selectedItem.unit.building}}</label>
                <label class="fieldValue" v-if="editMode">{{ selectedUnit.building}}</label>

            </div>
            <div class="field">
                <label class="fieldName">Address</label>
                <p class="fieldValue" v-if="!editMode">{{ selectedItem.unit.address}}</p>
                <p class="fieldValue" v-if="editMode">{{ selectedUnit.address}}</p>

            </div>
        </div>
    </div>
    <div class="field-group">
        <div class ="field-row">
            <div class="field">
                <label class="fieldName">Model Name</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.modelName }}</label>
                <InputText  v-if="editMode" v-model="changedItem.model.modelName"></InputText>
            </div>
            <div class="field">
                <label class="fieldName">Model Number</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.modelNumber }}</label>
                <InputText  v-if="editMode" v-model="changedItem.model.modelNumber"></InputText>
            </div>
        </div>
        <div class ="field-row">
            <div class="field">
                <label class="fieldName">Manufacturer</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.manufacturer }}</label>
                <InputText  v-if="editMode" v-model="changedItem.model.manufacturer"></InputText>
            </div>
            <div class="field">
                <label class="fieldName">Category</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.category }}</label>
                <InputText  v-if="editMode" v-model=changedItem.model.category></InputText>
            </div>
        </div>
        <div class ="field-row">
            <div class="field">
                <label class="fieldName">Weight</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.weight }}</label>
                <InputGroup class="num-input" v-show="editMode">
                    <InputGroupAddon>kg</InputGroupAddon>
                    <InputNumber v-if="editMode" id="weight" size="small" v-model="changedItem.model.weight" :minFractionDigits="2"/>             
                </InputGroup>
            </div>
            <div class="field"></div>
        
            <div class="dimensions">
                <div class="field">
                    <label class="fieldName">Height</label>
                    <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.height }}</label>
                    <InputGroup class="num-input" v-show="editMode">
                        <InputGroupAddon>mm</InputGroupAddon>
                        <InputNumber v-if="editMode" id="weight" size="small" v-model="changedItem.model.height" :minFractionDigits="2"/>             
                    </InputGroup>
                </div>
                <div class="field">
                    <label class="fieldName">Length</label>
                    <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.length }}</label>
                    <InputGroup class="num-input" v-show="editMode">
                        <InputGroupAddon>mm</InputGroupAddon>
                        <InputNumber v-if="editMode" id="weight" size="small" v-model="changedItem.model.length" :minFractionDigits="2"/>             
                    </InputGroup>
                </div>
                <div class="field">
                    <label class="fieldName">Width</label>
                    <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.width }}</label>
                    <InputGroup class="num-input" v-show="editMode">
                        <InputGroupAddon>mm</InputGroupAddon>
                        <InputNumber v-if="editMode" id="weight" size="small" v-model="changedItem.model.width" :minFractionDigits="2"/>             
                    </InputGroup>
                </div>
            </div>
        </div>
    </div>

    <div class="field-group">
        <div class ="field-row">
        <div class="field">
            <label class="fieldName">Reciept Date</label>
            <label class="fieldValue" v-show="!editMode">{{ FormatDate(selectedItem.date_of_reciept) }}</label>
            <DatePicker  v-if="editMode" showIcon icon-display="input" v-model="changedItem.date_of_reciept" date-format="dd/mm/yy" :maxDate="maxDate"/>

        </div>
        <div class="field">
            <label class="fieldName">Condition on Reciept</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.condition_on_reciept }}</label>
            <SelectButton v-if="editMode" id="condition" size="small" v-model="changedItem.condition_on_reciept" :options="condOptions"/>

        </div>
        </div>
        <div class ="field-row">
        <div class="field">
            <label class="fieldName">Commissioning Date</label>
            <label class="fieldValue" v-show="!editMode">{{ FormatDate(selectedItem.date_of_commissioning) }}</label>
            <DatePicker  v-if="editMode" showIcon icon-display="input" v-model="changedItem.date_of_commissioning" date-format="dd/mm/yy" :maxDate="maxDate"/>
        </div>
        <div class="field">
            <label class="fieldName">Date Added</label>
            <label class="fieldValue" v-show="!editMode">{{ FormatDate(selectedItem.dateCreated) }}</label>
        </div>

    </div>

        
    </div>
</div>
    <div class="panel-2">
        <div class="edit-bttns">
            <Button label="Edit"  v-if="!editMode" @click="toggleEdit"></Button>
            <Button label="Delete" v-if="!editMode" @click="showDelete = true" severity="danger"></Button>

            <Button label="Save Changes" v-if="editMode" @click="update()" :loading="saveLoading"></Button>
            <Button label="Cancel" v-if="editMode" @click="toggleEdit" severity="danger" ref="imgRef"></Button>
        </div>
        <img id="image" :src="selectedItem.imageUrl" v-if="selectedItem.imageUrl && !editMode" />
        <div class="image-upload" v-if="!editMode">
            <Button label="image" icon="pi pi-upload" @click="showImageCapture = true"></Button>
        </div>
        <div class="panel">
            <FileDisplay header="Documents" v-model="selectedItem.documents"  @upload="uploadFile" :upload-loading="uploadLoading" v-if="!editMode"></FileDisplay>
        </div>
    </div>
    <Dialog modal v-model:visible="showImageCapture"><CaptureImage @cancelled="showImageCapture = false" @imageConfirmed="setImage"></CaptureImage></Dialog>
</div>
</template>
<style scoped>

.container{
    display: flex;
    flex-wrap: wrap;
    gap: 1rem;
}
.p-inputtext{
    max-width: 200px;
}
.p-inputgroup{
    max-width: 150px;
}
.p-inputwrapper{
    max-width: 200px;
}
.fields{
    display: flex;
    flex-wrap: wrap;
    min-width: 300px;
    max-width: 550px;
    flex: 1;
    gap: 10px;
}
.panel{
    background-color: white;
    box-shadow: 0 2px 2px 0 rgba(28, 25, 25, 0.2);
    border: black solid 1px;
    border-radius: 10px;
}

#image{
    aspect-ratio: 1/1;
    min-width: 96px;
    max-width: 250px;
    width: 100%;
    height: fit-content;        
    box-shadow: 0 2px 2px 0 rgba(28, 25, 25, 0.2);
    background-color: white;
    border: 1px solid black;
    border-radius: 10px;
    padding: 5px;
    
    }

.edit-bttns{
    width: 100%;
    max-width: 200px;
    display: flex;
    flex-direction: column;
    gap: 10px;
    padding-inline: 10px;
}
.field-group{
display: flex;
flex-direction: column;
box-shadow: 0 2px 2px 0 rgba(28, 25, 25, 0.2);
background-color: white;
border: solid black 1px;
border-radius: 10px;
width: 100%;
max-width: 550px;
padding: 10px;

}
.field-row{
    display: flex;
    flex-wrap: wrap;
    justify-content: space-evenly;
    
    
}
.field{
    height: auto;
    display: flex;
    flex-direction: column;
    max-height: 100px;
    max-width: 300px;
    min-width: 150px;
    align-items: center;
}
.dimensions{
    display: flex;
    justify-content: space-evenly;
    flex-wrap: wrap;

}
.fieldName{
    font-weight: bold;
    margin-bottom: 4px;
}


.dimensions .field{
    flex: 1;
    min-width: 90px;
}
.panel-2{
    justify-content: flex-start;
    display: flex;
    flex-direction: column;
    gap: 10px;
    align-items: center;
    padding: 5px;
    max-width: 500px;
    width: fit-content;
}

@media(max-width:760px){
    .field-row{
        gap: 0;
    }
    .field{
        min-width: 120px;
    }
            
}

small{
    color: red
}

.deletion{
    display: flex;
    flex-direction: column;
    gap: 2rem;
    align-items: center;
}
.deletion strong{
    font-weight: bold;
}
.deletion div{
    display: flex;
    gap: 2rem;
}

</style>