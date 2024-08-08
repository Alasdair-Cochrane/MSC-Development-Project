<script setup>
import { UploadMaintenanceFile } from '@/Services/FileService';
import { FormatDate } from '@/Services/FormatService';
import { DeleteMaintenance } from '@/Services/MaintenanceService';
import { GetFromAPI } from '@/Store/Store';
import { onBeforeMount,  ref, watch } from 'vue';

const maintenaceList = defineModel('list')
const maintenance = defineModel('maintenance')
const deleteConfirmed = ref(false)
const deleteLoading = ref(false)
const errorMessage = ref()
const documents = ref([])
const fileUploadLoading = ref(false)
const emit = defineEmits(['delete'])
const props = defineProps({
    item:{},
    showDelete:{
        type:Boolean,
        default: true,
    }
})

onBeforeMount(async () => {
    documents.value =  await getFiles();
    
})

const getFiles = async () => {
    let files  = await GetFromAPI(`api/items/maintenance/${props.maintenance.id}/documents`)
    if(!files){
        return []
    }
    else{
        return files;
    }
}

watch(maintenance, async () => {documents.value = await getFiles()})

const maintenanceFileUpload = async (file) =>{
    fileUploadLoading.value = true;
    let result = await UploadMaintenanceFile(file, props.maintenance.id)
    if(result.successfull){
        documents.value.push(result.file)
    }
    fileUploadLoading.value = false
}

const deleteClicked = async () =>{
    deleteLoading.value = true;
    let result = await DeleteMaintenance(props.maintenance.id)
    deleteLoading.value = false;
    if(result.successfull){
        maintenaceList.value = maintenaceList.value.filter(x => x.id !== props.maintenance.id)
        deleteConfirmed.value = true
        emit('delete')
    }
    else{
        errorMessage.value = result.error
    }
}
</script>
<template>
    <div class="wrapper" v-if="!deleteConfirmed">
            <h3 v-if="item">{{item.serialNumber}}</h3>
            <h3 v-if="maintenance.serialNumber">{{ maintenance.serialNumber }}</h3>
        <div class="field">
            <label class="header">Type</label>
            <label class="value">{{maintenance.categoryName}}</label>
        </div>
        <div class="field">
            <label class="header">Date Completed</label>
            <label class="value">{{FormatDate(maintenance.date_Completed)}}</label>
        </div>
        <div class="field">
            <label class="header">Provider</label>
            <label class="value">{{maintenance.provider_Name}}</label>
        </div>
        <div class="field">
            <label class="header">Description</label>
            <p class="value">{{maintenance.description}}</p>
        </div>
        <FileDisplay header="Files" @upload="maintenanceFileUpload" v-model="documents" :uploadLoading="fileUploadLoading"></FileDisplay>
        <div class="footer-bttn">
            <Button v-if="showDelete" icon="pi pi-trash" label="Delete" severity="danger" @click="deleteClicked()" :loading="deleteLoading"></Button>
            <span style="color: red;" v-if="!deleteConfirmed && errorMessage">{{ errorMessage }}</span>
        </div>        
    </div>
    <div class="deleteConfirmation" v-if="deleteConfirmed">

    </div>
</template>
<style scoped>
.field{
    display: flex;
    flex-direction: column;
    align-items: center;
}
.header{
    font-weight: bold;
}
h3{
    font-weight: bold;
    margin-bottom: 2rem;
}
.wrapper{
    min-width: 250px;
    display: flex;
    flex-direction: column;
    align-items: center;
}
.footer-bttn{
    display: flex;
    justify-content: flex-end;
    width: 100%;
    margin-top: 1rem;
}

p{
    max-width: 400px;
    max-height: 300px;
    overflow-y: auto;
    margin-bottom: 1rem;
}

</style>
