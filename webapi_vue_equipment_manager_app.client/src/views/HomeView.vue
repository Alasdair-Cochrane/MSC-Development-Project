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
    chartData.value = setChartData();
    chartOptions.value = setChartOptions();
    dataLoading.value = false;
});




//https://primevue.org/chart/
const setChartData = () => {
    const documentStyle = getComputedStyle(document.body);

    return {
        labels: store.StatusQuantities.map(x => x.statusName),
        datasets: [
            {
                data: store.StatusQuantities.map(x => x.itemQuantity),
                //backgroundColor: [documentStyle.getPropertyValue('--p-cyan-500'), documentStyle.getPropertyValue('--p-orange-500'), documentStyle.getPropertyValue('--p-gray-500')],
                //hoverBackgroundColor: [documentStyle.getPropertyValue('--p-cyan-400'), documentStyle.getPropertyValue('--p-orange-400'), documentStyle.getPropertyValue('--p-gray-400'), documentStyle.getPropertyValue('--p-gray-400')]
            }
        ]
    };
};

const setChartOptions = () => {

    return {
        plugins: {
            legend: {
                display: false
            }
        },
        responsive: true,
        maintainAspectRatio: false,
    };
};

const showStatusQuantityItems= async (data) => {
    let result = await QueryItems("statusCategoryId", data.statusId)
    console.log(result)
}

</script>

<template>
<div class="page grid-nogutter">
        <!-- Left Column  -->      
    <div class="col-12 md:col-4 c-main ">
        <!-- Top Buttons -->   

        <div class="btn-group col-12">
            <div class="btn-row">
                <div class="btn-container">
                    <div class="btn-box">
                        <label class="btn-label">Add Item</label> 
                        <i class="pi pi-plus"/>            
                    </div>
                </div>
                <div class="btn-container">
                    <div class="btn-box">
                        <label class="btn-label">Search</label> 
                        <i class="pi pi-search"/>            
                    </div>
                </div>
            </div>
            <div class="btn-row">
                <div class="btn-container">
                    <div class="btn-box">
                        <label class="btn-label">Manage</label> 
                        <i class="pi pi-table"/>            
                    </div>
                </div>
                <div class="btn-container">
                    <div class="btn-box">
                        <label class="btn-label">Scan</label> 
                        <i class="pi pi-camera"/>            
                    </div>
                </div>
                
            </div>
        </div>

         <!-- Bottom Buttons -->
        <div class="btn-group col-12">
            <div class="btn-row">
                <div class="btn-container">
                    <div class="btn-box" @click="showExport= true">
                        <label class="btn-label">Export</label> 
                        <i class="pi pi-file-excel" />            
                    </div>
                </div>
                <div class="btn-container">
                    <div class="btn-box">
                        <label class="btn-label">Units</label> 
                        <i class="pi pi-building"/>            
                    </div>
                </div>
            </div>
        </div>

 </div>
    
    <!-- Right Column -->
    <div class="col-12 md:col-8 c-main">
        <div class="right-panels">
            <div class="panel data grid-nogutter"> 
                    <div class="left col-12 md:col-6">
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
                    <div class="right col-12 md:col-6" v-if="mobileScreen">
                        <!-- <Chart type="doughnut" :data="chartData" :options="chartOptions"></Chart> -->
                        <!-- <Chart type="doughnut" :data="chartData" :options="chartOptions"></Chart> -->
                    </div>   
            </div>
            <div class="panel activity">
     
            </div>
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
}
.right-panels{
    flex: 1;
    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
    height: 100%;
    border-radius: 40%;
    margin-inline: 1rem;    
}

@media(max-width:768px){
    .right-panels{
            height: auto;
            margin-left: 2.5rem;
        }
    .right{
        flex-direction: row;
            max-height: 300px;
    }
    .p-chart{
    display: none
    }
}

.data-table{
    box-shadow:  0 2px 2px 0 rgba(28, 25, 25, 0.2);
    width: fit-content;
    border-radius: 10px;
    padding: 10px;
}


.panel{
    display: flex;
    flex-wrap: wrap;
    flex: 1;
    max-height: 50%;
    margin-block: 0.5rem;
}

.right{
    flex: 1;
        height: auto; 
        display: flex;
        flex-direction: column;
        justify-content: center; 
        gap: 1rem;  
}
.left{
        padding: 1rem;
        flex-direction: column;
        display: flex;
        justify-content: space-between;
} 

.p-chart{
        height: 40%;
    }

.c-main{
    height: auto;
    display: flex;
    flex-wrap: wrap;
    align-content: flex-start;
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
    min-width: 96px;
    max-width: 96px;
    aspect-ratio: 1/1;
    justify-content: flex-end;
    align-items: center;
    margin-top: 1.5rem;

}

.btn-box :hover label{
    background-color: rgba(0, 0, 0, 0);
        top: -2.6rem;
        transition: 0.2s ease-out;
}
.btn-box label{
        position: relative;
        color:black;
        top:-1.9rem;
        margin-bottom: -2.5rem;
        font-size:large;
        background-color: var(--p-surface-0);
}

.btn-box i{
    height: 100%;
    z-index: 98;
    justify-self: center;
    align-self: center;
    align-content: center;
    font-size: 4.5rem;
    color: black;
}

.btn-group{
    display: flex;
        height: fit-content;  
        flex-direction: column;
        flex-wrap: wrap;    
    }

.btn-row{
    display: flex;
    justify-content: center;
    gap: 2rem;
    align-items: center;
    flex: 1;
}

</style>