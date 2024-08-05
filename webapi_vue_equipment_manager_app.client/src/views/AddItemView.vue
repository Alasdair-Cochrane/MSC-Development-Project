<script setup>
import AddItem from '@/components/AddItem.vue';
import ItemCard from '@/components/ItemCard.vue';
import {ref} from 'vue'

const addedItemsList = ref([])
const deleteItem = (item) =>{
    addedItemsList.value = addedItemsList.value.filter(x => x!== item)
}
</script>

<template>
<div class="grid-nogutter page">
    <div class="col-12 sm:col-9">
        <AddItem @itemSaved="(i) => addedItemsList.push(i)"></AddItem>
    </div>
    <div class="col-12 sm:col-3 right">
    <div class="added-list">
        <div v-for="i in addedItemsList" :key="i.id">
            <ItemCard :item=i @deleted="deleteItem(i)"></ItemCard>
        </div>
        
    </div>

    </div>


</div>
</template>
<style scoped>
    div{
    }
    .page{
        flex:1;
        display: flex;
        flex-wrap: wrap;
        min-height: 100vh;
    }
    .right{
        flex: 1;
        width: 100%;
    }

    .added-list{
        display: flex;
        flex-direction: column;
        gap: 5px;
        overflow-y: auto;
        max-height: 90vh;
        flex: 1;
        padding: 10px;
    }


</style>
