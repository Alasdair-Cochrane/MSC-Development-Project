<script setup>
import {onMounted, ref} from 'vue'
import { getSearchFields } from '@/Models/Item';
import { QueryItems } from '@/Services/ItemService';

const searchValue = ref()
const searchProperty = ref("Serial Number")
const searchFields = getSearchFields()
const searchTerms = ref([""])
onMounted(() => searchTerms.value = Object.keys(searchFields))

function search(){

    let queryProperty = searchFields[searchProperty.value];
    if(queryProperty.startsWith("Model"))
    {
        console.log("Model Search")
        QueryItems(queryProperty, searchValue.value)
    }
    else{
        console.log("Item Search")
        QueryItems(queryProperty, searchValue.value)
    }
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
        <h1>entries</h1>
    </div>
</div>
</template>
<style scoped>
.container{
    display: flex;
    flex: 1;
    flex-direction: column;
    height: auto;
    border: 2px red solid;
    padding: 1rem;
}
.search-input{
    display:flex;
    flex: 1;
    flex-wrap: wrap;
    gap: 10px;
    #scan{
        flex: 1;
    }
    #search{
        min-width: 100%;
    }
    .text-input{
        display:flex;
        flex-direction: column;
        gap: 5px;
    }
}
.entries{
        display: flex;
        flex-direction: column;
        flex: 1;
        height: 100%;
    }
</style>