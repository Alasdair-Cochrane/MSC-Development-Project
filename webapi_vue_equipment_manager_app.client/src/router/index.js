import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import  AddItemView  from '@/views/AddItemView.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: '/',
            name: 'home',
            component: HomeView
        },
        {
            path: '/items/add',
            name: 'additem',
            component: AddItemView
        },
    ]
})

export default router
