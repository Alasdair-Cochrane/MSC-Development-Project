<script setup>
import FileDisplay from '@/components/FileDisplay.vue';
import ItemDetail from '@/components/ItemDetail.vue';
import ItemSearch from '@/components/ItemSearch.vue'
import { EquipmentModel } from '@/Models/EquipmentModel';
import { Item } from '@/Models/Item';
import { UploadItemFile } from '@/Services/FileService';
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
    console.log(newItem.documents)
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
    max-width: 20%;
    min-width: 300px;


}

</style>