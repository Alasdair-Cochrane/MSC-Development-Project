<script setup>
import { onMounted, ref, watch } from 'vue';
import { FormatDate } from '@/Services/FormatService';

const maintenances = ref([])
const item = defineModel()
const showView = ref(false)
const showAdd = ref(false)
const selectedMaintenance = ref()

const displayDetails = (data) => {
    showView.value = true
    selectedMaintenance.value = data
}
onMounted(() => maintenances.value = item.value.maintenances)

watch(item, () => maintenances.value = item.value.maintenances)
watch(maintenances, () => {item.value.maintenances = maintenances.value
})

</script>
<template>
    <div class="container">
        <div class="header">
            <h3>Maintenance Activities</h3>
            <Button icon="pi pi-plus" @click="showAdd = true"></Button>
        </div>
        <div class="list">
            <DataTable :value="maintenances" scrollable scroll-height="280px" style="width: 100%" >
                <Column field="categoryName" header="Type" style="width: 120px;" sortable></Column>
                 <Column field="date_Completed" header="Date" style="width: 80px;" sortable>
                    <template #body="{data}">
                            {{ FormatDate(data.date_Completed) }}
                    </template>
                </Column>
                 <Column>
                    <template #body="{data}">
                        <div class="btn role-btn">
                    <Button rounded icon="pi pi-search" style="width: 28px; height: 28px;" @click="displayDetails(data)"></Button>
                        </div>
                    </template>
                </Column>
            </DataTable>
        </div>

    </div>
    <Dialog v-model:visible="showView"><MaintenanceDetails v-model:list="maintenances" :item="item" v-model:maintenance="selectedMaintenance" @delete="showView = false"></MaintenanceDetails></Dialog>
    <Dialog v-model:visible="showAdd"><AddMaintenance v-model="item"></AddMaintenance></Dialog>

</template>
<style scoped>
.container{
    min-width: 300px;
    display: flex;
    flex: 1;
    height: auto;
    flex-direction: column;
    box-shadow:  0 2px 2px 0 rgba(28, 25, 25, 0.4);
    border-radius: 10px;
    max-height: 450px;
    max-width: 360px;
}
h3{
    width: fit-content;
    font-weight: bold;
}
.header{
    display: flex;
    justify-content: space-between;
    padding: 10px;
    align-items: center;
}
.placeholder{
    width: 100%;
    border: solid black 1px;
    height: 40px;
    display: flex;
    align-items: center;
    padding: 5px;
    border-radius: 5px;
}
.list{
    padding: 10px;
}
.maintenance-card{
    display: flex;
    align-items: center;
    justify-content: space-between;
    flex: 1;
    padding: 5px;
    border: 1px solid black;
    border-radius: 5px;
}
.maintenance-card div{
    display: grid;
    flex: 1;
    grid-template-columns: 1fr 1fr;
}
.value{
    max-height: 400px;
    max-width: 250px;
    overflow-y: auto;
}
</style>