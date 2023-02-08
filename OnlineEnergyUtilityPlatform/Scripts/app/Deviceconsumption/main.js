import deviceconsumption from './deviceconsumption.vue'
import linechar from "./linechart.vue"
import Vue from 'vue'

new Vue({
    el: '#deviceconsumption',
    components: {
        linechar,
        deviceconsumption
    },
    data: {
        mdevice: window.mdevice,
    }
});