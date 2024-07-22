<script setup>
//Used chatgpt to troubleshoot accessing tree-node value for the edit & users buttons 
//- inserted '' #body="slotProps" ' per ChatGpt's 'advice' 

import { store } from '@/Store/Store';
import {onMounted, onScopeDispose, ref} from 'vue'
import AssignUser from './AssignUser.vue';
import AssignedUsersDisplay from '@/components/AssignedUsersDisplay.vue'

const unitTree = ref([])
const selectedUnit = ref()
const showUsersDialog= ref(false)
const showAssignUser = ref(false)


onMounted(async () => {
    unitTree.value.push(store.OrgStructure); })


const showUsers = (unit) => {
    showUsersDialog.value = true
    selectedUnit.value = unit 
}

</script>

<template>
    <div class="container">
        <TreeTable :value="unitTree" size="small" tableStyle="min-width:40rem" showGridlines scrollable>
            <Column field="name" header="Name" expander style="min-width:30%"></Column>
            <Column field="building" header="Building" ></Column>
            <Column field="room" header="Room" ></Column>
            <Column field="address" header="Address" ></Column>

            <Column style="width: 15%" header="Members">
                <template #body="slotProps">
                    <div class="btn field-btn">
                        <span>{{ slotProps.node.data.assignedUsers.length }}</span>
                        <Button rounded outlined icon="pi pi-search" @click="showUsers(slotProps.node.data)"></Button>
                    </div>
                </template>
            </Column>
            <Column style="width: 5%">
                <template #body="slotProps">
                    <div class="btn">
                        <Button rounded outlined icon="pi pi-pencil"></Button>
                    </div>
                </template>
            </Column>
        </TreeTable>

<!-- Show Users Pop Up -->
        <Dialog modal v-model:visible="showUsersDialog">
            <AssignedUsersDisplay v-model="selectedUnit.assignedUsers" :UnitId="selectedUnit.id"></AssignedUsersDisplay>
            </Dialog>
    </div>
</template>
<style scoped>
.field-btn{
    display: flex;
    justify-content: space-around;
    flex: 1;
}
.btn Button{
    width: 30px;
    height: 30px;
}
.role-btn{
    display: flex;
    justify-content: space-between;
    flex: 1;
    gap: 5px;
}

Dialog{
    max-height: 70%;
}

</style>