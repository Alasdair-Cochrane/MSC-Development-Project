<script setup>
    import AddItem from './components/AddItem.vue'
    import ViewItem from './components/ViewItem.vue'
    import {RouterLink, RouterView} from 'vue-router'
    import Sidebar from './components/Sidebar.vue'


</script>

<template>
    
        <div class="app">
            <Sidebar/>
        <main>
            <RouterView>

            </RouterView>
        </main>
       </div>
</template>

<style>
    * {
        margin: 0;
        padding: 0;
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
        main {
                 flex: 1 1 0;
                 padding: 2rem;

                 @media (max-width:768){
                     padding-left:6rem;
                 }
             }
    }

</style>
