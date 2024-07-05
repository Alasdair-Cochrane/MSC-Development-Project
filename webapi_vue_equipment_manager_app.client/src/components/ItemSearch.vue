<script setup>
import {onMounted, ref} from 'vue'
import { Item, getSearchFields } from '@/Models/Item';
import { QueryItems, GetItem } from '@/Services/ItemService';

const searchValue = ref("")
const searchProperty = ref("Serial Number")
const searchFields = getSearchFields()
const searchTerms = ref([""])
const searchResults = ref()
onMounted(() => searchTerms.value = Object.keys(searchFields))

async function search(){

    let queryProperty = searchFields[searchProperty.value];
    searchResults.value = await QueryItems(queryProperty, searchValue.value)

}
</script>
<template>
<div class="container">
    <div class="search-input">
        <div class="text-input">
        <InputText type="text" placeholder="Search" v-model="searchValue"></InputText>
        <Select :options="searchTerms" v-model="searchProperty"></Select>
        </div>
        <Button id="scan" icon="pi pi-barcode"></Button>
        <Button id="search" label="Search" @click="search"></Button>
    </div>
    <div class="entries">
        <ul v-for="i in searchResults" :key="i.Id">
           <div class="search-result">
               <label>{{i.serialNumber}}</label>
               <label>{{i.model.modelName}}</label>
               <label>{{i.model.modelNumber}}</label>
           </div>
        </ul>
    </div>
</div>
</template>
<style scoped>
.container{
    display: flex;
    flex: 1;
    flex-direction: column;
    height: auto;
    min-width: 320px;
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
}

.search-result{
    display: flex;
    justify-content: space-between;
}

.entries{
        display: flex;
        flex-direction: column;
        flex: 1;
        height: 100%;
        padding: 0.5rem;
    }
</style>