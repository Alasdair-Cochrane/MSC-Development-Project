<script>
import AddItem from '@/components/AddItem.vue';
export default{
    setup(){

        return {
            AddItem,
        }
    }

}
</script>

<template>
<div class="grid-nogutter page">
    <div class="col-12 sm:col-8">
        <AddItem></AddItem>
    </div>
    <div class="col-12 sm:col-4 right">
    <div class="added-list">
        <div v-for="n in 1" class="item-content">
            <label>serial</label>
            <label>serial</label>
            <label>serial</label>
            <Button icon="pi pi-pencil"/>
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
        justify-content: space-between;
        background-color: var(--p-surface-50);
        padding-left: 0.2rem;
        align-items: center
    }
    .added-list{
        display: flex;
        flex-direction: column;
        gap: 5px;
        overflow-y: auto;
        max-height: 90vh;
    }


</style>
