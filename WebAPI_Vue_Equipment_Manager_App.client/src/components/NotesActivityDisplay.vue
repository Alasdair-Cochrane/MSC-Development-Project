<script setup>
import { GetFromAPI } from '@/Store/Store';
import { onMounted, ref } from 'vue';
import { FormatDate } from '@/Services/FormatService';

const notes = ref([])
const daysBefore = ref(0)
const expandedNotesRows = ref([])
const daysOptionsLabel = (["Today","1 week", "2 weeks", "1 month"])
const daysOptions=([0,7,14,31])
const selectedOption = ref("Today")
const loading = ref(true)
onMounted(async () => getNotes())

const getNotes = async () => {
    if(selectedOption.value){
        let i =  daysOptionsLabel.indexOf(selectedOption.value)
        daysBefore.value = daysOptions[i]
    }
    notes.value = await GetFromAPI(`api/items/notes?daysBeforeNow=${daysBefore.value}`,"Could not retrieve recent notes")
    loading.value = false
}



</script>
<template>
<div class="wrapper">
    <div class="header">
        <h3>Recent Notes</h3>
        <Select v-model="selectedOption" :options="daysOptionsLabel" @change="getNotes()"></Select>
    </div>
                    <DataTable class="" :value="notes" size="small" 
                    :expanded-rows="expandedNotesRows" scrollable scroll-height="300px" 
                    :loading="loading">
                        <Column expander></Column>
                        <Column field="title" header="Title" style="width: 150px;"></Column>
                        <Column field="serialNumber" header="Item s/n" style="width: 80px;"></Column>
                        <Column field="userName" header="User" ></Column>
                        <Column field="datePosted" header="Date" >
                    <template #body="{data}">
                            {{ FormatDate(data.datePosted) }}
                    </template>
                    </Column>
                    <template #expansion="slotProps">
                        <div class="text-display">
                            <p>{{ slotProps.data.text }}</p>
                        </div>      
                    </template>          
                    </DataTable>
</div>
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