<script setup>
import {ref} from 'vue';

const email = ref(null)
const password = ref(null)
const fName = ref()
const lName = ref()

const successfull = ref(false)
const unsuccessfull = ref(false)
const loading = ref(false)


const errors = ref([])

async function register(){
    loading.value = true;
    let user = {
        email : email.value,
        password : password.value,
        firstName : fName.value,
        lastName : lName.value
    }

    var response = await fetch('api/user/register',
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
    }
    else{
        let result = await response.json()
        unsuccessfull.value = true;
        errors.value = result.errors;
        console.log(result.errors)
    }
    loading.value = false;
}
</script>
<template>
<div class="wrapper">
    <h2>Register</h2>
    <div id="login" class="inputForm" v-show="!successfull">
        <FloatLabel>
            <InputText id="fName" v-model="fName" :invalid="accessFailed"/>
            <label for="fName">First Name</label>
        </FloatLabel>
        <FloatLabel>
            <InputText id="lName" v-model="lName" :invalid="accessFailed"/>
            <label for="lName">Last Name</label>
        </FloatLabel>        
        
        <FloatLabel>
            <InputText id="email-register" v-model="email" :invalid="accessFailed"/>
            <label for="email-register">Email</label>
        </FloatLabel>
        <FloatLabel>
            <Password id="password-register" v-model="password" toggleMask :invalid="accessFailed"/>
            <label for="password-register">Password</label>
        </FloatLabel>
        <Button label="Register" @click="register"></Button>
        
    </div>
    <div v-show="successfull">
        <label>Registration Successfull!</label>
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
</style>