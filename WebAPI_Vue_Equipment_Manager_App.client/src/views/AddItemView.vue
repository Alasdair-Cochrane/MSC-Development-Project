<script setup>
import AddItem from '@/components/AddItem.vue';
import ItemCard from '@/components/ItemCard.vue';
import {ref} from 'vue'
import { store } from '@/Store/Store';
import { RouterLink } from 'vue-router';

const addedItemsList = ref([])
const deleteItem = (item) =>{
    addedItemsList.value = addedItemsList.value.filter(x => x!== item)
}
</script>

<template>
<div class="grid-nogutter page" v-if="store.UnitsAuthorised.length">
    <div class="col-12 sm:col-9">
        <AddItem @itemSaved="(i) => addedItemsList.push(i)"></AddItem>
    </div>
    <div class="col-12 sm:col-3 right">
    <div class="added-list">
        <div v-for="i in addedItemsList.slice().reverse()" :key="i.id">
            <ItemCard :item=i @deleted="deleteItem(i)"></ItemCard>
        </div>
        
    </div>

    </div>


</div>
<div v-else class="no-access">
    <div class="panel">
    <p>You must be an admin or private user of at least one location in order to add an item.<br>
    Request access from an administrator or create a new location <RouterLink to="/units"><strong>HERE</strong></RouterLink> </p>
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
        height: 100%;
        background-color: var(--p-surface-50);
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
    .no-access{
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100%;
    }
.panel{
    box-shadow:  0 2px 2px 0 rgba(28, 25, 25, 0.2);    
    padding: 2rem;
    background-color: red;
    color: white;
    border-radius: 10px;
}
.p{
    font-weight: bold;
}


</style>
