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
    <Dialog v-model:visible="showAdd"><AddMaintenance v-model="item" @confirmed="showAdd = false"></AddMaintenance></Dialog>

</template>
<style scoped>
.container{
    min-width: 280px;
    display: flex;
    flex: 1;
    height: auto;
    flex-direction: column;
    max-height: 450px;
}
h3{
    width: fit-content;
    font-weight: bold;
    font-size: medium;
}
.header{
    display: flex;
    justify-content: space-between;
    padding: 10px;
    align-items: center;
}
*{
    font-size: small;
}
.list{
    padding: 10px;
}

</style>