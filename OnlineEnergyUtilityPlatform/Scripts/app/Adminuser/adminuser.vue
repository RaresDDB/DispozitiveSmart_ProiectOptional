<template>
    <div class="selected-space">
        <md-table v-model="people" md-card @md-selected="onSelect">
            <md-table-toolbar>
                <h1 class="md-title">Users</h1>
                <button class="btn" v-on:click="activateAddForm">
                    <span class="material-symbols-outlined">
                        person_add
                    </span>
                </button>
            </md-table-toolbar>

            <md-table-row slot="md-table-row" slot-scope="{ item }" class="md-primary" md-selectable="single">
                <md-table-cell md-label="ID" md-sort-by="id">{{ item.id }}</md-table-cell>
                <md-table-cell md-label="Username" md-sort-by="name">{{ item.username }}</md-table-cell>
                <md-table-cell md-label="Email" md-sort-by="email">{{ item.email }}</md-table-cell>
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
                            <div class="md-title first-child-add-form">Add User</div>
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
                                    <label for="first-name">Username</label>
                                    <md-input name="first-name" id="first-name" autocomplete="given-name" v-model="form.username" />
                                </md-field>
                            </div>
                        </div>

                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="email">Email</label>
                                    <md-input name="email" id="email" autocomplete="email" v-model="form.email" />
                                </md-field>
                            </div>
                        </div>

                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="form-password">Password</label>
                                    <md-input type="password" name="form-password" id="form-password" autocomplete="form-password" v-model="form.password" />
                                </md-field>
                            </div>
                        </div>

                    </md-card-content>

                    <md-card-actions>
                        <div><input type="button" value="Create user" v-on:click="saveUser"></div>
                    </md-card-actions>
                </md-card>

            </form>
        </div>

        <div v-if="validUpdateUserForm" class="selected-space">
            <form class="md-layout">
                <md-card class="md-layout-item md-size-50 md-small-size-100">
                    <md-card-header>
                        <div class="add-form-header">
                            <div class="md-title first-child-add-form">Update User</div>
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
                                    <label for="update-user-name">Username</label>
                                    <md-input name="update-user-name" autocomplete="username" v-model="updateform.username" />
                                </md-field>
                            </div>
                        </div>

                        <div class="md-layout md-gutter">
                            <div class="md-layout-item md-small-size-100">
                                <md-field>
                                    <label for="email">Email</label>
                                    <md-input name="email" id="email" autocomplete="email" v-model="updateform.email" />
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
            users: Array,
        },
        data() {
            return {
                selected: {},
                people: users,
                form: {
                    username: null,
                    email: null,
                    password: null
                },
                updateform: {
                    username: null,
                    email: null,
                },
                validAddUserForm: false,
                validUpdateUserForm: false
            }    
        },
        methods: {
            onSelect(item) {
                this.selected = item
                this.updateform.username = item.username.slice()
                this.updateform.email = item.email.slice()
            },
            saveUser: function () {

                let usernameUser = this.form.username;
                let emailUsername = this.form.email;
                let passwordUsername = this.form.password;
                this.form.username = null;
                this.form.email = null;
                this.form.password = null;

                $.ajax({
                    type: "POST",
                    url: '/api/UserApi/Register',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    data: JSON.stringify({
                        Username: usernameUser,
                        Email: emailUsername,
                        Password: passwordUsername,
                    }),
                    success: function (data) {
                        console.log(data)
                        this.people = users;
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
                $.ajax({
                    type: "PUT",
                    url: '/api/UserApi/UpdateUser',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    data: JSON.stringify({
                        Id: this.selected.id,
                        Username: this.updateform.username,
                        Email: this.updateform.email,
                    }),
                    success: function (data) {
                        console.log(data)
                        this.people = users;
                        let ok = 0;
                        for (let i = 0; i < this.people.length && ok == 0; i++) {
                            if (this.people[i].id.localeCompare(data.id) == 0) {
                                this.people[i].username = data.username;
                                this.people[i].email = data.email;
                                ok = 1;
                            }
                        }
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
                let idUser = this.people.indexOf(myuser);
                this.people.splice(idUser, 1);
                console.log(this.people);
                $.ajax({
                    type: "DELETE",
                    url: '/api/UserApi/DeleteUser',
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