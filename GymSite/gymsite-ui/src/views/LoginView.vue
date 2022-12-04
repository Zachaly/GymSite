<template>
    <div class="columns is-centered mt-2">
        <div class="column is-4">
            <div class="field">
                <label class="label">Username</label>
                <div class="control">
                    <input class="input" v-model="loginModel.username"/>
                </div>
            </div>
            <div class="field">
                <label class="label">Password</label>
                <div class="control">
                    <input class="input" type="password" v-model="loginModel.password"/>
                </div>
            </div>
            <div class="field">
                <button class="button is-success" @click="confirm">Create account</button>
            </div>
        </div>
    </div>
</template>

<script setup>
const { useFetchStore }=require("@/stores/fetchStore");
const { ref }=require("@vue/reactivity");

const fetchStore = useFetchStore()

const loginModel = ref({
    username: '',
    password: '',
})

function confirm(){
    fetchStore.post('auth/login', loginModel.value, r => { alert("Logged as " + r.data.userId) })
}
</script>