<script setup>
import AddItem from '@/components/AddItem.vue';
import ItemCard from '@/components/ItemCard.vue';
import {ref} from 'vue'

const addedItemsList = ref([])

</script>

<template>
<div class="grid-nogutter page">
    <div class="col-12 sm:col-8">
        <AddItem @itemSaved="(i) => addedItemsList.push(i)"></AddItem>
    </div>
    <div class="col-12 sm:col-4 right">
    <div class="added-list">
        <div v-for="i in addedItemsList" :key="i.id" class="item-content">
            <ItemCard :item=i ></ItemCard>
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
        padding: 1rem;
    }

    .item-content{
        display: flex;
    }
    .added-list{
        display: flex;
        flex-direction: column;
        gap: 5px;
        overflow-y: auto;
        max-height: 90vh;
    }


</style>
