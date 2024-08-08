<script setup>
import { getAccessToken } from '@/Services/UserService';
import { store } from '@/Store/Store';
import { ref } from 'vue';
const selectedUnits = ref()

const getUrl= () =>{

    if(selectedUnits.value){
        let ids = selectedUnits.value.map((x) => `unitIds=${x.id}&`)
        let url="api/items/csv?"
        ids.forEach(element => {
            url += element
        });
        return url
    }
    return "api/items/csv"
}

//https://dev.to/fercarballo/descargar-archivo-pdf-en-una-aplicacion-web-con-vuejs-y-javascript-2glh
const getExport = async ()=> {
    let url = getUrl()
    let response = await fetch(url,{
        method: "GET",
        headers:{
            "Authorization": await getAccessToken()
        }
    })
    if(!response.ok){
        console.warn(response.statusText)
        console.warn(await response.json())
    }
   let file = await response.blob()
   let fileUrl = URL.createObjectURL(file)

    const a = document.createElement('a');
    a.href = fileUrl
    a.download = "Equipment.csv"
    a.target = "_blank"
    document.body.appendChild(a)
    a.click()
    document.body.removeChild(a)
}




</script>
<template>

<div class="container">
    <Button label="Export All" icon="pi pi-download" @click="getExport" v-show="!selectedUnits || !selectedUnits.length > 0"></Button>

    <FloatLabel style="max-width: 250px;">    
        <MultiSelect v-model="selectedUnits" 
        :options="store.Units" 
        optionLabel="name" 
        display="chip"
        filter
        style="min-width: 150px;"
        variant="filled"
        :maxSelectedLabels=3
        ></MultiSelect>
        <label for="selectUnit">Select Units</label>
    </FloatLabel>
    <DataTable :value="selectedUnits" 
    scrollable 
    scrollHeight="300px" 
    v-show="selectedUnits && selectedUnits.length > 0"
    style="min-width: 200px;">
        <Column field="name"></Column>
    </DataTable>
    <Button label="Export" icon="pi pi-download" @click="getExport" v-show="selectedUnits && selectedUnits.length > 0"></Button>

</div>
</template>
<style scoped>
.container{
    min-width: 300px;
    min-height: 300px;
    max-width: 400px;
    display: flex;
    flex-direction: column;
    align-content: center;
    align-items: center;
    flex: 1;
    gap: 1rem;
}
.p-floatlabel{
    width: fit-content;
    max-width: 250px;
    margin-top: 0.5rem;
}
.p-multiselect{
    max-width: 250px;
}
</style>