<script setup>
import {defineProps, onMounted, ref} from 'vue'
import {store} from '@/Store/Store'
import { AssignUser } from '@/Services/UserService';
const selectedRole = ref()
const selectedUser = ref()
const usersOptions = ref()
const assignmentLoading = ref(false)
const emits = defineEmits(['confirmed', 'cancelled'])
const errorOccured = ref(false)
const errorMessage = ref()

const noUser = ref(false)
const noRole = ref(false)

//exclude users already assigned to the unit from selection
const setEligibleUsers = () =>
{
    let userIds = props.Users.map(x => x.userId)
    usersOptions.value = store.PublicUsers.filter(x => !userIds.includes(x.id))
}

onMounted(() => {setEligibleUsers();})

const props = defineProps({
    UnitId : {
        type : Number
    },
    Users : {
        type : Array
    }   })

const confirm = async () =>{
    if(!selectedUser.value){
        noUser.value = true
        return
    }
    if(!selectedRole.value){
        noRole.value = true
        return
    }
    noUser.value = false
    noRole.value = false

    assignmentLoading.value = true
    var result = await AssignUser(selectedUser.value.id, selectedRole.value.id, props.UnitId)
    if(result.successfull){
        emits('confirmed', result.assignment)
        selectedUser.value = null
        selectedRole.value = null
    }
    else{
        errorOccured.value = true
        errorMessage.value = result.errors
    }
    assignmentLoading.value = false;

}

</script>
<template>
    <div class="container">
        <div id="selection-input">
        <Select v-model="selectedUser" filter dropdown 
        :options="usersOptions" option-label="email" placeholder="Select User" :invalid="noUser">
            <template #option="slotProps">
                <span>{{slotProps.option.firstName}} {{slotProps.option.lastName}}   {{ slotProps.option.email }}</span>
            </template>
        </Select>
        <Select v-model="selectedRole" 
        :options="store.Roles" optionLabel="name" placeholder="Select Role" :invalid="noRole"></Select>
        </div>
        <div id="user-selection-btns">
            <Button label="Confirm" @click="confirm" :loading="assignmentLoading"></Button>
            <Button label="Cancel" @click="$emit('cancelled')" severity="danger"></Button>
        </div>
        <small v-show="errorOccured">Assignment Failed : {{ errorMessage }}</small>
    </div>
</template>
<style scoped>
#selection-input{
    display: flex;
    gap:2rem;
    flex-wrap: wrap;
}
#user-selection-btns{
    display: flex;
    gap:2rem;
}
</style>