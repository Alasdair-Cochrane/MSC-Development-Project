<script setup>
import OrgChart from '@/components/OrgChart.vue'
import OrgTable from '@/components/OrgTable.vue'
import AddUnit from '@/components/AddUnit.vue';
import { onMounted, ref } from 'vue';
import { store, UpdateStructure} from '@/Store/Store';

const showOrgChart = ref(false)
const showAddNewUnit = ref(false)
const orgStructure = ref([])
const orgTable = ref(null)
const dataLoading = ref(true)

const chartIndex = ref()

onMounted(async () => {
    if(store.OrgStructure.length === 0){
        await UpdateStructure()
    }
    let structure = store.OrgStructure.length
    orgStructure.value.push(structure)
    dataLoading.value = false;
}
)

const newStructure = () => {
    console.log("structure new")
    orgTable.value.NewStructure()
}

const chartShown = (i) =>{
    console.log(i)
    chartIndex.value = i;
    showOrgChart.value = true
}

</script>
<template>
<div class="wrapper" >

        <div class="org" id="org-table">
            <div class="header-btns" v-show="!dataLoading">
                <Button label="New" icon="pi pi-plus" @click="showAddNewUnit = true"></Button>
            </div>
            <OrgTable v-model="orgStructure" ref="orgTable" v-if="!dataLoading" @chartButtonClicked="chartShown"></OrgTable>
        </div>
<!-- Org Chart Popup -->
        <div v-if="showOrgChart">
        <Dialog v-model:visible="showOrgChart">
            <div class="org" id="org-chart">
                <OrgChart :index="chartIndex" @unitAdded="newStructure"></OrgChart>
            </div>
        </Dialog>
        </div>
<!-- Add New Unit Popup -->
<Dialog v-model:visible="showAddNewUnit" header="New Unit">
    <AddUnit @confirmed="newStructure()" @cancelled="showAddNewUnit= false"></AddUnit>
</Dialog>
<div v-if="dataLoading">
    <Skeleton class="mb-2"></Skeleton>
    <Skeleton class="mb-2"></Skeleton>
    <Skeleton class="mb-2"></Skeleton>
    <Skeleton class="mb-2"></Skeleton>
    <Skeleton class="mb-2"></Skeleton>
    <Skeleton class="mb-2"></Skeleton>
    <Skeleton class="mb-2"></Skeleton>
    <Skeleton class="mb-2"></Skeleton>
</div>

</div>
</template>
<style scoped>
.wrapper{
    display: flex;
    flex: 1;
    height: 100%;
    padding: 1rem;
    flex-wrap: wrap;
    gap: 2rem;
    justify-content: center;
    align-items: center;
    background-color: var(--p-surface-50);
}
.header-btns{
    display: flex;
    justify-content: space-between;
    padding: 1rem;
}

#org-table{
    min-height: 50%;
    flex: 1;
    min-width: 600px;
    height: fit-content;
    padding: 1rem;
    background-color: white;
    border-radius: 10px;
    box-shadow:  0 2px 2px 0 rgba(28, 25, 25, 0.2);

    
}
@media(max-width:768px){
    #org-table{
        max-width: none;
    }
}
</style>