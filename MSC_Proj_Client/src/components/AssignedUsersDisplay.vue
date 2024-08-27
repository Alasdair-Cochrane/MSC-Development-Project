<script setup>
import { DeleteAssignment, EditAssignment } from '@/Services/UserService';
import { store } from '@/Store/Store';
import {defineProps, onMounted, ref} from 'vue'

const showAssignUser = ref(false)
const users = defineModel()
const showDelete = ref(false)
const toBeEdited = ref()
const toBeDeleted = ref()
const showEdit = ref(false)
const selectedRole = ref()
const errorMessage = ref()
const errorOccured = ref(false)
const succesMessage = ref()
const operationSuccesfull = ref(false)
const deletionSuccesfull = ref(false)
const operationLoading = ref(false)


const props = defineProps({
    UnitId :{
        type : Number
    }
})
const newUser = (u) => users.value.push(u)

const editRole = async () => {
    operationLoading.value = true
    errorMessage.value = ""
    if(toBeEdited.value.roleId === selectedRole.value.id){
        operationLoading.value = false
        return;
    }
    var editedAssignment = {unitId: toBeEdited.value.unitId, 
        userId: toBeEdited.value.userId, 
        roleId: selectedRole.value.id}
    let result = await EditAssignment(editedAssignment)

    if(result.successfull){
        operationSuccesfull.value = true;

        let edited = users.value.find(u => u.userId === result.updated.userId);
        if(edited){
            edited.roleId = result.updated.roleId
            edited.roleName = result.updated.roleName
        }
        else{
            console.warn("Could not find existing user assignment")
        }
        succesMessage.value = "Update Successfull"
    }
    else{
        errorOccured.value = true
        errorMessage.value = result.message ?? result.errors
    }
    //
    toBeEdited.value = null
    operationLoading.value = false
}

const toggleEdit = (data) => {
    toBeEdited.value = data
    if(!data){
        return;
    }
    selectedRole.value = {id: toBeEdited.value.roleId, name : toBeEdited.value.roleName}
}


const deleteRole = async () => {
    deletionSuccesfull.value = false;
    operationLoading.value = true;

    let result = await DeleteAssignment(toBeDeleted.value)

    if(result.successfull){
        users.value = users.value.filter(u => u !== toBeDeleted.value)
        deletionSuccesfull.value = true;
        succesMessage.value = "Deletion Successfull"
    }
    else{
        errorOccured.value = true
        console.warn(result.message)
        errorMessage.value = result.message ?? result.errors
    }
    operationLoading.value = false
}

</script>
<template>
    <div class="container">
    <DataTable :value="users" scrollable size="small">
                    <Column field="firstName" header="Forename"></Column>
                    <Column field="lastName" header="Surname"></Column>
                    <Column field="roleName" header="Role">
                        <template #body="{data}">
                            <div >
                                <span v-show="toBeEdited !== data">{{ data.roleName }}</span>
                                <Select v-show="toBeEdited == data" v-model="selectedRole" :options="store.Roles" optionLabel="name" placeholder="Select Role"></Select>
                            </div>
                        </template>
                    
                    </Column>
                    <Column header="Edit">
                        <template #body="{data}">
                            <div class="btn role-btn">
                                <Button v-show="toBeEdited !== data" rounded outlined icon="pi pi-pencil" @click="toggleEdit(data)" ></Button>
                                <Button v-show="toBeEdited !== data" rounded outlined icon="pi pi-trash" @click="{showDelete = true; toBeDeleted = data;errorOccured=false}" 
                                :loading="operationLoading"></Button>
                                <Button v-show="toBeEdited == data" rounded outlined icon="pi pi-times" @click="toggleEdit(null)"></Button>
                                <Button v-show="toBeEdited == data" rounded outlined icon="pi pi-check" @click="editRole(data)" :loading="operationLoading"></Button>

                            </div>
                        </template>
                    </Column>
                </DataTable>
                <small v-show="errorOccured">{{ errorMessage }}</small>
                <Button label="New Member" icon="pi pi-plus" @click="showAssignUser = true" v-if="!showAssignUser"></Button>
                <AssignUser v-if="showAssignUser" :Users=users :UnitId="UnitId" @confirmed="newUser" @cancelled="showAssignUser = false"></AssignUser>

                <Dialog v-model:visible="showDelete" modal>
                    <div class="deletion" v-show="!deletionSuccesfull">
                        <strong>Confirm Deletion?</strong>
                        <div>
                             <Button label="Confirm" @click="deleteRole" :loading="operationLoading"></Button>
                            <Button label="Cancel" @click="toBeDeleted=null; showDelete = false" severity="danger"></Button>
                        </div>
                        <span v-show="errorOccured"> Failed to Delete : {{ errorMessage }}</span>
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
.container{
    display: flex;
    flex-direction: column;
    gap: 2rem;
    align-items: center
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

</style>