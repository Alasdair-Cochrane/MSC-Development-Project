import './assets/main.css';
import 'primeicons/primeicons.css'
import 'primeflex/primeflex.css'
import Primeview from 'primevue/config';
import { createApp } from 'vue';
import App from './App.vue';
import Aura from '@primevue/themes/aura';
import router from '@/router/index'
import { definePreset, updatePrimaryPalette } from '@primevue/themes';
import ToastService from 'primevue/toastservice';

const MyPreset = definePreset(Aura,{
    semantic: {
        primary:{
            50: '{blue.50}',
            100: '{blue.100}',
            200: '{blue.200}',
            300: '{blue.300}',
            400: '{blue.400}',
            500: '{blue.500}',
            600: '{blue.600}',
            700: '{blue.700}',
            800: '{blue.800}',
            900: '{blue.900}',
            950: '{blue.950}'
        }
    }
})

createApp(App).
    use(Primeview,
        {
            theme: {
                preset: MyPreset,
                options: {
                    prefix: 'p',
                    darkModeSelector: 'system',
                    cssLayer: false
                }
            }
        }).
        use(router).
        use(ToastService).
    mount('#app')
