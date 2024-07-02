<script setup>
    import AddItem from './components/AddItem.vue'
    import {RouterLink, RouterView} from 'vue-router'
    import Sidebar from './components/Sidebar.vue'


</script>

<template>
    
    <div class="app">
            <Sidebar/>
        <main>
            <RouterView class="content">

            </RouterView>
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
        main {
                 flex: 1 1 0;
                 @media(max-width:768px)
                 {
                    margin-left: 4rem;
                 }

             }
    }    
.content{
    flex:1;
    height: auto; 

        
}

</style>
