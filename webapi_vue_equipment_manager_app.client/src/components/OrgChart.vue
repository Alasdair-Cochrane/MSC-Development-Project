<script setup>
import { store, UpdateUnits, UpdateStructure } from '@/Store/Store';
import {onMounted, ref} from 'vue'

const units = ref({})
const addUnitVisible = ref(false)
const parentUnit = ref()
const loading = ref(true)
const props = defineProps({index:{
    type : Number,
    default : 0,
}})

const emits = defineEmits(['unitAdded'])

onMounted(async () => {
    if(store.OrgStructure.length === 0){
        await UpdateStructure()
    }
    let tree = store.OrgStructure
    units.value = tree[props.index]    
    loading.value = false;

})

const addUnitClicked = (unitId) => {
    let parent = store.Units.find( unit => unit.id === unitId)
    console.log(parent)
    parentUnit.value = parent
    addUnitVisible.value = true;
}

async function unitAdded(){
    await UpdateUnits()
    let tree = await UpdateStructure()
    units.value = tree[props.index]
    addUnitVisible.value = false;
    emits('unitAdded')
}

</script>
<template>
    <div class="container">
        <Dialog v-model:visible="addUnitVisible" modal>
            <AddUnit @cancelled="addUnitVisible = false" :parent=parentUnit @confirmed="unitAdded"></AddUnit>
        </Dialog>
        <OrganizationChart :value="units" v-if="!loading">
            <template #default="slotProps">
                <div>
                <label>{{ slotProps.node.label }}</label>
                <div class="edit-btns">
                    <Button icon="pi pi-plus" rounded outlined @click="addUnitClicked(slotProps.node.key)"></Button>
                </div>


            </div>
    </template>
        </OrganizationChart>
    </div>
</template>
<style scoped>
.p-organizationchart-node div{
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: 5px;
    align-items: center
}
.p-organizationchart-node div Button{ 
    height: 25px;
    width: 25px;
}
.p-organizationchart-node div div{
    flex: 1;
    display: flex;
    flex-direction: row;
    gap: 5px;
    margin-top: 5px;
}
.p-organizationchart-node div{
    min-width: 60px;
    width: fit-content;
    display: flex;
    justify-content: center;
}
.container{
    max-width: 95vw;
}





</style>