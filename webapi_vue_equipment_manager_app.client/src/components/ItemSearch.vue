<script setup>
import {onMounted, ref} from 'vue'
import {  getSearchFields } from '@/Models/Item';
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

const props = defineProps({showScannerFirst:{type:Boolean, default:false}, showBarcodeScannerFirst:{type:Boolean, default:false}})


onMounted(() => {
    searchTerms.value = Object.keys(searchFields);
    showScanner.value = props.showScannerFirst
    showBarcodeScanner.value = props.showBarcodeScannerFirst
    console.log("params: " + props.showScannerFirst)

})

async function search(){

    let queryProperty = searchFields[searchProperty.value];
    searchResults.value = await QueryItems(queryProperty, searchValue.value)
    emit('itemSearched', searchResults.value[0])
}

async function SearchBarcode(code){
    showBarcodeScanner.value = false;
    searchProperty.value = "Barcode"
    searchValue.value = code
    let queryProperty = {Barcode : searchValue.value}
    searchResults.value = await searchItemsByProperties(queryProperty)
    emit('itemSearched', searchResults.value[0])

}

async function scanImage(item){
    showScanner.value = false;
    let queryProperties = {
        SerialNumber : item.serialNumber,
        ModelNumber : item.model.modelNumber,
        ModelName : item.model.modelName,
    }
    let result = await searchItemsByProperties(queryProperties)
    searchResults.value = result
    emit('itemSearched', searchResults.value[0])
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
        <Button id="search" label="Search" @click="search"></Button>
    </div>
    <div class="entries">
        <ul v-for="i in searchResults" :key="i.Id">
           <ItemCard :item="i" :show-buttons="false" :clickable="true" 
           @clicked="$emit('itemSelected',i);beenClicked = i" :class="{clicked : beenClicked === i}"></ItemCard>
        </ul>
    <div v-if="!searchResults.length">
        <label>No Results Found</label>
    </div>
    </div>
</div>
<Dialog v-model:visible="showScanner" modal header="Scan Label" :closable=false :style="{ width: '400px' }" :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
        <InterpretImage @cancelled="showScanner = false" @confirmed="scanImage"></InterpretImage>
    </Dialog>
<Dialog v-model:visible="showBarcodeScanner" modal header="Scan Barcode"><BarcodeScanner @cancelled="showBarcodeScanner=false" @confirmed="SearchBarcode"></BarcodeScanner></Dialog>
</template>
<style scoped>
.container{
    display: flex;
    flex: 1;
    flex-direction: column;
    height: auto;
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
    background-color: var(--p-surface-100);
}
.clicked{
    background-color:var(--p-surface-300);
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

</style>