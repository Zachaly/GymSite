<template>
    <div class="columns is-centered mt-2">
        <div class="column is-4">
            <div class="field">
                <label class="label">Username</label>
                <div class="control">
                    <input class="input" v-model="registerModel.username"/>
                </div>
            </div>
            <div class="field">
                <label class="label">Email</label>
                <div class="control">
                    <input class="input" v-model="registerModel.email"/>
                </div>
            </div>
            <div class="field">
                <label class="label">Password</label>
                <div class="control">
                    <input class="input" type="password" v-model="registerModel.password"/>
                </div>
            </div>
            <div class="field">
                <label class="label">Nickname</label>
                <div class="control">
                    <input class="input" v-model="registerModel.nickName"/>
                </div>
            </div>
            <div class="field">
                <label class="label">First name</label>
                <div class="control">
                    <input class="input" v-model="registerModel.firstName"/>
                </div>
            </div>
            <div class="field">
                <label class="label">Last name</label>
                <div class="control">
                    <input class="input" v-model="registerModel.lastName"/>
                </div>
            </div>
            <div class="field">
                <label class="label">Gender</label>
                <div class="control">
                    <label class="radio">
                        <input type="radio" @change="changeGender(0)">
                        Don't want to specify
                    </label>
                    <label class="radio">
                        <input type="radio" @change="changeGender(1)">
                        Male
                    </label>
                    <label class="radio">
                        <input type="radio" @change="changeGender(2)">
                        Female
                    </label>
                </div>
            </div>
            <div class="field">
                <button class="button is-success" @click="confirm">Create account</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useFetchStore } from "@/stores/fetchStore";
import { ref } from "@vue/reactivity";
import { useRouter } from "vue-router";


const fetchStore = useFetchStore()

const router = useRouter()

const registerModel = ref({
    username: '',
    email: '',
    password: '',
    nickName: '',
    firstName: '',
    lastName: '',
    gender: 0,
})

function changeGender(value){
    registerModel.value.gender = value
}

function confirm(){
    fetchStore.postNoContent('auth/register', registerModel.value)
    .then(() => router.push('/login'))
}
</script>