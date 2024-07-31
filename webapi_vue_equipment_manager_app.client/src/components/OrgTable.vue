<script setup>
//Used chatgpt to troubleshoot accessing tree-node value for the edit & users buttons 
//- inserted '' #body="slotProps" ' per ChatGpt's 'advice' 

import { store, UpdateStructure } from '@/Store/Store';
import {onMounted, ref} from 'vue'
import AssignUser from './AssignUser.vue';
import AssignedUsersDisplay from '@/components/AssignedUsersDisplay.vue'
import { DeleteUnit, EditUnit } from '@/Services/UnitService';

const unitTree = ref([])
const selectedUnit = ref()
const showUsersDialog= ref(false)
const expandedKey= ref({})
const editErrorOccurred = ref(false)
const errorMessage = ref()
const operationLoading = ref(false)

const toBeEdited = ref(null)
const toBeDeleted = ref()
const showDelete = ref()
const deletionSuccesfull = ref(false)
const deleteErrorMessage = ref()
const deleteErrorOccured = ref(false)

const openOptions = [{label: "Private", value: false}, {label: "Public", value: true}]
const openOptionSelected = ref()

const emits = defineEmits(['chartButtonClicked'])

onMounted(async () => {
    if(store.OrgStructure.length === 0){
        await UpdateStructure()
    }
    unitTree.value = store.OrgStructure
    for(const element of unitTree.value){
        expandedKey.value[element.key] = true
    }
    }
)

const setOpenOption = () =>{
    console.log(toBeEdited.value.isPublic)
    if(toBeEdited.value.isPublic){
        openOptionSelected.value = openOptions[1]
    }
    else{
        openOptionSelected.value = openOptions[0]
    }
}

const deleteUnit = async () =>{
    operationLoading.value = true;
    var result = await DeleteUnit(toBeDeleted.value)
    if(result.successful){
        await NewStructure()
        deletionSuccesfull.value = true;
    }
    else{
        deleteErrorOccured.value = true;
        deleteErrorMessage.value = result.error + " " + result?.message
    }
    operationLoading.value = false;
}

defineExpose({NewStructure})

async function NewStructure(){
    let org = await UpdateStructure()
    unitTree.value = []
    unitTree.value = org
}


const showUsers = (unit) => {
    showUsersDialog.value = true
    selectedUnit.value = unit 
}

const confirmEdit = async (unit) =>{
    editErrorOccurred.value = false;
    unit.isPublic = openOptionSelected.value.value
    let result = await EditUnit(unit)
    if(result.successful){
        toBeEdited.value = null
        return
    }
    else{
        editErrorOccurred.value = true
        errorMessage.value = result.errors
    }

}

const showChart = (node) =>{
    let index = unitTree.value.indexOf(node)
    console.log(index)
    emits('chartButtonClicked', index)
}


</script>

<template>
    <div class="container">
        <TreeTable :value="unitTree" 
        size="small" tableStyle="min-width:40rem" 
        showGridlines scrollable 
        v-model:expanded-keys="expandedKey">
            <Column style="width: 100px;  ">
                <template #body="slotProps">
                    <div class="btn edit-btns">
                        <Button rounded  icon="pi pi-pencil" v-show="slotProps.node.data !== toBeEdited" @click=" toBeEdited=slotProps.node.data;setOpenOption(); "></Button>
                        <Button rounded severity="danger" icon="pi pi-trash" v-show="slotProps.node.data !== toBeEdited" @click="showDelete = true; toBeDeleted=slotProps.node.data"></Button>
                        <Button rounded outlined icon="pi pi-check" v-show="slotProps.node.data === toBeEdited" @click="confirmEdit(slotProps.node.data)"></Button>
                        <Button rounded outlined icon="pi pi-times" v-show="slotProps.node.data === toBeEdited " @click="toBeEdited=null"></Button>
                    </div>
                </template>
            </Column>
            <Column field="name" header="Name" expander style="width: 300px">
                <template #body="slotProps">
                    <div class="inner-field">
                        <span v-show="slotProps.node.data !== toBeEdited">{{ slotProps.node.data.name }}</span>
                        <InputText v-model=slotProps.node.data.name v-show="slotProps.node.data === toBeEdited"></InputText>
                        <Button rounded outlined icon="pi pi-sitemap" v-show="slotProps.node.data !== toBeEdited && !slotProps.node.data.parentId" @click="showChart(slotProps.node)"></Button>

                    </div>
                </template>
            </Column>
            <Column field="room" header="Room" >
                <template #body="slotProps">
                    <div class="inner-field">
                        <span v-show="slotProps.node.data !== toBeEdited">{{ slotProps.node.data.room }}</span>
                        <InputText v-model=slotProps.node.data.room v-show="slotProps.node.data === toBeEdited"></InputText>
                    </div>
                </template>
            </Column>
            <Column field="building" header="Building" >
                <template #body="slotProps">
                    <div class="inner-field">
                        <span v-show="slotProps.node.data !== toBeEdited">{{ slotProps.node.data.building }}</span>
                        <InputText v-model=slotProps.node.data.building v-show="slotProps.node.data === toBeEdited"></InputText>
                    </div>
                </template>
            </Column>
           
            <Column field="address" header="Address" >
                <template #body="slotProps">
                    <div class="inner-field">
                        <span v-show="slotProps.node.data !== toBeEdited">{{ slotProps.node.data.address }}</span>
                        <InputText v-model=slotProps.node.data.address v-show="slotProps.node.data === toBeEdited"></InputText>
                    </div>
                </template>
            </Column>
            <Column field="isPublic" header="Open"  style="width: 150px;">
                <template #body="slotProps">
                    <div class="inner-field">
                        <span v-show="slotProps.node.data !== toBeEdited && slotProps.node.data.isPublic">Public</span>
                        <span v-show="slotProps.node.data !== toBeEdited && !slotProps.node.data.isPublic">Private</span>
                        <SelectButton v-model="openOptionSelected" :options="openOptions" optionLabel="label" aria-labelledby="basic" v-show="slotProps.node.data === toBeEdited"/>
                    </div>
                </template>
            </Column>


            <Column style="width: 100px" header="Members">
                <template #body="slotProps">
                    <div class="btn field-btn">
                        <span>{{ slotProps.node.data.assignedUsers.length }}</span>
                        <Button rounded severity="primary" icon="pi pi-users" @click="showUsers(slotProps.node.data)"></Button>
                    </div>
                </template>
            </Column>           
            
        </TreeTable>

<!-- Show Users Pop Up -->
        <Dialog modal v-model:visible="showUsersDialog">
            <AssignedUsersDisplay v-model="selectedUnit.assignedUsers" :UnitId="selectedUnit.id"></AssignedUsersDisplay>
            </Dialog>

            <Dialog v-model:visible="showDelete" modal>
                    <div class="deletion" v-show="!deletionSuccesfull">
                        <strong>Confirm Deletion?</strong>
                        <label>{{ toBeDeleted.name }}</label>
                        <div>
                             <Button label="Confirm" @click="deleteUnit" :loading="operationLoading"></Button>
                            <Button label="Cancel" @click="toBeDeleted=null; showDelete = false" severity="danger" v-show="!operationLoading"></Button>
                        </div>
                        <span v-show="deleteErrorOccured"> Failed to Delete : {{ errorMessage }}</span>
                    </div>
                    <div v-show="deletionSuccesfull" class="deletion">
                        <label>Deletetion Successfull</label>
                        <Button label="Close" @click="deletionSuccesfull = false; showDelete = false"></Button>
                    </div>
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
.edit-btns{
    display: flex;
    justify-content: space-around;
    flex: 1;
}

.deletion{
    display: flex;
    flex-direction: column;
    gap: 2rem;
    align-items: center;
}
.deletion strong{
    font-weight: bold;
}
.deletion div{
    display: flex;
    gap: 2rem;
}
.inner-field Button{
    width: 30px;
    height: 30px;
}
.inner-field{
    display: flex;
    flex: 1;
    justify-content: space-between
    
}

</style>