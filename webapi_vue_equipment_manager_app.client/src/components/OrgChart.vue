<script setup>
import { store, UpdateUnits, UpdateStructure } from '@/Store/Store';
import {onMounted, ref} from 'vue'

const units = ref({})
const addUnitVisible = ref(false)
const parentUnit = ref()


onMounted(async () => units.value = store.OrgStructure)

const addUnitClicked = (unitId) => {
    parentUnit.value = store.Units.find( unit => unit.id === unitId)
    console.log(parentUnit.value.id)
    addUnitVisible.value = true;
}

async function unitAdded(){
    await UpdateUnits()
    units.value = await UpdateStructure()
    addUnitVisible.value = false;
}

</script>
<template>
    <div class="container">
        <Dialog v-model:visible="addUnitVisible" modal>
            <AddUnit @cancelled="addUnitVisible = false" :parent=parentUnit @created="unitAdded"></AddUnit>
        </Dialog>
        <OrganizationChart :value="units">
            <template #default="slotProps">
                <div>
                <label>{{ slotProps.node.label }}</label>
                <div class="edit-btns">
                    <Button icon="pi pi-plus" rounded outlined @click="addUnitClicked(slotProps.node.key)"></Button>
                    <Button icon="pi pi-search" rounded outlined ></Button>
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



</style>