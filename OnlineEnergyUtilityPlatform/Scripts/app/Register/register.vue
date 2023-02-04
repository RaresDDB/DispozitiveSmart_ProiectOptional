<template>
    <div>
        <form name="registerForm">
            <input type="text" id="username" name="username" placeholder="Username" v-model:value="selectedUsername" class="authenticate-input">
            <input type="text" id="email" name="email" placeholder="Email" v-model:value="selectedEmail" class="authenticate-input">
            <input type="password" id="pwd" name="pwd" placeholder="Password" v-model:value="selectedPassword" class="authenticate-input pwd-checker">
            <input type="password" id="cpwd" name="cpwd" placeholder="Confirm Password" v-model:value="selectedConfirmPassword" class="authenticate-input">
            <span id="passwordMatchMessage" class="register-validation-input" v-if="showPasswordMatchMsg">{{passwordMatchMsg}}</span>
            <div class="authenticate-button-container">
                <button type="button" class="btn btn-secondary authenticate-button" id="register-button" v-on:click="verifyInput">Register</button>
            </div>

        </form>
    </div>
</template>

<script>
    import Vue from 'vue';
    import VueToast from 'vue-toast-notification';
    import 'vue-toast-notification/dist/theme-sugar.css';
    export default {
        data() {
            return {
                selectedUsername: '',
                selectedEmail: '',
                selectedPassword: '',
                selectedConfirmPassword: '',
                passwordMatchMsg: '',
                showPasswordMatchMsg: false,
            };
        },
        methods: {
            verifyInput: function () {

                let cpasswordInput = document.getElementById('cpwd');

                if (this.selectedPassword != this.selectedConfirmPassword) {
                    this.passwordMatchMsg = 'Passwords did not match!';
                    this.showPasswordMatchMsg = true;
                    cpasswordInput.style.margin = '0';
                    cpasswordInput.style.borderColor = '#DA394B';
                }
                else {
                    this.showPasswordMatchMsg = false;
                    cpasswordInput.style.margin = '0 0 2rem 0';
                    cpasswordInput.style.borderColor = 'darkgrey';
                }

                if (this.showPasswordMatchMsg) {
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: '/api/UserApi/Register',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    data: JSON.stringify({
                        Username: this.selectedUsername,
                        Email: this.selectedEmail,
                        Password: this.selectedPassword,
                    }),
                    success: function (data) {
                        console.log(data);
                        location.href = "/login";
                    },
                    error: function (error) {
                        console.log(error)
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