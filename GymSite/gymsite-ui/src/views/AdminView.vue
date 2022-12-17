<template>
    <Tabs :tabs="['exercise']" @change-tab="changeTab(num)"/>
    <div v-if="currentIndex == 0">
        <AdminExerciseList/>
    </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/authStore';
import { useRouter } from 'vue-router';
import Tabs from '@/components/Tabs.vue'
import AdminExerciseList from '@/components/AdminExerciseList.vue'
import { ref } from '@vue/reactivity';

const authStore = useAuthStore()
const router = useRouter()
const currentIndex = ref(0)

if(!authStore.claims.includes('Admin')){
    router.push('/')
}

function changeTab(num){
    currentIndex.value = num
}

</script>