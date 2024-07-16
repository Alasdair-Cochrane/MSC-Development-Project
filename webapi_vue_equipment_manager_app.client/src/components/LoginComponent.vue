<script setup>
import {ref} from 'vue';
import { userLogin } from '@/Services/UserService';

const email = ref(null)
const password = ref(null)
const successfull = ref(false)
const unsuccessfull = ref(false)
const error = ref()
const loading = ref(false)

async function login(){
    loading.value = true;
    let login = {
        email : email.value,
        password : password.value,
    }
    var response = await userLogin(login)
    if(response.successfull){
        successfull.value = true;
        unsuccessfull.value = false;
        console.log("login successful")
    }
    else{
        unsuccessfull.value = true;
        error.value = response.errors;
        loading.value = false;
    }
}

</script>
<template>
<div class="wrapper">
    <div id="login" class="inputForm">
        <h2>Login</h2>
        <small v-show="unsuccessfull">Login Failed : {{ error }}</small>
        <div>
            <FloatLabel>
                <InputText id="email" type="email" v-model="email" :invalid="unsuccessfull"/>
                <label for="email">Email</label>
            </FloatLabel>
        </div>

        <FloatLabel>
            <Password id="password" v-model="password" toggleMask :invalid="unsuccessfull" :feedback=false />
            <label for="password">Password</label>
        </FloatLabel>
        <Button label="Login" @click="login" :loading="loading"></Button>
        <div class="bottom-links">
            <Button label="Forgot Password" severity="secondary" @click="$emit('passwordForgot')"></Button>
            <Button label="Register" severity="secondary" @click="$emit('register')"></Button>
        </div>
    </div>
    
</div>
</template>
<style scoped>


.wrapper{
    display: flex;
    flex: 1;
    justify-content: center;
    align-items: center;
    height: 100%;
}

.inputForm{
    display: flex;
    flex-direction: column;
    padding: 1rem;
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
    min-width: 120px;
}
small{
    color: red;
}

#email{
    min-width: 234px;
}
</style>