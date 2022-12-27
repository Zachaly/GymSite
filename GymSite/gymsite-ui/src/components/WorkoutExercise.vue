<template>
    <div class="card m-1">
        <div class="card-content">
            <div class="media">
                <div class="media-content">
                    <p class="title has-text-centered">
                        {{ exercise.exerciseName }}
                    </p>
                    <p class="subtitle has-text-centered">{{ exercise.exerciseDescription }}</p>
                    <p class="has-text-centered"><button class="button is-danger" @click="deleteExercise">Remove</button></p>
                </div>
            </div>
            <div class="content">
                <table class="table">
                    <tr>
                        <th>Set</th>
                        <th>Reps</th>
                        <th>Weight</th>
                        <th></th>
                    </tr>
                    <tr v-for="set, index in exercise.sets" :key="set.id">
                        <td>{{ index + 1 }}</td>
                        <td>{{ set.reps }}</td>
                        <td>{{ set.weight }}</td>
                        <td><button class="button is-danger" @click="deleteSet(set.id)">Remove</button></td>
                    </tr>
                </table>
                <div class="columns is-vcentered">
                    <div class="column">
                        <label class="label">Reps</label>
                        <input class="input" v-model="setModel.reps">
                    </div>
                    <div class="column">
                        <label class="label">Weight</label>
                        <input class="input" v-model="setModel.weight">
                    </div>
                    <div class="column ">
                        <button class="button is-success is-fullwidth" @click="addSet">Add set</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useFetchStore } from '@/stores/fetchStore';
import { defineProps, ref, defineEmits } from 'vue';

const fetchStore = useFetchStore()

const props = defineProps({
    exercise: Object
})

const emit = defineEmits(['delete-exercise'])

const exercise = ref(props.exercise)

const setModel = ref({
    reps: 0,
    weight: 0.0,
    workoutExerciseId: exercise.value.id
})

function addSet(){
    fetchStore.post('exercise-set', setModel.value, res => exercise.value.sets.push(res.data))
    setModel.value.reps = 0
    setModel.value.weight = 0.0
}

function deleteExercise(){
    fetchStore.delete('workout-exercise/' + exercise.value.id)
    emit('delete-exercise', exercise.value.id)
}

function deleteSet(id){
    fetchStore.delete('exercise-set/' + id)
    exercise.value.sets = exercise.value.sets.filter(set => set.id != id)
}
</script>