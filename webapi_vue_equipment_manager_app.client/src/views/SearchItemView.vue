<script setup>
import ItemDetail from '@/components/ItemDetail.vue';
import ItemSearch from '@/components/ItemSearch.vue'
import MaintenanceDisplay from '@/components/MaintenanceDisplay.vue';
import NotesDisplay from '@/components/NotesDisplay.vue';


import {ref} from 'vue'

const editMode = ref(false)
const item = ref()
const uploadLoading = ref(false)
const searchExpanded = ref(true)
const searchResults = ref([])


function toggleExtra() {editMode.value = !editMode.value
}

function viewItem(newItem){
    if(newItem)
    {
    item.value = newItem
    }
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


        <div v-show="searchExpanded">
            <ItemSearch @item-searched="viewItem" @item-selected="viewItem" v-model="searchResults"></ItemSearch></div>
    </div>
    <div class="details">
        <div class="display">
            <ItemDetail @editTrue="toggleExtra()" :selected-Item="item" v-if="item" @update="(i) => item=i" @deleted="itemDeleted()"></ItemDetail>
        </div>
        
    </div>
    <div v-if="!editMode" class="extra">
            <MaintenanceDisplay v-model="item" v-if="item"></MaintenanceDisplay>
            <NotesDisplay v-model="item" v-if="item"></NotesDisplay>
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
}
.search{
    max-width: 300px;
    display: flex;
    flex-direction: column;
    gap: 10px;

}
.search .p-button{
    max-height: 25px; 
    width: 100%;
    min-width: 40px;
}
.details{
    display: flex;
    justify-content: space-around;
    flex: 1;
}
.display{

}
.extra{


}

</style>