<script setup>
import ItemDetail from '@/components/ItemDetail.vue';
import ItemSearch from '@/components/ItemSearch.vue'
import MaintenanceDisplay from '@/components/MaintenanceDisplay.vue';

import {ref} from 'vue'

const editMode = ref(false)
const item = ref()
const uploadLoading = ref(false)

function toggleExtra() {editMode.value = !editMode.value
}

function viewItem(newItem){
    if(newItem)
    {
    item.value = newItem
    }
}



</script>
<template>
<div class="page">
    <div class="search">
        <ItemSearch @item-searched="viewItem" @item-selected="viewItem"></ItemSearch>
    </div>
    <div class="details">
        <div class="display">
            <ItemDetail @editTrue="toggleExtra()" :selected-Item="item" v-if="item" @update="(i) => item=i"></ItemDetail>
        </div>
        <div v-if="!editMode" class="extra">
            <MaintenanceDisplay v-model="item" v-if="item"></MaintenanceDisplay>
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
    gap: 2rem;
}
.search{


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