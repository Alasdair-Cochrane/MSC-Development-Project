<script setup>
import {onMounted, ref} from 'vue'
import { QueryItems, searchItemsByProperties } from '@/Services/ItemService';
import ItemCard from './ItemCard.vue';

const searchValue = ref("")
const searchProperty = ref("Serial Number")
const searchFields = getSearchFields()
const searchTerms = ref([""])
const searchResults = defineModel()
const showScanner = ref(false)
const showBarcodeScanner = ref(false)
const emit = defineEmits(['itemSearched', 'itemSelected'])
const beenClicked = ref()
const searchLoading = ref(false)
const searched = ref(false)

const props = defineProps({showScannerFirst:{type:Boolean, default:false}, showBarcodeScannerFirst:{type:Boolean, default:false}})


onMounted(() => {
    searchTerms.value = Object.keys(searchFields);
    showScanner.value = props.showScannerFirst
    showBarcodeScanner.value = props.showBarcodeScannerFirst

})

function getSearchFields(){
    return {"Serial Number" : "SerialNumber" , 
         "Local Name" :"LocalName" , 
         "Barcode" : "Barcode", 
         "Owner" : "UnitName",
         "Model Name" : "ModelName", 
         "Model Number" : "ModelNumber",
         "Manufacturer" : "Manufacturer",
         "Category" : "Category" }
}


async function search(){
    //if the user has not input a value to search just return
    if(!searchValue.value){
        searched.value = true;
        searchResults.value = []
        return;
    }
    searched.value = false
    searchLoading.value = true

    let queryProperty = searchFields[searchProperty.value];
    searchResults.value = await QueryItems(queryProperty, searchValue.value)
    emit('itemSearched')
    searchLoading.value = false
    searched.value = true

}

async function SearchBarcode(code){
    searchLoading.value = true;
    searched.value = false
    //close scanner
    showBarcodeScanner.value = false;
    //change search select to barcode & assign value in input  
    searchProperty.value = "Barcode"
    searchValue.value = code
    //send query to api
    let queryProperty = {Barcode : searchValue.value}
    searchResults.value = await searchItemsByProperties(queryProperty)
    emit('itemSearched')

    searched.value = true;
    searchLoading.value = false;
}

async function scanImage(item){
    searchLoading.value = true;
    searched.value = false
    showScanner.value = false;
    let queryProperties = {
        SerialNumber : item.serialNumber,
        ModelNumber : item.model.modelNumber,
        ModelName : item.model.modelName,
    }
    searchResults.value = await searchItemsByProperties(queryProperties)    
    emit('itemSearched')
    searchLoading.value = false;
    searched.value = true;
}

</script>
<template>
<div class="container">
    <div class="search-input">
        <div class="text-input">
        <InputText type="text" placeholder="Search" v-model="searchValue"></InputText>
        <Select :options="searchTerms" v-model="searchProperty"></Select>
        </div>
        <div class="btns-scan">
        <Button id="scan" icon="pi pi-barcode" @click="showBarcodeScanner = true"></Button>
        <Button id="imageScan" icon="pi pi-camera" @click="showScanner = true"></Button>
        </div>
        <Button id="search" label="Search" @click="search" :loading="searchLoading"></Button>
    </div>
    <div class="entries">
        <ul v-for="i in searchResults" :key="i.Id">
           <ItemCard :item="i" :show-buttons="false" :clickable="true" 
           @clicked="$emit('itemSelected',i);beenClicked = i" :class="{clicked : beenClicked === i}"></ItemCard>
        </ul>
    <div v-if="!searchResults.length && searched" class="no-results">
        <label>No Results Found</label>
    </div>
    </div>
</div>
<Dialog v-model:visible="showScanner" modal header="Scan Label" :closable=false :style="{ width: '400px' }" :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
        <InterpretImage @cancelled="showScanner = false" @confirmed="scanImage"></InterpretImage>
    </Dialog>
<Dialog v-model:visible="showBarcodeScanner" modal header="Scan Barcode" :closable=false>
    <BarcodeScanner @cancelled="showBarcodeScanner=false" @confirmed="SearchBarcode"></BarcodeScanner></Dialog>
</template>
<style scoped>
.container{
    display: flex;
    flex: 1;
    flex-direction: column;
    height: fit-content;
    max-width: 350px;
}
.search-input{
    display:flex;
    flex: 1;
    flex-wrap: wrap;
    gap: 10px;    
}

#scan{
        flex: 1;
    }
#search{
    min-width: 100%;
}
.search-input .text-input{
    display:flex;
        flex-direction: column;
        gap: 5px;
}

ul{
    list-style-type: none;
    width: 100%;
    padding: 0;

}

ul :hover{
    background-color: var(--p-surface-200);
}
.clicked{
    background-color:var(--p-surface-200);
    border-radius: 10px;
}


.entries{
        display: flex;
        flex-direction: column;
        flex: 1;
        max-height: 70vh;
        padding: 0.5rem;
        width: 100%;
        gap: 5px;
        overflow-y: auto;
    }

.btns-scan{
    display: flex;
    flex: 1;
    flex-direction:  column;
    gap: 0.2rem;
}
.btns-scan button{
    flex: 1;
    width: 100%;
}
.no-results{
    display: flex;
    justify-content: center;
    background-color: white;
    border-radius: 10px;
}

</style>