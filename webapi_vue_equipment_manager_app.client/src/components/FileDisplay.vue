<script setup>
import { Download, Upload } from '@/Services/FileService';
import {ref} from 'vue'

defineProps({
    title: String,
    recordId: Number,
    files: [{
            name: String,
            url: String,
            }]
})

function download(url){
    Download(url)
}

const testItem = ref([{name:"filename", url:"url/url"},])

import { useToast } from "primevue/usetoast";
const toast = useToast();
const fileupload = ref();

const upload = () => {
    toast.add({ severity: 'info', summary: 'Success', detail: 'File Uploaded', life: 3000 });
    Upload(fileupload.value);
    fileupload.value = null;
};

</script>

<template>
<div class="wrapper">
    <label id="title">{{ title ?? "Documents" }}</label>
    <div class="list">
        <div v-for="file in files" :key="file.name"  class="file-item">
            <label>{{ file.name }}</label>
            <Button icon="pi pi-download" @click="download(file.url)"></Button>
        </div>
        <div v-for="file in testItem" :key="file.name"  class="file-item">
            <label>{{ file.name }}</label>
            <Button icon="pi pi-download" @click="download(file.url)"></Button>
        </div>
    </div>
    <div class="upload">
        <div class="card flex justify-center">
        <Toast />
        <FileUpload mode="basic" name="demo[]" url="/api/upload" accept=".pdf" :maxFileSize="1000000" @upload="upload" :auto="true" chooseLabel="Browse" />
    </div>
        
    </div>
    

</div>
</template>

<style scoped>
.wrapper{
    flex: 1;
    display: flex;
    flex-direction: column;
    max-height: 400px;
    height: 100%;
    background-color: var(--p-surface-100);
    border-radius: 25px;
    padding:10px 10px 5px 10px;
    width: 100%;
    min-width: 200px;
    max-width: 300px;
    align-items: center;
    gap:5px;
}

#title{
    width: 100%;
    display: block;
    text-align: center;
    font-weight: bolder;
    
}
.list{
    flex: 1;
    width: auto;
    overflow-y: auto;
    background-color: var(--p-surface-0);
    border-radius: 25px;
    padding: 5px;
    width: 100%;
}
.file-item{
    background-color: var(--p-surface-200);
    display: flex;
    justify-content: flex-end;
    gap: 5px;
    align-items: center;
    padding-left: 5px;
    border-radius: 15px;
    
}
.file-item Button{
}
.file-item label{
    font-size: smaller;
    text-align: end;

}

.upload{
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex: 0;
    padding-inline: 5px;
    gap: 5px;

}
.upload button{
    background-color: var(--p-surface-0);
    align-self: center;
}

</style>
