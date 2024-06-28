import './assets/main.css';
import 'primeicons/primeicons.css'
import Primeview from 'primevue/config';
import { createApp } from 'vue';
import App from './App.vue';
import Aura from '@primevue/themes/aura';
import router from '@/router/index'



createApp(App).
    use(Primeview,
        {
            theme: {
                preset: Aura,
                options: {
                    prefix: 'p',
                    darkModeSelector: 'system',
                    cssLayer: false
                }
            }
        }).
        use(router).
    mount('#app')
