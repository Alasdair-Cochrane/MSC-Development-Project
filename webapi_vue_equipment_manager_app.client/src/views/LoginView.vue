<script setup>
import {ref} from 'vue';


const showLogin = ref(true)
const showPasswordRest = ref(false)
const showRegister = ref(false)

const toggleLogin = () => {showLogin.value = true; showPasswordRest.value = false; showRegister.value = false}
const toggleForgot = () => {showLogin.value = false; showPasswordRest.value = true; showRegister.value = false}
const toggleRegister = () => {showLogin.value = false; showPasswordRest.value = false; showRegister.value = true}

</script>
<template>
<div class="wrapper">

    <div class="panel">
        <LoginComponent v-show="showLogin" @register="toggleRegister" @password-forgot="toggleForgot"></LoginComponent>
        <RegisterComponent v-show="showRegister" @login="toggleLogin"></RegisterComponent>       
        <ForgotPasswordComponent v-show="showPasswordRest" @login="toggleLogin"></ForgotPasswordComponent> 
    </div>
</div>
</template>
<style scoped>

.panel{
    max-width: 400px;
    min-width: 300px;
    box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
    border-radius: 5%;
    height: auto;
    flex: 1;
}

.wrapper{
    display: flex;
    flex: 1;
    justify-content: center;
    align-items: center;
    height: 100%
}

</style>