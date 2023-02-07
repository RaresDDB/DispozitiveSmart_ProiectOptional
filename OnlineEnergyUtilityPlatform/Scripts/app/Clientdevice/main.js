import clientdevice from './clientdevice.vue'
import Vue from 'vue/dist/vue.esm.js'

Vue.config.productionTip = false

import VueMaterial from 'vue-material'
import 'vue-material/dist/vue-material.min.css'
import 'vue-material/dist/theme/default.css'

Vue.use(VueMaterial)

//import VueSignalR from '@latelier/vue-signalr'

//Vue.use(VueSignalR, 'https://localhost:443/message')

new Vue({
    el: '#clientdevice',
    components: { clientdevice },
    data: {
        cdevices: window.devices,
    }
});