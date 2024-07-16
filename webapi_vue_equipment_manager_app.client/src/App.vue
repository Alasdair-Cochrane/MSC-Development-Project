<script setup>
    import {RouterLink, RouterView} from 'vue-router'
    import SideBar from './components/SideBar.vue'
import { loggedIn } from './Store/Store';
import LoginView from './views/LoginView.vue';
import {ref} from 'vue'


</script>

<template>    
    <div class="app" >
            <SideBar v-if="loggedIn"/>
        <main>
            <RouterView class="content" v-if="loggedIn">

            </RouterView>
            <div v-else class="content login-view">
                <LoginView ></LoginView>
        </div>
        </main>
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
    height: auto;
    
}   

main {
     flex: 1 1 0;     
 } 

 @media(max-width:768px)
     {
        main{
        margin-left: 4rem;
        }
     }

.content{
    flex:1;
    height: auto;         
}

.login-view{
    height: 100vh;
    display: flex;
    flex: 1;
}

</style>
