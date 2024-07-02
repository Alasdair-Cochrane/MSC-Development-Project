import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import  AddItemView  from '@/views/AddItemView.vue'
import SearchItemView from '@/views/SearchItemView.vue'

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
            name: 'addItem',
            component: AddItemView
        },
        {
            path: '/items/search',
            name: 'searchItem',
            component: SearchItemView,

        }
    ]
})

export default router
