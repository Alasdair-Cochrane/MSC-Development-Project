import './assets/main.css';
import './assets/InputForm.css';
import 'primeicons/primeicons.css'
import Primeview from 'primevue/config';
import { createApp } from 'vue';
import App from './App.vue';
import Aura from '@primevue/themes/aura';



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
    mount('#app')
