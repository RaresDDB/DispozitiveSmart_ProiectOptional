<template>
    <div>
        <div class="md-layout md-gutter">
            <div class="md-layout-item" style="display: flex; align-items: center; justify-content:center">
                <div>
                    <span class="my-account-label">User: </span>
                    <select id="allocateUserId" name="allocateUser" v-model:value="selectedUser" class="authenticate-input">
                        <option v-bind:value="user" v-for="user in dataUsers">
                            <label>{{user.username}}</label>
                        </option>
                    </select>
                </div>
            </div>
            <div class="md-layout-item" style="display: flex; align-items: center; justify-content:center">
                <div>
                    <span class="my-account-label">Device: </span>
                    <select id="allocateDeviceId" name="allocateDevice" v-model:value="selectedDevice" class="authenticate-input">
                        <option v-bind:value="device" v-for="device in dataDevices">
                            <label>{{device.name}}</label>
                        </option>
                    </select>
                </div>
            </div>
        </div>
        <div class="allocate-button-cantainer">
            <md-button typeof="submit" class="md-primary md-raised" v-on:click="allocateUserDevice">Allocate</md-button>
        </div>
    </div>
</template>

<script>
    import Vue from 'vue';
    import VueToast from 'vue-toast-notification';
    import 'vue-toast-notification/dist/theme-sugar.css';
    export default {
        name: 'BasicSelect',
        props: {
            ausers: Array,
            adevices: Array,
        },
        data: () => ({
            dataUsers: ausers,
            dataDevices: adevices,
            selectedUser: null,
            selectedDevice: null,
        }),
        methods: {
            allocateUserDevice: function () {
                var userId = this.selectedUser === null ? null : JSON.parse(JSON.stringify(this.selectedUser)).id
                var deviceId = this.selectedDevice === null ? null : JSON.parse(JSON.stringify(this.selectedDevice)).id
                if (!userId || !deviceId) {
                    Vue.use(VueToast);
                    Vue.$toast.open({
                        message: 'Please complete all fields!!',
                        type: 'error'
                    });
                }
                else {
                    $.ajax({
                        type: "PUT",
                        url: '/api/DeviceApi/AllocateUserToDevice',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        },
                        data: JSON.stringify({
                            UserId: userId,
                            DeviceId: deviceId
                        }),
                        success: function (data) {
                            console.log(data)
                            location.href = './Devices'
                        },
                        error: function (error) {
                            console.log(error)
                            Vue.use(VueToast);
                            if (error.responseJSON.type) {
                                Vue.$toast.open({
                                    message: 'Empty inputs!',
                                    type: 'error'
                                });
                            }
                            else {
                                Vue.$toast.open({
                                    message: error.responseJSON,
                                    type: 'error'
                                });
                            }
                        }
                    });
                }
            }
        }
    }
</script>