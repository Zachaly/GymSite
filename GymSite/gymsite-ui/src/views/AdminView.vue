<template>
    Admin
</template>

<script setup>
import { useAuthStore } from '@/stores/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()
const router = useRouter()

if(!authStore.claims.includes('Admin')){
    router.push('/')
}

</script>