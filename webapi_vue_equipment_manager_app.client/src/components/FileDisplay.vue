<script setup>
import { DeleteFile, Download, } from '@/Services/FileService';
import {onBeforeUnmount, onMounted, ref} from 'vue'

const files = defineModel()
const props = defineProps({header:{type:String},uploadLoading:{type:Boolean}})
const fileSelected = ref()
const elementSelected = ref()
const loading = ref(false)
const deleteLoading = ref(false)
const selectedNewFile = ref()
const emit = defineEmits(['upload'])

function deselectFile(event) {
    if(event.target !== elementSelected.value){
        elementSelected.value = null
    }
}
function selectFile(event){
    elementSelected.value = event.target
}

function newFileSelected(event){     
    selectedNewFile.value = event.target.files[0]
    console.log(selectedNewFile.value)
}
onMounted(() => {
    document.addEventListener('click', deselectFile)
})
onBeforeUnmount(() => {
    document.removeEventListener('click', deselectFile)
})

const deleteFile = async  () => {
    deleteLoading.value = true
    console.log("happened")
    console.log(fileSelected.value)
    let result = await DeleteFile(fileSelected.value.document)
    if(result.successfull){
        files.value = files.value.filter(x => x !== fileSelected.value)
    }
    console.log(result)
    deleteLoading.value = false

}
const downloadFile = async (file) =>{
    loading.value = true
    console.log(file)
    await Download(file.document)
    loading.value = false;
}

const uploadFile = async () => {
    if(selectedNewFile.value){
        emit('upload', selectedNewFile.value)
    }
}

</script>

<template>
<div class="container">
<div id="header" >
    <h3>{{ header }}</h3>
    <Button severity="danger" rounded icon="pi pi-trash" v-if="elementSelected" 
    @click="deleteFile() ; elementSelected = $event.target"
    :loading="deleteLoading"></Button>
</div>
    <ul>
        <li v-for="file in files" v-bind:key="file.id" 
            class="file-item" 
            :class="{selected : file === fileSelected && elementSelected}">
            <span @click="selectFile($event); fileSelected = file" style="flex: 1;">{{ file.document.fileName }}</span>
            <Button  icon="pi pi-download" :loading="loading" @click="downloadFile(file)" class="download-bttn"></Button>
        </li>
    </ul>    
<div id="footer">
    <input type="file" class="fileInput" @change="newFileSelected">
    <Button label="Upload" icon="pi pi-upload" 
    style="max-width: fit-content;" @click="uploadFile"
    v-if="selectedNewFile"
    :loading="uploadLoading"
    ></Button>
</div>

</div>
</template>

<style scoped>

.container{
    padding-inline: 1rem;
    display: flex;
    flex-direction: column;
    align-items: center;
    border-radius: 10px;
    box-shadow: 0 2px 2px 0 rgba(28, 25, 25, 0.2);
    height: fit-content;
    

}
span{
    font-size: small;
    text-wrap: nowrap;
    overflow: hidden;
}

ul{
    list-style-type: none;
    padding: 2px;
    display: flex;
    flex-direction: column;
    gap: 10px;
    min-height: 50px;
    overflow-y: auto;
    max-height: 350px;
    width: 100%;
    background-color: var(--p-surface-50);
    border-radius: 10px;
}
.download-bttn{
    max-width: 30px;
    position: relative;
    left: 1px;
    max-height: 95%;
    margin: 2px;
}

li{
    text-decoration: none;
    display: flex;
    gap: 1rem;
    align-items: center;
    box-shadow: 0 2px 2px 0 rgba(28, 25, 25, 0.6);
    border: solid black 1px;
    justify-content: space-between;
    padding-left: 5px;
    border-radius: 7px;
    max-height: 35px;
    max-width: 250px;
}
#header{
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-block: 10px;
    min-height: 40px;
    max-height: 45px;
    flex: 1;
    width: 100%;
}
.selected{
    background-color: var(--p-surface-300);
}

h3{
    font-weight: bold;
}
#footer{
    margin-block: 10px;
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 10px;
}
</style>
