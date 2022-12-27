<template>
    <div class="columns is-centered">
        <div class="column is-4">
            <div class="field">
                <label class="label">Name</label>
                <input class="input" v-model="workoutModel.name">
            </div>
            <div class="field">
                <label class="label">Description</label>
                <input class="input" v-model="workoutModel.description">
            </div>
            <div class="field">
                <button class="button" @click="confirm">Confirm</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/authStore';
import { useFetchStore } from '@/stores/fetchStore';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const fetchStore = useFetchStore()
const authStore = useAuthStore()
const router = useRouter()

const workoutModel = ref({
    name: '',
    description: '',
    userId: authStore.userId
})

function confirm(){
    fetchStore.postNoContent('workout', workoutModel.value).then(() => router.push('/workout'))
}

</script>