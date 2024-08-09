<script setup>
import { GetFromAPI } from '@/Store/Store';
import { onMounted, ref } from 'vue';
import { FormatDate } from '@/Services/FormatService';

const maintenances = ref([])
const daysBefore = ref(0)
const daysOptionsLabel = (["Today","1 week", "2 weeks", "1 month"])
const daysOptions=([0,7,14,31])
const selectedOption = ref("Today")
const selectedMaintenance = ref()
const loading = ref(true)

const showMaintenance = ref(false)

const displayMaintenance = (data) =>{
    selectedMaintenance.value = data
    showMaintenance.value = true
}

onMounted(async () => getNotes())

const getNotes = async () => {

    if(selectedOption.value){
        let i =  daysOptionsLabel.indexOf(selectedOption.value)
        daysBefore.value = daysOptions[i]
    }
    maintenances.value = await GetFromAPI(`api/items/maintenance?daysBeforeNow=${daysBefore.value}`,"Could not retrieve recent notes")
    loading.value = false
}



</script>
<template>
<div class="wrapper">
    <div class="header">
        <h3>Recent Maintenance</h3>
        <Select v-model="selectedOption" :options="daysOptionsLabel" @change="getNotes()" ></Select>
    </div>
                    <DataTable class="" :value="maintenances" size="small" scrollable scroll-height="300px" :loading="loading">
                        <Column field="categoryName" header="Type" style="width: 80px;"></Column>
                        <Column field="serialNumber" header="Item s/n" style="width: 80px;"></Column>
                        <Column field="date_Completed" header="Date" >
                    <template #body="{data}">
                            {{ FormatDate(data.date_Completed) }}
                    </template>
                    </Column>
                    <Column style="width: 50px;">
                        <template #body="{data}">
                        <div class="btn role-btn">
                            <Button rounded outlined icon="pi pi-search" @click="displayMaintenance(data)" style="width: 25px; height: 25px;"></Button>
                        </div>
                        </template>                        
                    </Column>
                    </DataTable>
</div>
<Dialog v-model:visible="showMaintenance">
    <MaintenanceDetails v-model:maintenance="selectedMaintenance" :showDelete="false">

    </MaintenanceDetails>
</Dialog>
</template>
<style scoped>
*{
    font-size: 12px;
}
.header{
    display: flex;
    justify-content: space-between;
}
h3{
    font-weight: bold;
    font-size: 16px;
}
</style>