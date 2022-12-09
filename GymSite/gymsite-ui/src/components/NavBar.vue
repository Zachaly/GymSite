<template>
    <nav class="navbar" role="navigation" aria-label="main navigation">
        <div class="container">
            <div class="navbar-menu">
                <div v-if="!authStore.authorized" class="navbar-end" >
                    <router-link to="/login" class="navbar-item">Login</router-link>
                    <router-link to="/register" class="navbar-item">Register</router-link>
                </div>
                <div v-else class="navbar-end" >
                    <p>xd</p>
                    <router-link :to="`/user/${authStore.userId}`" class="navbar-item">Profile</router-link>
                </div>
            </div>
        </div>
    </nav>
</template>


<script setup>
import { useAuthStore } from '@/stores/authStore';

const authStore = useAuthStore()

</script>