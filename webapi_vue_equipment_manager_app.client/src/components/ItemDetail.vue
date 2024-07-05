<script setup>
import {ref} from 'vue'
import {Item} from '@/Models/Item'
import {EquipmentModel} from '@/Models/EquipmentModel'


const selectedItem = ref(new Item(new EquipmentModel()))
const editMode = ref(false)
const emit = defineEmits(['editTrue'])

const toggleEdit = () => {
    editMode.value = !editMode.value
    emit('editTrue')}

const upload = () => {}

</script>

<template>
<div class="container">
<div class="fields">
    <div class="detail-box">
        <div class="field">
            <label class="fieldName">Serial Number</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.serialNumber }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
        <div class="field">
            <label class="fieldName">Local Identifier</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.localName }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
        <div class="field">
            <label class="fieldName">Barcode</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.barcode }}</label>
            <InputText v-show="editMode"></InputText>

        </div>
        <div class="field">
            <label class="fieldName">Owner</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.unitName }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
    </div>
    <div>
    <div class="detail-box">
        <div class="field">
            <label class="fieldName">Model Name</label>
            <label class="fieldValue">{{ selectedItem.model?.modelName }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
        <div class="field">
            <label class="fieldName">Model Number</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.modelNumber }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
        <div class="field">
            <label class="fieldName">Manufacturer</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.manufacturer }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
        <div class="field">
            <label class="fieldName">Category</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.category }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
       </div>
        <div class="numerical">
            <div class="field">
                <label class="fieldName">Weight</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.weight }}</label>
                <InputText v-show="editMode"></InputText>
            </div>
            <div class="field">
                <label class="fieldName">Height</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.height }}</label>
                <InputText v-show="editMode"></InputText>
            </div>
            <div class="field">
                <label class="fieldName">Length</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.length }}</label>
                <InputText v-show="editMode"></InputText>
            </div>
            <div class="field">
                <label class="fieldName">Depth</label>
                <label class="fieldValue" v-show="!editMode">{{ selectedItem.model?.depth }}</label>
                <InputText v-show="editMode"></InputText>
            </div>
        </div>
    </div>

    <div class="detail-box">
        <div class="field">
            <label class="fieldName">Date of Reciept</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.date_of_reciept }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
        <div class="field">
            <label class="fieldName">Date of Commissioning</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.date_of_commissioning }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
        <div class="field">
            <label class="fieldName">Purchase Order</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.purchaseOrder }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
        <div class="field">
            <label class="fieldName">Condition on Reciept</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.condition_on_reciept }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
        <div class="field">
            <label class="fieldName">Current Status</label>
            <label class="fieldValue" v-show="!editMode">{{ selectedItem.currentStatus }}</label>
            <InputText v-show="editMode"></InputText>
        </div>
    </div>
</div>
    <div class="panel-2">
        <div class="edit-bttns">
            <Button label="Edit" v-if="!editMode" @click="toggleEdit"></Button>
            <Button label="Save Changes" v-if="editMode"></Button>
            <Button label="Cancel" v-if="editMode" @click="toggleEdit"></Button>
        </div>
        <img id="image"/>
        <div class="upload">
        <div class="card flex justify-center">
        <Toast />
        <FileUpload mode="basic" name="demo[]" url="/api/upload" accept=".pdf" :maxFileSize="1000000" @upload="upload" :auto="true" chooseLabel="Image" />
        </div>
        </div>
        <FileDisplay id="files"></FileDisplay>
    </div>
</div>
</template>
<style scoped>

.col-12{
    border: 1px solid black;
    flex: 1;
}

.container{
    display: flex;
    flex-wrap: wrap;
    flex: 1;
    justify-content: space-around;
    

}
.fields{

    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
    min-width: 340px;
    max-width: 800px;
    width: 100%;
    flex: 1;
    padding-left: 2rem;

}
.panel-2{
    justify-content: flex-start;
    display: flex;
    flex-direction: column;
    gap: 10px;
    align-items: center;
    min-width: 250px;
    max-width: 400px;
    padding: 5px;
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
    .files{
        height: fit-content;
        flex:0;
    }
.edit-bttns{
    width: 100%;
    display: flex;
    flex-direction: column;
    gap: 10px;
    padding-inline: 10px;
}
.p-inputtext{
    width: fit-content;
}

.detail-box{
    display: flex;
    flex-wrap: wrap;
    flex: 1;
    justify-content: flex-start;
}
.field{

    height: auto;
    display: flex;
    flex-direction: column;
    flex: 1 0 50%;
}
.fieldName{

}

.numerical{
    display: flex;
    flex: 1;
    justify-content: flex-start;
    flex-wrap: wrap;
}
.numerical .field{
    flex: 1
}

</style>