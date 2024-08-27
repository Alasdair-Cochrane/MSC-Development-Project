<script setup>
import OrgChart from '@/components/OrgChart.vue'
import OrgTable from '@/components/OrgTable.vue'
import AddUnit from '@/components/AddUnit.vue';
import { onMounted, ref } from 'vue';
import { store, UpdateStructure} from '@/Store/Store';
import AssignmentsDisplay from '@/components/AssignmentsDisplay.vue';

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
    orgStructure.value.push(store.OrgStructure)
    dataLoading.value = false;
}
)

const newStructure = () => {
    orgTable.value.NewStructure()
}

const chartShown = (i) =>{
    chartIndex.value = i;
    showOrgChart.value = true
}

</script>
<template>
<div class="wrapper" >

        <div class="org" id="org-table">
            <h3>Admin</h3>
            <span v-show="!store.OrgStructure.length && store.StructureLoaded">You are not an administrator of any locations. Create a new location to get started.</span>
            <div class="header-btns">
                <Button label="New" icon="pi pi-plus" @click="showAddNewUnit = true"></Button>
            </div>
            <div v-show="store.OrgStructure.length && store.StructureLoaded">
                <OrgTable v-model="orgStructure" ref="orgTable"  @chartButtonClicked="chartShown"></OrgTable>
            </div>
            <div class="loading">
            <Skeleton v-show="!store.StructureLoaded" width="100%" height="1rem"></Skeleton>
            <Skeleton v-show="!store.StructureLoaded" width="100%" height="1rem"></Skeleton>
            <Skeleton v-show="!store.StructureLoaded" width="100%" height="1rem"></Skeleton>
            <Skeleton v-show="!store.StructureLoaded" width="100%" height="1rem"></Skeleton>
            </div>
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
<AssignmentsDisplay></AssignmentsDisplay>
</div>
</template>
<style scoped>
.wrapper{
    display: flex;
    flex: 1;
    min-height: 100vh;
    height: 100%;
    padding: 1rem;
    flex-wrap: wrap;
    gap: 2rem;
    justify-content: center;
    background-color: var(--p-surface-50);
}
.loading{
    display: flex;
    flex-direction: column;
    gap: 5px;
}
.header-btns{
    display: flex;
    justify-content: space-between;
    padding: 1rem;
}

#org-table{
    min-height: 50%;
    flex: 1;
    height: fit-content;
    min-width: 100%;
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
h3{
    font-weight: bold;
}
</style>