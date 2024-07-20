<script setup>

import { ref } from 'vue'
import { RouterLink} from 'vue-router'
import { userLogout } from '@/Services/UserService';

const expanded = ref(localStorage.getItem("expanded") === "true")

const ToggleMenu = () => {
    expanded.value = !expanded.value
    localStorage.setItem("expanded", expanded.value)
}

const logOut = () => userLogout()



</script>

<template>
    <aside :class="`${expanded ? 'expanded' : ''}`">
        <div class="logo">
            <img src="../assets/testlogo.png" alt="logo" />
        </div>

        <div class="menu-toggle-wrap">
            <button class="menu-toggle" @click="ToggleMenu">
                <i class="pi pi-arrow-right nav-icon" style="font-size : 2rem ; color: white"/>
            </button>
        </div>
        <div class="menu">
            <div>
            <RouterLink to="/" class="menu-button">
                <i class="pi pi-home menu-icon" />
                <span class="menu-text">Home</span>
            </RouterLink>
            <RouterLink to="/add" class="menu-button">
                <i class="pi pi-plus menu-icon" />
                <span class="menu-text">Add</span>
            </RouterLink>
            <RouterLink to="/search" class="menu-button">
                <i class="pi pi-search menu-icon" />
                <span class="menu-text">Search</span>
            </RouterLink>
            <RouterLink class="menu-button" to="/manage">
                <i class="pi pi-table menu-icon" />
                <span class="menu-text">View All</span>
            </RouterLink>
            <RouterLink class="menu-button">
                <i class="pi pi-users menu-icon" />
                <span class="menu-text">Manage Users</span>
            </RouterLink>
            <RouterLink class="menu-button" to="/units">
                <i class="pi pi-building menu-icon" />
                <span class="menu-text">Manage Units</span>
            </RouterLink>
         </div>
         <div>
            <div class="menu-button" id="sign-out" @click="logOut">
                <i class="pi pi-sign-out menu-icon" />
                <span class="menu-text">Sign Out</span>
            </div>
        </div>
        </div>
       

    </aside>
</template>

<style scoped>

    aside{
        display: flex;
        flex-direction: column;

        background-color: var(--p-primary-color); /*COLOUR*/
        width: 4rem;
        min-width: 4rem;
        height: 100%;
        min-height: 100vh;
        overflow: hidden;
        padding: 1rem;
        position: sticky;
        top: 0;
        transition: 0.2s ease-out;
       

        .menu-toggle-wrap{
             display:flex;
             justify-content:flex-end;
             margin-bottom: 1rem;

             position: relative;
             top: 0;
             transition: 0.2s ease-out;

             .menu-toggle{
                    transition: 0.2s ease-out;
                    .nav-icon{
                                 transition: 0.2s ease-out;
                             }

                    &:hover{
                        .nav-icon{
                            
                            transform: translatex(0.5rem);
                        }
                    }
             }
         }

        &.expanded{
             min-width: 250px; /*SIZE*/
                .menu-toggle-wrap{
                            top: -3rem;
                        }
                        .menu-toggle{
                            transform: rotate(-180deg);
                        }
            .menu-button .menu-text {
                opacity: 1;
                
            }
            .menu-button{
                .menu-icon{
                              margin-right: 1rem;
                          }
            }

        }

        @media(max-width:768px){
            position:fixed;
            z-index: 99;
        }
        .logo{
                 margin-bottom: 1rem;
                 img{
                        width:2rem
                    }
             }

    }

    .menu{
        margin: 0 -1rem;
        height: 100%;
        flex: 1;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        .menu-button{
                   display: flex;
                   align-items: center;
                   text-decoration: none;
                   

                   padding: 0.5rem 1rem;
                   transition: 0.2s ease-out;

                   .menu-icon{
                                 font-size: 2rem;
                                 color: white;
                                 transition: 0.2s ease-out;
                                 margin-right: 1rem;
                              }
                    .menu-text {
                        color: white; /*COLOUR*/
                        transition: 0.2s ease-out;
                    }
                    &:hover{
                        background-color: var(--p-surface-0); /*COLOUR*/
                        .menu-text, .menu-icon{
                                                 color: var(--p-blue-900); /*COLOUR*/
                                             }
                    }

               }
    }

    .menu-button .menu-text {
        opacity: 0;
        transition: 0.3s ease-out;
    }


    .menu-text{
        font-size: 1.2rem;
    }

</style>