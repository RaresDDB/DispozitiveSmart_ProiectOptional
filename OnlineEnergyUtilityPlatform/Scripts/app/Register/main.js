import register from './register.vue'
import Vue from 'vue/dist/vue.esm.js'

new Vue({
    el: '#register',
    components: { register },
    data: {
        loginurl: window.loginurl,
    }
});