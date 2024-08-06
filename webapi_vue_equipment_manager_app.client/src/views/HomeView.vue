<script setup>
import { RouterLink } from 'vue-router';
import {ref, onMounted} from 'vue'
import { IsMobile } from '@/Services/DeviceService';
import ExportItems from '@/components/ExportItems.vue';
import { GetFromAPI, store } from '@/Store/Store';
import { QueryItems } from '@/Services/ItemService';
import { FormatDate } from '@/Services/FormatService';
import NotesActivityDisplay from '@/components/NotesActivityDisplay.vue';
import MaintenanceActivityDisplay from '@/components/MaintenanceActivityDisplay.vue';


const dataLoading = ref(true)
const chartData = ref();
const chartOptions = ref();
const mobileScreen = ref(true);
const showExport = ref(false);


onMounted(async () => {
    mobileScreen.value = !IsMobile();     
    dataLoading.value = false;
});


const showStatusQuantityItems= async (data) => {
    let result = await QueryItems("statusCategoryId", data.statusId)
    console.log(result)
}

</script>

<template>
<div class="page">
        <!-- Left Column  -->      
        <div class="btn-group ">
            <div class="btn-row">
                <div class="btn-container">
                    <RouterLink to="/add">
                    <div class="btn-box">
                        <label class="btn-label">Add Item</label> 
                        <i class="pi pi-plus"/>            
                    </div>
                </RouterLink>
                </div>
                <div class="btn-container">
                    <RouterLink to="/search">
                    <div class="btn-box">
                        <label class="btn-label">Search</label> 
                        <i class="pi pi-search"/>            
                    </div>
                    </RouterLink>
                </div>
            </div>

            <div class="btn-row">
                <div class="btn-container">
                    <RouterLink to="/manage">
                    <div class="btn-box">
                        <label class="btn-label">Manage</label> 
                        <i class="pi pi-table"/>            
                    </div>
                    </RouterLink>
                </div>
                <div class="btn-container">
                    <RouterLink to="search/scanImage">
                    <div class="btn-box">
                        <label class="btn-label">Scan Image</label> 
                        <i class="pi pi-camera"/>            
                    </div>
                    </RouterLink>
                </div>                
            </div>

            <div class="btn-row">
                <div class="btn-container">
                    <div class="btn-box" @click="showExport= true">
                        <label class="btn-label">Export</label> 
                        <i class="pi pi-file-excel" />            
                    </div>
                </div>
                <div class="btn-container">
                    <RouterLink to="/units">
                    <div class="btn-box">
                        <label class="btn-label">Units</label> 
                        <i class="pi pi-building"/>            
                    </div>
                    </RouterLink>
                </div>
            </div>
 </div>
    
    <!-- Right Column -->
            <div class="data "> 
                    <div class="data-table">
                    <DataTable class="" :value="store.StatusQuantities" size="small" v-if="!dataLoading" style="width: 300px;">
                        <Column field="statusName" header="Status" style="width: 150px;"></Column>
                        <Column field="itemQuantity" header="#" style="width: 80px;"></Column>
                        <Column style="width: 50px;">
                            <template #body="{data}">
                            <div class="btn role-btn">
                                <Button rounded outlined icon="pi pi-search" @click="showStatusQuantityItems(data)" style="width: 25px; height: 25px;"></Button>
                            </div>
                        </template></Column>

                    </DataTable>
                    <div v-else>
                        <Skeleton></Skeleton>
                        <Skeleton></Skeleton>
                        <Skeleton></Skeleton>
                        <Skeleton></Skeleton>
                        <Skeleton></Skeleton>
                    </div>
                </div>
                <div class="data-table">
                    <NotesActivityDisplay></NotesActivityDisplay>                    
                </div>
                <div class="data-table">
                    <MaintenanceActivityDisplay></MaintenanceActivityDisplay>
                </div>
            </div>
   
   <div>
    <Dialog v-model:visible="showExport">
        <ExportItems></ExportItems>
    </Dialog>
   </div>
</div>
</template>

<style scoped>

.page {
    flex: 1;
    display: flex;
    flex-wrap: wrap;
    height: 100%;
    background-color: var(--p-surface-50);
}
a{
    text-decoration: none;
    color: black;
}

.data{
    flex: 1;
    display: flex;
    flex-wrap: wrap;
    gap: 1rem;
    padding: 1rem;
}

.data-table{
    box-shadow:  0 2px 2px 0 rgba(28, 25, 25, 0.2);
    width: fit-content;
    height: fit-content;
    border-radius: 10px;
    padding: 10px;
    min-height: 300px;
    border: black solid 1px;
    background-color: white;
}

.btn-container{
    display: block;
    width: fit-content;
    justify-content: center;
    align-items: center;    
}

.btn-box{
    border: solid black 2px;
    text-decoration: none;  
    display: flex; 
    flex-direction: column;
    flex: 1;
    min-width: 100px;
    max-width: 100px;
    aspect-ratio: 1/1;
    align-items: center;
    border-radius: 10px;
    box-shadow:  0 2px 2px 0 rgba(28, 25, 25, 0.2);
    background-color: white;
}

.btn-box i{
    z-index: 98;
    justify-self: center;
    align-self: center;
    align-content: center;
    font-size: 4rem;
    color: black;
}

.btn-container :hover{
    background-color: var(--p-primary-400);
    color: white;
}
.btn-container :hover i{
    color: white;
}

label{
    font-weight: bold;
}

.btn-group{
        display: flex;
        height: fit-content;  
        flex-direction: column;
        padding: 1rem;
        gap: 1rem;
    }

.btn-row{
    display: flex;
    justify-content: center;
    gap: 2rem;
    align-items: center;
    flex: 1;
}

</style>