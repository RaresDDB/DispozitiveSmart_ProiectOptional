<template>
    <div class="selected-space">
        <md-table v-model="people" md-card @md-selected="onSelect">
            <md-table-toolbar>
                <h1 class="md-title">Devices</h1>
                <button class="btn" v-on:click="activateAddForm">
                    <span class="material-symbols-outlined">
                        person_add
                    </span>
                </button>
            </md-table-toolbar>

            <md-table-row slot="md-table-row" slot-scope="{ item }" class="md-primary" md-selectable="single">
                <md-table-cell md-label="Name">{{ item.name }}</md-table-cell>
                <md-table-cell md-label="Description">{{ item.description }}</md-table-cell>
                <md-table-cell md-label="Address">{{ item.address }}</md-table-cell>
                <md-table-cell md-label="Consumption (max)">{{ item.maximumconsumption }}</md-table-cell>
                <md-table-cell md-label="Username">{{ item.username }}</md-table-cell>
                <md-table-cell md-label="Update">
                    <button class="btn" v-on:click="activateUpdateForm">
                        <span class="material-symbols-outlined">
                            settings
                        </span>
                    </button>
                </md-table-cell>
                <md-table-cell md-label="Delete">
                    <button class="btn-danger" v-on:click="deleteUser(item.id, item)">
                        <span class="material-symbols-outlined">
                            delete_forever
                        </span>
                    </button>
                </md-table-cell>
            </md-table-row>
        </md-table>

        <div v-if="validAddUserForm" class="selected-space">
            <form class="md-layout">
                <md-card class="md-layout-item md-size-50 md-small-size-100">
                    <md-card-header>
                        <div class="add-form-header">
                            <div class="md-title first-child-add-form">Add Device</div>
                            <div class="second-child-add-form">
                                <button class="btn" v-on:click="deactivateAddForm">
                                    <span class="material-symbols-outlined">
                                        close
                                    </span>
                                </button>
                            </div>
                        </div>

                    </md-card-header>

                    <md-card-content>
                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="device-name">Name</label>
                                    <md-input name="device-name" id="device-name" autocomplete="device-name" v-model="form.name" />
                                </md-field>
                            </div>
                        </div>

                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="description">Description</label>
                                    <md-input name="description" id="description" autocomplete="description" v-model="form.description" />
                                </md-field>
                            </div>
                        </div>

                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="device-address">Address</label>
                                    <md-input type="device-address" name="device-address" id="device-address" autocomplete="device-address" v-model="form.address" />
                                </md-field>
                            </div>
                        </div>

                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="device-consumption">Max Consumption</label>
                                    <md-input type="device-consumption" name="device-consumption" id="ddevice-consumption" autocomplete="device-consumption" v-model="form.maximumconsumption" />
                                </md-field>
                            </div>
                        </div>

                    </md-card-content>

                    <md-card-actions>
                        <div><input type="button" value="Create device" v-on:click="saveUser"></div>
                    </md-card-actions>
                </md-card>

            </form>
        </div>

        <div v-if="validUpdateUserForm" class="selected-space">
            <form class="md-layout">
                <md-card class="md-layout-item md-size-50 md-small-size-100">
                    <md-card-header>
                        <div class="add-form-header">
                            <div class="md-title first-child-add-form">Update Device</div>
                            <div class="second-child-add-form">
                                <button class="btn" v-on:click="deactivateUpdateForm">
                                    <span class="material-symbols-outlined">
                                        close
                                    </span>
                                </button>
                            </div>
                        </div>

                    </md-card-header>

                    <md-card-content>
                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="update-device-name">Name</label>
                                    <md-input name="update-device-name" autocomplete="name" v-model="updateform.name" />
                                </md-field>
                            </div>
                        </div>

                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="update-device-description">Description</label>
                                    <md-input name="update-device-description" id="update-device-description" autocomplete="description" v-model="updateform.description" />
                                </md-field>
                            </div>
                        </div>

                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="update-device-address">Address</label>
                                    <md-input name="update-device-address" id="update-device-description" autocomplete="address" v-model="updateform.address" />
                                </md-field>
                            </div>
                        </div>

                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="update-device-c">Maximum consumption</label>
                                    <md-input name="update-device-c" id="update-device-c" autocomplete="max-c" v-model="updateform.maximumconsumption" />
                                </md-field>
                            </div>
                        </div>

                    </md-card-content>

                    <md-card-actions>
                        <div><input type="button" value="Update user" v-on:click="updateUser"></div>
                    </md-card-actions>
                </md-card>
            </form>
        </div>
    </div>

</template>

<script>
    import Vue from 'vue';
    import VueToast from 'vue-toast-notification';
    import 'vue-toast-notification/dist/theme-sugar.css';
    export default {
        name: 'TableSingle',
        props: {
            devices: Array,
        },
        data() {
            return {
                selected: {},
                people: devices,
                form: {
                    name: null,
                    description: null,
                    address: null,
                    maximumconsumption: null,
                },
                updateform: {
                    name: null,
                    description: null,
                    address: null,
                    maximumconsumption: null,
                },
                validAddUserForm: false,
                validUpdateUserForm: false
            }
        },
        methods: {
            onSelect(item) {
                this.selected = item
                this.updateform.name = item.name.slice()
                this.updateform.description = item.description.slice()
                this.updateform.address = item.address.slice()
                this.updateform.maximumconsumption = item.maximumconsumption
            },
            saveUser: function () {

                let nameDevice = this.form.name;
                let descriptionDevice = this.form.description;
                let addressDevice = this.form.address;
                let maximumconsumptionDevice = this.form.maximumconsumption;
                this.form.name = null;
                this.form.description = null;
                this.form.address = null;
                this.form.maximumconsumption = null;

                $.ajax({
                    type: "POST",
                    url: '/api/DeviceApi/AddDevice',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    data: JSON.stringify({
                        Name: nameDevice,
                        Description: descriptionDevice,
                        Address: addressDevice,
                        MaximumConsumption: maximumconsumptionDevice,
                    }),
                    success: function (data) {
                        console.log(data)
                        this.people = devices;
                        this.people.push(data);
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
            },
            updateUser: function () {

                this.people = devices;
                let ok = 0;
                for (let i = 0; i < this.people.length && ok == 0; i++) {
                    if (JSON.stringify(this.people[i]) === JSON.stringify(this.selected)) {
                        this.people[i].name = this.updateform.name;
                        this.people[i].description = this.updateform.description;
                        this.people[i].address = this.updateform.address;
                        this.people[i].maximumconsumption = this.updateform.maximumconsumption;
                        ok = 1;
                    }
                }

                $.ajax({
                    type: "PUT",
                    url: '/api/DeviceApi/UpdateDevice',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    data: JSON.stringify({
                        Id: this.selected.id,
                        Name: this.updateform.name,
                        Description: this.updateform.description,
                        Address: this.updateform.address,
                        MaximumConsumption: this.updateform.maximumconsumption
                    }),
                    success: function (data) {
                        console.log(data)
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
            },
            deleteUser: function (code, myuser) {
                this.validAddUserForm = false;
                this.validUpdateUserForm = false;
                let idDevice = this.people.indexOf(myuser);
                this.people.splice(idDevice, 1);
                console.log(this.people);
                $.ajax({
                    type: "DELETE",
                    url: '/api/DeviceApi/DeleteDevice',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    data: JSON.stringify({
                        Id: code,
                    }),
                    success: function (data) {
                        console.log(data)
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
            },
            activateAddForm() {
                this.validAddUserForm = true
                this.validUpdateUserForm = false
            },
            deactivateAddForm() {
                this.validAddUserForm = false
            },
            activateUpdateForm() {
                this.validUpdateUserForm = true
                this.validAddUserForm = false
            },
            deactivateUpdateForm() {
                this.validUpdateUserForm = false
            }
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