import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import  AddItemView  from '@/views/AddItemView.vue'
import SearchItemView from '@/views/SearchItemView.vue'
import ManageItemsView from '@/views/ManageItemsView.vue'
import LoginView from '@/views/LoginView.vue'
import UnitsView from '@/views/UnitsView.vue'
import {  loggedIn, PopulateStartingData, } from '@/Store/Store'



const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: '/',
            name: 'home',
            component: HomeView
        },
        {
            path: '/add',
            name: 'addItem',
            component: AddItemView
        },
        {
            path: '/search',
            name: 'searchItem',
            component: SearchItemView,

        },
        {
            path: '/manage',
            name: 'manageItems',
            component: ManageItemsView
        },
        {
            path: '/login',
            name: 'login',
            component: LoginView
        },
        {
            path: '/units',
            name: 'units',
            component: UnitsView
        }

    ]
})

export default router
