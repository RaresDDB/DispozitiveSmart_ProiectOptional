import admindevice from './admindevice.vue'
import Vue from 'vue/dist/vue.esm.js'

Vue.config.productionTip = false

import VueMaterial from 'vue-material'
import 'vue-material/dist/vue-material.min.css'
import 'vue-material/dist/theme/default.css'

Vue.use(VueMaterial)

new Vue({
    el: '#admindevice',
    components: { admindevice },
    data: {
        devices: window.devices,
    }
});