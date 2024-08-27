<script setup>
import { getAccessToken } from '@/Services/UserService';
import { store } from '@/Store/Store';
import { ref } from 'vue';

const newMaintenance = ref({})
const item = defineModel()
const loading = ref(false)
const maxDate = ref(new Date())
const emit = defineEmits(['confirmed'])

async function saveMaintenance(){
    loading.value = true
    newMaintenance.value.itemId = item.value.id
    let entry = JSON.stringify(newMaintenance.value)
    try{
        let response = await fetch('api/items/maintenance',{
            method: "POST",
            headers:{
                "Authorization" : await getAccessToken(),
                "Content-Type" : "application/json",

            },
            body: entry
        })
        if(response.ok){
            let newEntry = await response.json()
            item.value.maintenances.push(newEntry);
            emit('confirmed')
        }
        else{
            console.warn(await response.json())
        }
    }
    catch(e){
        console.warn(e)
    }
    loading.value = false
}

</script>
<template>
<div class="wrapper">
    <div>
    <div class="field">
        <label for="mCategory">Type</label>
        <Select id="mCategory" :options="store.MaintenanceCategories" v-model="newMaintenance.categoryName" style="max-width: 200px;"></Select>
    </div>
    <div class="field">
        <label for="mProvider">Provider</label>
        <InputText v-model="newMaintenance.provider_Name"></InputText>
    </div>
    <div class="field">
        <label for="mDate">Date Completed</label>
        <DatePicker v-model="newMaintenance.date_Completed" style="max-width: 200px;" :maxDate="maxDate"></DatePicker>
    </div>
    <div class="field">
        <label for="mProvider">Description</label>
        <Textarea v-model="newMaintenance.description" rows="5" cols="30"></Textarea>
    </div>
    </div>
    <div>
    <Button label="Submit" icon="pi pi-save" @click="saveMaintenance" :loading="loading"></Button>
    </div>
</div>
</template>
<style scoped>
.field{
    display: flex;
    flex-direction: column
}
label{
    font-weight: bold;
}
.p-inputtext{
    max-width: 200px;
}
input .p-datepicker-input{
    max-width: 200px;
}
</style>