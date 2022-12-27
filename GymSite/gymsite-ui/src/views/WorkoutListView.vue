<template>
    <div class="columns is-centered ">
        <div class="column is-4 mt-2">
            <div class="columns is-vcentered" v-for="workout in workouts" :key="workout.id">
                <div class="column">
                    <p class="subtitle has-text-centered">
                        <router-link :to="'workout/' + workout.id" class="subtitle has-text-centered">{{ workout.name }}</router-link>
                    </p>
                </div>
                <div class="column"><button class="button is-danger" @click="deleteWorkout(workout.id)">Remove</button></div>
            </div>
            <router-link to="/workout/add" class="button is-success has-text-centered is-fullwidth">Add workout</router-link>
        </div>
    </div>
    
</template>

<script setup>
import { useAuthStore } from '@/stores/authStore';
import { useFetchStore } from '@/stores/fetchStore';
import { ref } from 'vue';

const fetchStore = useFetchStore()
const authStore = useAuthStore()

const workouts = ref([])

fetchStore.getWithParams('workout', { userId: authStore.userId }, res => workouts.value = res.data)

function deleteWorkout(id){
    fetchStore.delete('workout/' + id)
    workouts.value = workouts.value.filter(workout => workout.id != id)
}

</script>