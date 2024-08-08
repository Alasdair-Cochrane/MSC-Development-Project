<script setup>

const props = defineProps({items:[]})
</script>
<template>
    <div class="container">
        <div>
            <DataTable :value="items">
                <Column field="serialNumber" header="Serial Number" sortable></Column>
                <Column field="model.modelName" header="Model Name" sortable></Column>
                <Column field="model.category" header="Category" sortable></Column>
                <Column field="unit.name" header="Owner" sortable></Column>
            </DataTable>
        </div>
    </div>
</template>
<style scoped>
.container{
    display: flex;
    flex-direction: column;
    align-items: center;
}

</style>