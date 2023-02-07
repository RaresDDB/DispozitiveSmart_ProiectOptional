import deallocatedevice from './deallocatedevice.vue'
import Vue from 'vue/dist/vue.esm.js'

Vue.config.productionTip = false

import VueMaterial from 'vue-material'
import 'vue-material/dist/vue-material.min.css'
import 'vue-material/dist/theme/default.css'

Vue.use(VueMaterial)

new Vue({
    el: '#deallocatedevice',
    components: { deallocatedevice },
    data: {
        ddevices: window.adevices,
    }
});