<script setup>
import ItemDetail from '@/components/ItemDetail.vue';
import ItemSearch from '@/components/ItemSearch.vue'
import MaintenanceDisplay from '@/components/MaintenanceDisplay.vue';
import NotesDisplay from '@/components/NotesDisplay.vue';


import {onMounted, ref} from 'vue'

const editMode = ref(false)
const item = ref()
const searchExpanded = ref(true)
const searchResults = ref([])
const props = defineProps({scanImage:{
    type:Boolean,
    default:false
    } ,
    scanBarcode:{
        type:Boolean,
        default:false,
    }  })


function toggleExtra() {editMode.value = !editMode.value
}

function itemsSearched(){
    if(searchResults.value.length)
    {
    item.value = searchResults.value[0]
    }
}


function itemSelected(selectedItem){
    item.value = selectedItem
}




function itemDeleted(){
    searchResults.value = searchResults.value.filter(x => x !== item.value )
    if(searchResults.value.length){
        item.value = searchResults.value[0]
    }
    else{
        item.value = null
    }
}



</script>
<template>
<div class="page">

    <div class="search">
        <Button icon="pi pi-chevron-left" @click="searchExpanded = false" severity="secondary" outlined="" v-show="searchExpanded && item"></Button>
        <Button icon="pi pi-search" @click="searchExpanded = true"   v-show="!searchExpanded && item"></Button>


        <div v-show="searchExpanded" class="search-display">
            <ItemSearch @item-searched="itemsSearched()" @item-selected="itemSelected" v-model="searchResults" :show-barcode-scanner-first="scanBarcode" :show-scanner-first="scanImage"></ItemSearch></div>
    </div>
    <div class="details" >
        <div class="display">
            <ItemDetail @editTrue="toggleExtra()" :selected-Item="item" v-if="item" @update="(i) => {item=i; toggleExtra()}" @deleted="itemDeleted()"></ItemDetail>
        </div>
        
    </div>
    
</div>
</template>

<style scoped>

.page{
    display: flex;
    flex-wrap: wrap;
    flex: 1;
    padding: 1rem;
    gap: 10px;
    background-color: var(--p-surface-50);
    min-height: 100vh;
    height: 100%;
}
@media(max-width:430px){
        .page{
            justify-content: center;
        }
        .search{
            width: 100%;
            min-width: 350px;
        }
}
.search{
    max-width: 320px;
    display: flex;
    flex-direction: column;
    gap: 10px;
}
.panel{
    border-radius: 10px;
    background-color: white;
    border: solid black 1px;
    box-shadow:  0 2px 2px 0 rgba(28, 25, 25, 0.2);    
    height: fit-content;
    max-width: 350px;
}

.search .p-button{
    max-height: 25px; 
    width: 100%;
    min-width: 40px;
}
.search-display{
    height: auto;
}
.details{
    display: flex;
    justify-content: space-around;
    flex: 1;
}
.display{
    flex: 1;
}
.extra{
    display: flex;
    flex-wrap: wrap;
    flex-direction: column;
    gap: 1rem;

}

</style>