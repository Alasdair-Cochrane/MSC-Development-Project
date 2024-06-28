import { createRouter, createWebHistory } from 'vue-router'
import Home from '@/views/Home.vue'
import  AddItem  from '../components/AddItem.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: '/',
            name: 'home',
            component: Home
        },
        {
            path: '/AddItem',
            name: 'AddItem',
            component: AddItem
        }
    ]
})

export default router
