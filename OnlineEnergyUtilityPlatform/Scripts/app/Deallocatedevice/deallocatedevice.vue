<template>
    <div>
        <div class="md-layout md-gutter">
            <div class="md-layout-item" style="display: flex; align-items: center; justify-content: center">
                <div>
                    <span class="my-account-label">Device: </span>
                    <select id="deallocateDeviceId" name="deallocateDevice" v-model:value="selectedDevice" class="authenticate-input">
                        <option v-bind:value="device" v-for="device in dataDevices">
                            <label>{{device.name}}</label>
                        </option>
                    </select>
                </div>
            </div>
        </div>
        <div class="allocate-button-cantainer">
            <md-button typeof="submit" class="md-primary md-raised" v-on:click="deallocateUserDevice">Deallocate</md-button>
        </div>
    </div>
</template>

<script>
    import Vue from 'vue';
    import VueToast from 'vue-toast-notification';
    import 'vue-toast-notification/dist/theme-sugar.css';
    export default {
        name: 'BasicSelectD',
        props: {
            ddevices: Array,
        },
        data: () => ({
            dataDevices: ddevices,
            selectedDevice: null,
        }),
        methods: {
            deallocateUserDevice: function () {
                var deviceId = this.selectedDevice === null ? null : JSON.parse(JSON.stringify(this.selectedDevice)).id
                if (!deviceId) {
                    Vue.use(VueToast);
                    Vue.$toast.open({
                        message: 'Please select a device!',
                        type: 'error'
                    });
                }
                else {
                    $.ajax({
                        type: "PUT",
                        url: '/api/DeviceApi/DeallocateUserToDevice',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        },
                        data: JSON.stringify({
                            Id: deviceId,
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