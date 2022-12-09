<template>
     <div class="columns is-centered mt-2">
        <div class="column is-4">
            <div class="field">
                <label class="label">Nickname</label>
                <div class="control">
                    <input class="input" v-model="user.nickName"/>
                </div>
            </div>
            <div class="field">
                <label class="label">First name</label>
                <div class="control">
                    <input class="input" v-model="user.firstName"/>
                </div>
            </div>
            <div class="field">
                <label class="label">Last name</label>
                <div class="control">
                    <input class="input" v-model="user.lastName"/>
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
                <button class="button is-success" @click="confirm">Update</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/authStore';
import { useFetchStore } from '@/stores/fetchStore';
import { ref } from '@vue/reactivity';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()
const router = useRouter()
const fetchStore = useFetchStore()

if(!authStore.authorized){
    alert('You must be logged in to go here!')
    router.push('/login')
}

const user = ref({})

console.log(fetchStore.token)

fetchStore.get('user/' + authStore.userId, 
    res => user.value = { 
        nickName: res.data.nickName,
        firstName: res.data.firstName,
        lastName: res.data.lastName,
        gender: res.data.gender
    })

function changeGender(num){
    user.value.gender = num
}

function confirm(){
    fetchStore.put('user', user.value).then(() => router.push('/user/' + authStore.userId))
}
</script>