import allocatedevice from './allocatedevice.vue'
import Vue from 'vue/dist/vue.esm.js'

Vue.config.productionTip = false

import VueMaterial from 'vue-material'
import 'vue-material/dist/vue-material.min.css'
import 'vue-material/dist/theme/default.css'

Vue.use(VueMaterial)

new Vue({
    el: '#allocatedevice',
    components: { allocatedevice },
    data: {
        adevices: window.adevices,
        ausers: window.ausers,
    }
});