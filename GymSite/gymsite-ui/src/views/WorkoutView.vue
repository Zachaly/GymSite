<template>
    <div class="columns is-centered">
        <div class="column is-half">
            <p class="title has-text-centered">{{ workout.name }}</p>
            <p class="subtitle has-text-centered">{{ workout.description }}</p>
            <div>
                <div>
                    <div class="select">
                        <select @change="pickExercise">
                            <option v-for="exercise in exercises" :key="exercise.id" :value="exercise.id">{{ exercise.name }}</option>
                        </select>
                    </div>
                    <button @click="addExercise" class="button is-success">Add exercise</button>
                </div>
                <WorkoutExercise v-for="exercise in workout.exercices" :key="exercise.id" :exercise="exercise" @delete-exercise="deleteExercise"/>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useFetchStore } from '@/stores/fetchStore';
import { useRoute } from 'vue-router';
import { ref } from 'vue';
import { useAuthStore } from '@/stores/authStore';
import WorkoutExercise from '@/components/WorkoutExercise.vue';

const fetchStore = useFetchStore()
const authStore = useAuthStore()

const id = useRoute().params.id

const workout = ref({})
const exercises = ref([])
const pickedExerciseId = ref(0)

fetchStore.get('workout/' + id, res => workout.value = res.data)
fetchStore.get('exercise/user/' + authStore.userId, res => exercises.value = res.data)

function pickExercise(e){
    pickedExerciseId.value = e.target.value
}

function addExercise(){
    console.log(workout.value)
    fetchStore.post('workout-exercise', { workoutId: id, exerciseId: pickedExerciseId.value }, res => workout.value.exercices.push(res.data))
    pickedExerciseId.value = 0
}

function deleteExercise(id){
    workout.value.exercices = workout.value.exercices.filter(ex => ex.id != id)
}
</script>