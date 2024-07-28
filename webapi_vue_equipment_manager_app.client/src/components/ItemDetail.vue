<script setup>
import {ref, watch} from 'vue'
import { UpdateItem, UploadImage } from '@/Services/ItemService';
import FileDisplay from './FileDisplay.vue';
import { UploadItemFile } from '@/Services/FileService';
import { store } from '@/Store/Store';


const editMode = ref(false)
const emit = defineEmits(['editTrue', 'update'])
const selectedItem = defineModel('selectedItem')
const changedItem = ref()
const uploadLoading = ref(false)
const saveLoading = ref(false)
const selectedUnit = ref()
const condOptions = ["New", "Used"]
const showImageCapture = ref(false)
const imgRef = ref()

const toggleEdit = () => {
    changedItem.value = selectedItem.value
    selectedUnit.value = {name: selectedItem.value.unitName, id : selectedItem.value.unitId}
    editMode.value = !editMode.value
    emit('editTrue')
}

watch(selectedItem, ()=> {
    changedItem.value = selectedItem.value
    selectedUnit.value = {name: selectedItem.value.unitName, id : selectedItem.value.unitId}})

async function uploadFile(file){
    uploadLoading.value = true
    let result = await UploadItemFile(file, selectedItem.value.id)
    if(result.successfull){
        selectedItem.value.documents.push(result.file)
    }
    console.log(result)
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
    console.log(changedItem.value)
    let response = await UpdateItem(changedItem.value)
    if(response.successful){
        selectedItem.value = response.item
        emit('update', selectedItem.value)
    }
    saveLoading.value = false
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
                <label class="fieldName">Local Identifier</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.localName }}</label>
                <InputText v-if="editMode" v-model=changedItem.localName></InputText>
            </div>
        </div>
        <div class ="field-row">
            <div class="field">
                <label class="fieldName">Barcode</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.barcode }}</label>
                <InputGroup v-if="editMode" >
                            <InputText id="serial" size="small" v-model="changedItem.barcode"/>
                            <Button icon="pi pi-barcode"> </Button>
                </InputGroup>
            </div>
            <div class="field">
                <label class="fieldName">Owner</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.unitName }}</label>
                <Select  v-if="editMode" id="owner" size="small" v-model="selectedUnit" :placeholder="selectedUnit.name" showClear 
            :options="store.Units"
            optionLabel="name"/>
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
            <div class="field">
                <label class="fieldName">Weight</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.weight }}</label>
                <InputGroup class="num-input" v-show="editMode">
                    <InputGroupAddon>kg</InputGroupAddon>
                    <InputNumber v-if="editMode" id="weight" size="small" v-model="changedItem.model.weight" :minFractionDigits="2"/>             
                </InputGroup>
            </div>
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

    <div class="field-group">
        <div class ="field-row">
        <div class="field">
            <label class="fieldName">Date of Reciept</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.date_of_reciept }}</label>
            <DatePicker  v-if="editMode" showIcon icon-display="input" v-model="changedItem.date_of_reciept" date-format="dd/mm/yy"/>

        </div>
        <div class="field">
            <label class="fieldName">Condition on Reciept</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.condition_on_reciept }}</label>
            <SelectButton v-if="editMode" id="condition" size="small" v-model="changedItem.condition_on_reciept" :options="condOptions"/>

        </div>
        </div>
        <div class="field">
            <label class="fieldName">Date of Commissioning</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.date_of_commissioning }}</label>
            <DatePicker  v-if="editMode" showIcon icon-display="input" v-model="changedItem.date_of_commissioning" date-format="dd/mm/yy"/>

        </div>

        <div class="field">
            <label class="fieldName">Current Status</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.currentStatus }}</label>
            <InputText  v-if="editMode" v-model="changedItem.currentStatus"></InputText>
        </div>
    </div>
</div>
    <div class="panel-2">
        <div class="edit-bttns">
            <Button label="Edit" v-if="!editMode" @click="toggleEdit"></Button>
            <Button label="Save Changes" v-if="editMode" @click="update()" :loading="saveLoading"></Button>
            <Button label="Cancel" v-if="editMode" @click="toggleEdit" severity="danger" ref="imgRef"></Button>
        </div>
        <img id="image" :src="selectedItem.imageUrl" v-if="selectedItem.imageUrl"/>
        <div class="img-placeholder" style="width: 200px; height: 200px; background-color: var(--p-surface-100); border: solid 1px black" v-if="!selectedItem.imageUrl"></div>
        <div class="image-upload">
            <Button label="image" icon="pi pi-upload" @click="showImageCapture = true"></Button>
        </div>
        <FileDisplay header="Documents" v-model="selectedItem.documents"  @upload="uploadFile" :upload-loading="uploadLoading"></FileDisplay>
    </div>
    <Dialog modal v-model:visible="showImageCapture"><CaptureImage @cancelled="showImageCapture = false" @imageConfirmed="setImage"></CaptureImage></Dialog>
</div>
</template>
<style scoped>

.container{
    display: flex;
    flex-wrap: wrap;
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
    min-width: 340px;
    max-width: 550px;
    width: 100%;
    flex: 1;
}

#image{
    aspect-ratio: 1/1;
    background-color: var(--p-surface-200);
    min-width: 96px;
    max-width: 250px;
    width: 100%;
    height: fit-content;        
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
width: 100%;
display: flex;
flex-direction: column;
}
.field-row{
    display: flex;
    flex-wrap: wrap;
    
}
.field{
    height: auto;
    display: flex;
    flex-direction: column;
    max-height: 100px;
    max-width: 300px;
    min-width: 100px;
    margin: 10px;
}
.dimensions{
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;

}
.fieldName{
    font-weight: bold;
    margin-bottom: 4px;
}


.numerical .field{
    flex: 1
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
</style>