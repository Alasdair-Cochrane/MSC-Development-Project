<script setup>
import { getPublicUnits } from '@/Services/UnitService';
import {onMounted, ref} from 'vue';
import AddUnit from './AddUnit.vue';

const email = ref("")
const password = ref("")
const fName = ref()
const lName = ref()
const returnedFirstName = ref()
const returnedLastName = ref()
const returnedOrganisationName = ref()


const successfull = ref(false)
const unsuccessfull = ref(false)
const errors = ref()

const loading = ref(false)
const organisations = ref([])
const emailRegEx = /^\S+@\S+\.\S+$/

const showCreateOrganisations = ref(false)
const selectedOrganisation = ref()

function  createNewOrganisation(org) {
    selectedOrganisation.value = org;
    organisations.value.push(org);
    showCreateOrganisations.value = false;
}
//https://stackoverflow.com/questions/46155/how-can-i-validate-an-email-address-in-javascript
const emailIsValid= ()=>{         
    return emailRegEx.test(email.value)
    
}
//https://stackoverflow.com/questions/26322867/how-to-validate-password-using-following-conditions
const passwordIsValid = () =>{
    if(!password.value){
        return false
    }
    if(password.value.length < 8){
        return false
    }
    return  /[A-Z]/      .test(password.value) &&
           /[a-z]/       .test(password.value) &&
           /[0-9]/       .test(password.value) &&
           /[^A-Za-z0-9]/.test(password.value) 
}
onMounted(async () => {
    organisations.value = await getPublicUnits()}
)

const isValid = ()=>{
    if(!fName.value) return false
    if(!lName.value) return false
    if(!emailIsValid()) return false
    if(!passwordIsValid) return false
    return true
}

async function register(){
    if(!isValid()){
        return;
    }
    loading.value = true;

    let user = {
        email : email.value,
        password : password.value,
        firstName : fName.value,
        lastName : lName.value,
        organisation : selectedOrganisation.value
    }

    var response = await fetch('api/users/register',
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(user),
        });
    if(response.ok){
        successfull.value = true;
        unsuccessfull.value = false;
        let detail = await response.json()
        returnedFirstName.value = detail.firstName;
        returnedLastName.value = detail.lastName;
        returnedOrganisationName.value = detail.unit;
    }
    else{
        let result = await response.json()
        unsuccessfull.value = true;
        errors.value = result.errors;
    }
    loading.value = false;
}
</script>
<template>
<div class="wrapper">
    <h2>Register</h2>
    <div id="login" class="inputForm" v-show="!successfull">
        <FloatLabel>
            <InputText id="fName" v-model="fName" :invalid="!fName"/>
            <label for="fName">First Name</label>
        </FloatLabel>
        <FloatLabel>
            <InputText id="lName" v-model="lName" :invalid="!lName"/>
            <label for="lName">Last Name</label>
        </FloatLabel>        
        
        <FloatLabel>
            <InputText id="email-register" v-model="email" :invalid="!emailIsValid()"/>
            <label for="email-register">Email</label>
        </FloatLabel>
        <FloatLabel>
            <Password id="password-register" v-model="password" toggleMask :invalid="!passwordIsValid()">
                <template #header>
        <div class="font-semibold text-xm mb-4">Pick a password</div>
    </template>
    <template #footer>
        <Divider />
        <ul class="pl-2 ml-2 my-0 leading-normal">
            <li>At least one special character</li>
            <li>At least one uppercase</li>
            <li>At least one numeric</li>
            <li>Minimum 8 characters</li>
        </ul>
    </template>
            </Password>
            <label for="password-register">Password</label>
        </FloatLabel>
        <div id="organisation">
            <Select placeholder="Organisation" :options="organisations" optionLabel="name" v-model="selectedOrganisation" showClear></Select>
            <Button id="new-org-bttn" label="New" icon="pi pi-plus" @click="showCreateOrganisations = true" severity="warn"></Button>
            
            <Dialog v-model:visible=showCreateOrganisations header="Organisation">
                <AddUnit :delay=true @cancelled="showCreateOrganisations = false" @confirmed="createNewOrganisation" modal></AddUnit>
            </Dialog>
        </div>

        <Button label="Register" @click="register"></Button>
        
    </div>
    <div v-show="successfull" id="registration-sucess">
        <label >Registration Successfull!</label>
        <div id="user-details">
            <label>{{ returnedFirstName }} {{ returnedLastName }}</label>
            <label>{{ returnedOrganisationName }}</label>
        </div>
    </div>
    <div class="bottom-links">
            <Button label="Back to Login" icon="pi pi-arrow-circle-left" severity="secondary" @click="$emit('login')"></Button>
        </div>
    
</div>
</template>
<style scoped>


.wrapper{
    display: flex;
    flex-direction: column;
    flex: 1;
    justify-content: center;
    align-items: center;
    height: 100%;
    padding: 1rem;
}

.inputForm{
    display: flex;
    flex-direction: column;
    gap: 2rem;
    padding-block: 1rem;
    align-items: center;
    flex: 1;
}

.bottom-links{
    display: flex;
    justify-content: space-between;
    flex: 1;
    width: 100%;
}
.bottom-links Button{
    font-size: small;
}

.p-inputtext{
    min-width: 234px;
}

#organisation{
    display: flex;
    flex: 1;
    gap : 10px;
}
#registration-sucess{
    padding: 2rem;
    background-color: rgb(149, 237, 142);
    border-radius: 5%;
    margin: 1rem;
    gap: 2rem;    
}
#registration-sucess label{
   color: white;
   font-weight: bold;
    
}

#user-details{
    display: flex;
    flex-direction: column;
    align-items: center;
}


</style>