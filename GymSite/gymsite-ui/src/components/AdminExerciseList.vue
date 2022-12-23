<template>
    <div class="columns is-centered">
        <div class="column is-half">
            <table class="table is-fullwidth">
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                </tr>
                <tr v-for="exercise in exercises" :key="exercise.id">
                    <td>{{exercise.id}}</td>
                    <td>{{exercise.name}}</td>
                    <td><button class="button is-danger" @click="remove(exercise.id)">Remove</button></td>
                </tr>
            </table> 
            <div>
                <div class="field">
                    <label class="label">Name</label>
                    <div class="control">
                        <input class="input" v-model="exerciseModel.name"/>
                    </div>
                </div>
                <div class="field">
                    <label class="label">Description</label>
                    <div class="control">
                        <textarea class="input" v-model="exerciseModel.description"></textarea>
                    </div>
                </div>
                <div class="field checkboxes">
                    <div class="checkbox" v-for="filter in filters" :key="filter.id">
                        <input type="checkbox" :value="filter.id" :id="filter.id" v-model="exerciseModel.filterIds">
                        <label :for="filter.id">{{ filter.name }}</label>
                    </div>
                </div>
                <div class="field">
                    <button class="button is-success" @click="confirm">Add</button>
                </div>        
            </div>
        </div>
    </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/authStore';
import { useFetchStore } from '@/stores/fetchStore';
import { ref } from '@vue/reactivity';

const fetchStore = useFetchStore()
const authStore = useAuthStore()
const exercises = ref([])
const exerciseModel = ref({
    name: '',
    description: '',
    userId: authStore.userId,
    filterIds: []
})
const filters = ref([])

fetchStore.get('exercise/default', res => exercises.value = res.data)
fetchStore.get('exercise-filter', res => filters.value = res.data)

function confirm(){
    fetchStore.postNoContent('exercise/default', exerciseModel.value)
        .then(() => fetchStore.get('exercise/default', res => exercises.value = res.data))
    exerciseModel.value.name = '',
    exerciseModel.value.description = ''
}

function remove(id){
    fetchStore.delete('exercise/' + id)
        .then(() => exercises.value = exercises.value.filter(x => x.id !== id))
}

</script>

<style scoped>
    .checkboxes{
        display: flex;
    }
    .checkbox{
        margin: 5px;
    }
</style>