<script setup>
import { AssignUser } from '@/Services/UserService';
import { GetPublicOrganisations, store } from '@/Store/Store';
import { onMounted, ref } from 'vue';
const joinLoading = ref(false)

onMounted(async () => await GetPublicOrganisations())

const JoinOrg = async (data)=>{
    joinLoading.value = true;
    let result = await AssignUser(store.UserDetails.id,3,data.id)
    if(result.successfull){
        store.UserDetails.assignments.push(result.assignment)
        store.PublicOrganisations = store.PublicOrganisations.filter(x => x.id !== data.id)
    }
    joinLoading.value = false;
}

</script>
<template>
<div class="container">
    <div class="panel">
        <h3>Current Roles</h3>
        <DataTable :value="store.UserDetails?.assignments ?? []" size="small" scrollable scrollHeight="350px" :loading="!store.UserDetails">
            <Column field="unitName" header="Unit"></Column>
            <Column field="roleName" header="Role"></Column>
        </DataTable>
    </div>
    <div class="panel">
        <h3>Available Public Organisations</h3>
        <DataTable :value="store.PublicOrganisations" size="small" scrollable scrollHeight="350px" :loading="!store.PublicOrganisations">
            <Column field="name"></Column>  
            <Column style="width: 50px;">
                   <template #body="{data}">
                   <div class="btn role-btn">
                       <Button  icon="pi pi-plus" label="Join" style="max-width: 60px; font-size: small;max-height: 40px;" @click="JoinOrg(data)" :loading="joinLoading"></Button>
                   </div>
               </template>
            </Column>          
        </DataTable>
    </div>

</div>
</template>
<style scoped>
.container{
    display: flex;
    gap: 1rem;
    height: fit-content;
    width: fit-content;
    flex-wrap: wrap;
}
.panel{
    box-shadow:  0 2px 2px 0 rgba(28, 25, 25, 0.2);
    height: 100%;
    border-radius: 10px;
    padding: 10px;
    min-height: 200px;
    min-width: 320px;
    max-width: 380px;
    max-height: 350px;
    border: black solid 1px;
    background-color: var(--p-surface-0);
    font-size: small;
    flex: 1;
}
h3{
    font-weight: bold;
    font-size: 1rem;
}
*{
    font-size: small
}
</style>