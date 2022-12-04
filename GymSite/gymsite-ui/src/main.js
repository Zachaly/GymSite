import { createPinia } from 'pinia'
import { createApp } from 'vue'
import App from './App.vue'
import 'bulma/css/bulma.css'
import 'bulmaswatch/flatly/bulmaswatch.min.css'
import router from './router/router'

const pinia = createPinia()

createApp(App).use(pinia).use(router).mount('#app')
