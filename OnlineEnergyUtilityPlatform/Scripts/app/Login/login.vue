<template>
    <div>
        <form>
            <input type="text" name="username" id="login-username-input" placeholder="Username" class="authenticate-input" v-model:value="username" />
            <input v-bind:type="passwordIsTextOrPassword" name="password" id="login-password-input" placeholder="Password" class="authenticate-input" v-model:value="password" />
            <div class="authenticate-button-container">
                <button type="button" class="btn btn-secondary authenticate-button" id="login-sign-in-button" v-on:click="handleSignIn">Sign in</button>
            </div>
        </form>
    </div>
</template>

<script>
    import Vue from 'vue';
    import VueCookies from 'vue-cookies';
    import VueToast from 'vue-toast-notification';
    import 'vue-toast-notification/dist/theme-sugar.css';
    export default {
        data() {
            return {
                username: '',
                password: '',
                token: '',
                passwordIsTextOrPassword: 'password'
            };
        },
        methods: {
            handleSignIn: function () {

                $.ajax({
                    type: "POST",
                    url: '/api/UserApi/login',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                    },
                    data: JSON.stringify({
                        Username: this.username,
                        Password: this.password,
                    }),
                    success: function (data) {
                        Vue.use(VueCookies);
                        Vue.$cookies.set('userId', data.id);
                        Vue.$cookies.set('userName', data.userName);
                        Vue.$cookies.set('user', data);
                        console.log(Vue.$cookies.get('user').role);

                        if (Vue.$cookies.get('user').role == "admin") {
                            location.href = '/admin';
                        }

                        if (Vue.$cookies.get('user').role == "client") {
                            location.href = '/client';
                        }
                    },
                    error: function (error) {
                        console.log(error);
                        Vue.use(VueToast);
                        Vue.$toast.open({
                            message: error.responseJSON,
                            type: 'error'
                        });
                    }
                });

            },
        }
    }
</script>