<template>
    <nav class="navbar" role="navigation" aria-label="main navigation">
        <div class="container">
            <div class="navbar-menu">
                <div v-if="!authStore.authorized" class="navbar-start">
                    <router-link to="/" class="navbar-item">Home</router-link>
                </div>
                <div v-else class="navbar-start">
                    <router-link to="/" class="navbar-item">Home</router-link>
                    <router-link to="/exercise" class="navbar-item">Exercises</router-link>
                    <router-link to="/workout" class="navbar-item">Workouts</router-link>
                    <router-link to="/article/add" class="navbar-item">Add article</router-link>
                </div>
                <div v-if="!authStore.authorized" class="navbar-end" >
                    <router-link to="/login" class="navbar-item">Login</router-link>
                    <router-link to="/register" class="navbar-item">Register</router-link>
                </div>
                <div v-else class="navbar-end" >
                    <router-link :to="`/user/${authStore.userId}`" class="navbar-item">Profile</router-link>
                    <a class="navbar-item" @click="logout">Logout</a>
                    <router-link v-if="authStore.claims.includes('Admin')" to="/admin" class="navbar-item">Admin</router-link>
                </div>
            </div>
        </div>
    </nav>
</template>


<script setup>
import { useAuthStore } from '@/stores/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()

const router = useRouter()

function logout() {
    authStore.logout()
    router.push('/login')
}

</script>