<template>
    <div class="selected-space">
        <md-table v-model="userDevices" md-card @md-selected="onSelect">
            <md-table-toolbar>
                <h1 class="md-title">Devices</h1>
            </md-table-toolbar>
            <md-table-row slot="md-table-row" slot-scope="{ item }" class="md-primary" md-selectable="single">
                <md-table-cell md-label="Name">{{ item.name }}</md-table-cell>
                <md-table-cell md-label="Description">{{ item.description }}</md-table-cell>
                <md-table-cell md-label="Address">{{ item.address }}</md-table-cell>
                <md-table-cell md-label="Consumption (max)">{{ item.maximumconsumption }}</md-table-cell>
            </md-table-row>
        </md-table>
        <div>
            {{message}}
        </div>
    </div>
</template>

<script>
    import Vue from 'vue';
    import VueCookies from 'vue-cookies';
    const signalR = require("@microsoft/signalr");
    export default {
        name: 'TableSingle',
        props: {
            cdevices: Array,
        },
        data() {
            return {
                selected: {},
                userDevices: cdevices,
                connection: null,
                message: "sall",
            }
        },
        methods: {
            onSelect(item) {
                this.selected = item
                Vue.use(VueCookies);
                Vue.$cookies.set('deviceId', this.selected.id);
                window.location.href = '/client/measurements';
            },
        },
        /*sockets: {

            // Equivalent of
            // signalrHubConnection.on('someEvent', (data) => this.someActionWithData(data))
            receiveMessage(data) {
                this.message = data;
            },

        },*/
        created: function () {

            /*this.$socket.start({
                log: true // Logging is optional but very helpful during development
            })*/
        },
        mounted: function () {
           /* let connection = new signalR.HubConnectionBuilder()
                .withUrl('https://localhost:7115/message', { accessTokenFactory: () => this.loginToken })
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Trace)
                .build();
            connection.serverTimeoutInMilliseconds = 120000;
            connection.on("receive", data => {
                console.log(data);
                //this.message = "test";
            });
            connection.start()
                .then(() => {
                    /*this.connection.on('ReceiveMessage', async function (data) {
                        console.log("sall2");
                        this.message = "test";
                    });*/
               //     console.log("connection started");
               // })
              //  .catch(err => console.log("connecting hub failed err is : ", err));
        }
    }
</script>

<style lang="scss" scoped>
    .md-table + .md-table {
        margin-top: 16px
    }

    .md-progress-bar {
        position: absolute;
        top: 0;
        right: 0;
        left: 0;
    }
</style>