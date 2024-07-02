<script>
import { RouterLink } from 'vue-router';
import {ref, onMounted} from 'vue'
import { IsMobile } from '@/Services/DeviceService';

export default{
    setup(){

        onMounted(() => {
    mobileScreen.value = !IsMobile();     
    chartData.value = setChartData();
    chartOptions.value = setChartOptions();
});

const chartData = ref();
const chartOptions = ref();

const mobileScreen = ref(true)

//https://primevue.org/chart/
const setChartData = () => {
    const documentStyle = getComputedStyle(document.body);

    return {
        labels: ['A', 'B', 'C'],
        datasets: [
            {
                data: [540, 325, 702],
                backgroundColor: [documentStyle.getPropertyValue('--p-cyan-500'), documentStyle.getPropertyValue('--p-orange-500'), documentStyle.getPropertyValue('--p-gray-500')],
                hoverBackgroundColor: [documentStyle.getPropertyValue('--p-cyan-400'), documentStyle.getPropertyValue('--p-orange-400'), documentStyle.getPropertyValue('--p-gray-400')]
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
return{
    chartData,
    chartOptions,
    mobileScreen
}
    }
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
                    <RouterLink class="btn-box">
                        <label class="btn-label">Add Item</label> 
                        <img src=/src/assets/plus-icon.svg/>            
                    </RouterLink>
                </div>
                <div class="btn-container">
                    <RouterLink class="btn-box">
                        <label class="btn-label">Add Item</label> 
                        <img src=/src/assets/plus-icon.svg/>            
                    </RouterLink>
                </div>
            </div>
            <div class="btn-row">
                <div class="btn-container">
                    <RouterLink class="btn-box">
                        <label class="btn-label">Add Item</label> 
                        <img src=/src/assets/plus-icon.svg/>            
                    </RouterLink>
                </div>
                <div class="btn-container">
                    <RouterLink class="btn-box">
                        <label class="btn-label">Add Item</label> 
                        <img src=/src/assets/plus-icon.svg/>            
                    </RouterLink>
                </div>
                
            </div>
        </div>

         <!-- Bottom Buttons -->
        <div class="btn-group col-12">
            <div class="btn-row">
                <div class="btn-container">
                    <RouterLink class="btn-box">
                        <label class="btn-label">Add Item</label> 
                        <img src=/src/assets/plus-icon.svg/>            
                    </RouterLink>
                </div>
                <div class="btn-container">
                    <RouterLink class="btn-box">
                        <label class="btn-label">Add Item</label> 
                        <img src=/src/assets/plus-icon.svg/>            
                    </RouterLink>
                </div>
            </div>
            <div class="btn-row">
                <div class="btn-container">
                    <RouterLink class="btn-box">
                        <label class="btn-label">Add Item</label> 
                        <img src=/src/assets/plus-icon.svg/>            
                    </RouterLink>
                </div>
                <div class="btn-container">
                    <RouterLink class="btn-box">
                        <label class="btn-label">Add Item</label> 
                        <img src=/src/assets/plus-icon.svg/>            
                    </RouterLink>
                </div>
                
            </div>
        </div>

 </div>
    
    <!-- Right Column -->
    <div class="col-12 md:col-8 c-main">
        <div class="right-panels">
            <div class="panel data grid-nogutter"> 
                    <div class="left col-12 md:col-6">
                    <DataTable class="">
                        <Column field="status" header="Status"></Column>
                        <Column field="quantity" header="#"></Column>
                    </DataTable>
                    <DataTable class="">
                        <Column field="type" header="Type"></Column>
                        <Column field="quantity" header="#"></Column>
                    </DataTable>
                </div>
                    <div class="right col-12 md:col-6" v-if="mobileScreen">
                        <Chart type="doughnut" :data="chartData" :options="chartOptions"></Chart>
                        <Chart type="doughnut" :data="chartData" :options="chartOptions"></Chart>
                    </div>   
            </div>
            <div class="panel activity">
     
            </div>
        </div>

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
    @media(max-width:768px){
            height: auto;
            margin-left: 2.5rem;
        }

}
.panel{
    display: flex;
    flex-wrap: wrap;
    flex: 1;
    max-height: 50%;
    margin-block: 0.5rem;

    .right{
        flex: 1;
        height: auto; 
        display: flex;
        flex-direction: column;
        justify-content: center; 
        gap: 1rem;  
        @media(max-width:768px){
            flex-direction: row;
            max-height: 300px;
        }   
    }  
    .left{
        padding: 1rem;
    }  

}
.p-chart{
        height: 40%;
        @media(max-width:768px){
            height: auto;
        }
        
    }
.left{
        flex-direction: column;
        display: flex;
        justify-content: space-between;
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


    label{
        position: relative;
        color:black;
        top:-1.9rem;
        margin-bottom: -2.5rem;
        font-size:large;
        background-color: var(--p-surface-0);
    }
    img{
        height: 100%;
        z-index: 98;
    }
    
    &:hover label{
        background-color: rgba(0, 0, 0, 0);
        top: -2.6rem;
        transition: 0.2s ease-out;
    }

    
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