<script setup>
import { UserLogout } from '@/Services/UserService';
import { ref } from 'vue';


const expanded = ref(false)
</script>
<template>
<div class="bar" :class="`${expanded ? 'expanded' : ''}`">
    <div class="start-icons">
        <button class="menu-toggle menu-button" @click="expanded = !expanded">
                <i class="pi pi-bars nav-icon"/>
            </button>
    </div>
<div class="nav-icons">
    <div class="btn">
    <RouterLink to="/" class="menu-button">
        <i class="pi pi-home menu-icon" />
        <span class="menu-text">Home</span>
    </RouterLink>
    </div>
    <div class="btn">
    <RouterLink to="/add" class="menu-button">
        <i class="pi pi-plus menu-icon" />
        <span class="menu-text">Add</span>
    </RouterLink>
</div>
    <div class="btn">
    <RouterLink to="/search" class="menu-button">
        <i class="pi pi-search menu-icon" />
        <span class="menu-text">Search</span>
    </RouterLink>
</div>
    <div class="btn">
    <RouterLink class="menu-button" to="/manage">
        <i class="pi pi-table menu-icon" />
        <span class="menu-text">View</span>
    </RouterLink>
</div>
    <div class="btn">
    <RouterLink class="menu-button" to="/units">
        <i class="pi pi-building menu-icon" />
        <span class="menu-text">Locations</span>
    </RouterLink>
</div>
</div>
<div class="end-icons">
    <div class="btn">
    <div class="menu-button" id="sign-out" @click="UserLogout()">
            <i class="pi pi-sign-out menu-icon" />
            <span class="menu-text">Sign Out</span>
        </div>
    </div>
</div>
</div>
</template>
<style scoped>
.bar{
    background-color: var(--p-primary-500);
    height: 80px;
    min-width: 100vw;    
    width: 100%;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding-inline: 8px;
    transition: 0.2s ease-out;
    max-height: 50px;
}
.expanded{
    max-height: 70px;
}
a{
    text-decoration: none;
    color: white;
    background-color: transparent
}
.nav-icons{
    gap: 5px;
}
.menu-button{
    display: flex;
    flex-direction: column;
    width: fit-content;
    color: white;
    align-items: center;
    padding: 2px;
    transition: 0.2s ease-out;
}

.nav-icons{
    flex: 1;
    display: flex;
    justify-content: center;
    gap: 20px;
    height: 100%;
    transition: 0.2s ease-out;
}

.expanded .menu-toggle{
    transform: rotate(90deg);
}
span{
    font-size: small;
}
 .menu-button span{
    display:none;
}
.menu-button i{
    font-size: 1rem;
}
.btn{
    height: 100%;
    display: flex;
    align-items: center;
    width: fit-content;
}
.expanded .btn :hover{          
    background-color: var(--p-primary-100); /*COLOUR*/
    color: var(--p-surface-0);
          .menu-text, .menu-icon{
                                   color: var(--p-blue-900); /*COLOUR*/
                               }
}
.btn :hover i{          
    font-size: x-large;
}

.expanded .menu-button span{
    display: contents;
}
.expanded .nav-icons{
    gap: 10px;
}


</style>