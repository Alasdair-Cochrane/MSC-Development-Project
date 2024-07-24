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

//exclude users already assigned to the unit from selection
const setEligibleUsers = () =>
{
    let userIds = props.Users.map(x => x.userId)
    usersOptions.value = store.PublicUsers.filter(x => !userIds.includes(x.id))
}

onMounted(() => {setEligibleUsers(); console.log(props.UnitId)})

const props = defineProps({
    UnitId : {
        type : Number
    },
    Users : {
        type : Array
    }   })

const confirm = async () =>{
    assignmentLoading.value = true
    var result = await AssignUser(selectedUser.value.id, selectedRole.value.id, props.UnitId)
    if(result.successfull){
        emits('confirmed', result.assignment)
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
        <Select v-model="selectedUser" filter dropdown :options="usersOptions" option-label="email" placeholder="Select User">
            <template #option="slotProps">
                <span>{{slotProps.option.firstName}} {{slotProps.option.lastName}}   {{ slotProps.option.email }}</span>
            </template>
        </Select>
        <Select v-model="selectedRole" :options="store.Roles" optionLabel="name" placeholder="Select Role"></Select>
        </div>
        <div id="user-selection-btns">
            <Button label="Confirm" @click="confirm"></Button>
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