<script setup>
import { GetFromAPI } from '@/Store/Store';
import { onMounted, ref } from 'vue';
import { FormatDate } from '@/Services/FormatService';

const items = ref([])
const daysBefore = ref(0)
const daysOptionsLabel = (["Today","1 week", "2 weeks", "1 month"])
const daysOptions=([0,7,14,31])
const selectedOption = ref("Today")
const selectedItem = ref()

const loading = ref(true)
const showItem = ref(false)

const displayItem = (data) =>{
    selectedItem.value = data
    showItem.value = true
}

onMounted(async () => await getNotes())

const getNotes = async () => {
    if(selectedOption.value){
        let i =  daysOptionsLabel.indexOf(selectedOption.value)
        daysBefore.value = daysOptions[i]
    }
    const duration = 5000
    const timeout = new Promise((resolve,reject)=> {
        setTimeout(()=>{
            resolve([]);
        }, duration)
    })
    let result = await Promise.race([GetFromAPI(`api/items/latest?daysBefore=${daysBefore.value}`,"Could not retrieve recent notes"),
    timeout])
    loading.value= false
    items.value = result;
}



</script>
<template>
<div class="wrapper">
    <div class="header">
        <h3>New Items {{ items.length ?? "" }}</h3>
        <Select v-model="selectedOption" :options="daysOptionsLabel" @change="getNotes()" :virtualScrollerOptions="{ itemSize: 50 }"></Select>
    </div>
                    <DataTable class="" :value="items" size="small" scrollable scroll-height="300px" :loading="loading">
                        <Column field="serialNumber" header="s/n" style="width: 80px;"></Column>
                        <Column field="model.category" header="Category" style="width: 80px;"></Column>
                        <Column field="unit.name" header="Unit" style="width: 80px;"></Column>
                        <Column field="dateCreated" header="Added" >
                    <template #body="{data}">
                            {{ FormatDate(data.dateCreated) }}
                    </template>
                    </Column>
                    
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