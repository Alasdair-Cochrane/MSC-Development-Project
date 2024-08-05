<script setup>
import { GetFromAPI } from '@/Store/Store';
import { onMounted, ref } from 'vue';
import { FormatDate } from '@/Services/FormatService';

const notes = ref([])
const daysBefore = ref(7)
const expandedNotesRows = ref([])
const daysOptionsLabel = (["1 day","1 week", "2 weeks", "1 month"])
const daysOptions=([1,7,14,31])
const selectedOption = ref("1 week")
onMounted(async () => getNotes())

const getNotes = async () => {
    if(selectedOption.value){
        let i =  daysOptionsLabel.indexOf(selectedOption.value)
        daysBefore.value = daysOptions[i]
    }
    notes.value = await GetFromAPI(`api/items/notes?daysBeforeNow=${daysBefore.value}`,"Could not retrieve recent notes")
}



</script>
<template>
<div class="wrapper">
    <div class="header">
        <h3>Recent Notes</h3>
        <Select v-model="selectedOption" :options="daysOptionsLabel" @change="getNotes()"></Select>
    </div>
                    <DataTable class="" :value="notes" size="small" :expanded-rows="expandedNotesRows" scrollable scroll-height="300px" >
                        <Column expander></Column>
                        <Column field="title" header="Title" style="width: 150px;"></Column>
                        <Column field="userName" header="User" ></Column>
                        <Column field="serialNumber" header="Item s/n" style="width: 80px;"></Column>
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