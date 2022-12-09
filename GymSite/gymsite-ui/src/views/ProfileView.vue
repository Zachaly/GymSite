<template>
    <div class="columns is-centered mt-1">
        <div class="column is-narrow has-text-centered">
            <p class="title mt-1">{{user.nickName}}</p>
            <p class="subtitle mt-1">Joined {{user.created}}</p>
            <table class="table is-centered">
                <tr>
                    <th>First name</th>
                    <td>{{user.firstName}}</td>
                </tr>
                <tr>
                    <th>Last name</th>
                    <td>{{user.lastName}}</td>
                </tr>
                <tr>
                    <th>Gender</th>
                    <td>{{$gender(user.gender)}}</td>
                </tr>
            </table>
            <button v-if="(authStore.userId === id)" class="button is-warning" @click="router.push('/user/update')">Update profile</button>
        </div>
    </div>
</template>

<script setup>
import { useRoute, useRouter } from "vue-router";
import { useFetchStore } from "@/stores/fetchStore";
import { useAuthStore } from "@/stores/authStore";
const { ref }=require('vue');

const fetchStore = useFetchStore()
const authStore = useAuthStore()
const router = useRouter()

const route = useRoute()

const id = route.params.id

const user = ref({})

fetchStore.get('user/' + id, res => user.value = res.data)

</script>