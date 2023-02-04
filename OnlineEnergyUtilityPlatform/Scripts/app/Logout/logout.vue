<template>
    <div class="authenticate-button-container">
        <img class="logout-image" id="logoutIcon" v-on:click="handleSignOut" src="https://cdn-icons-png.flaticon.com/512/126/126467.png" alt="Logout" />
    </div>
</template>

<script>
    export default {
        methods: {
            handleSignOut: function () {
                $.ajax({
                    type: "POST",
                    url: '/api/UserApi/logout',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    success: function () {
                        console.log("Success log out!")
                        location.href = '/'
                    },
                    error: function (error) {
                        console.log(error)
                    }
                });
            }
        }
    }

</script>