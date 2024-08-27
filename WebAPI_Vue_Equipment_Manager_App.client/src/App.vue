<script setup>
    import {RouterLink, RouterView} from 'vue-router'
    import SideBar from './components/SideBar.vue'
import { loggedIn, PopulateStartingData} from './Store/Store';
import LoginView from './views/LoginView.vue';
import {onBeforeMount, onMounted, ref} from 'vue'
import { CheckRefreshToken} from './Services/UserService';
import { IsMobile } from './Services/DeviceService';
import MobileNavBar from './components/MobileNavBar.vue';

const loading = ref(false)
const mobileTest = ref(false)

onBeforeMount(async  () => {
    await CheckRefreshToken()
    if(loggedIn){
        await PopulateStartingData()
    }
})
onMounted(async () => {
    loading.value= true;
    
    loading.value = false
})

</script>

<template>    
    <div class="app" >
            <SideBar v-if="loggedIn && !IsMobile() && !mobileTest"/>
            <MobileNavBar v-if="loggedIn && (IsMobile() || mobileTest)"></MobileNavBar>
        <div class="main">
            <RouterView class="content" v-if="loggedIn && !loading">

            </RouterView>
        <div v-else-if="!loggedIn" class="content login-view">
                <LoginView ></LoginView>
        </div>
        <div v-else>
            LOADING
        </div>
    </div>
    </div>
    
</template>

<style>
* {
    box-sizing: border-box;
}

button {
    cursor: pointer;
    appearance: none;
    border: none;
    outline: none;
    background: none;
}

.app{
    display:flex;
    flex-wrap: wrap;
    min-height: 100%;     
    align-items: flex-start;
    align-content: flex-start;
}   

.main {
    flex: 1;
    min-height:100%;
    flex-grow: 1;
 } 
.content{
    flex:1;
    min-height: 100vh;   
    flex-grow: 1;
  
}

.login-view{
    height: 100vh;
    display: flex;
    flex: 1;
}

</style>
