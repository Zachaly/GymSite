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
                <button class="button is-success" @click="confirm">Login</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useAuthStore } from "@/stores/authStore";
import { useRouter } from "vue-router";
const { ref }=require("@vue/reactivity");

const authStore = useAuthStore()
const router = useRouter()

const loginModel = ref({
    username: '',
    password: '',
})

function confirm(){
    authStore.login(loginModel.value).then(() => router.push('/'))
}
</script>