<template class="content">
    <div class="columns is-centered">
        <div class="column is-6">
            <table class="table is-fullwidth">
                <tr>
                    <th class="has-text-centered">Name</th>
                    <th class="has-text-centered">Go to</th>
                    <th></th>
                </tr>
                <tr v-for="exercise in exercises" :key="exercise.id">
                    <td class="has-text-centered is-vcentered">{{exercise.name}}</td>
                    <td class="has-text-centered"><router-link class="button" :to="'/exercise/' + exercise.id">Go to</router-link></td>
                    <td v-if="exercise.removable" class="hax-text-centered"><button @click="remove(exercise.id)" class="button is-danger is-fullwidth">Remove</button></td>
                </tr>
            </table>
            <button class="button is-success is-fullwidth" @click="router.push('/exercise/add')">Add exercise</button>
        </div>
        <div class="column is-2 mt-5">
            <div class="field" v-for="filter in filters" :key="filter.id">
                <input type="checkbox" :value="filter.id" :id="filter.id" v-model="usedFilters" @change="get">
                <label :for="filter.id">{{ filter.name }}</label>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/authStore';
import { useFetchStore } from '@/stores/fetchStore';
import { useRouter } from 'vue-router';
import { ref } from 'vue';

const authStore = useAuthStore()
const router = useRouter()
const fetchStore = useFetchStore()

if(!authStore.authorized){
    alert('You must be logged in to go here!')
    router.push('/login')
}

const exercises = ref([])
const filters = ref([])

fetchStore.get('exercise-filter', res => filters.value = res.data)
fetchStore.get('exercise/user/' + authStore.userId, res => exercises.value = res.data)

const usedFilters = ref([])

function get(){
    if(usedFilters.value.length > 0){
        fetchStore.getWithParams('exercise/filter', { userId: authStore.userId, filterIds: usedFilters.value }, res => exercises.value = res.data)
    } else {
        fetchStore.get('exercise/user/' + authStore.userId, res => exercises.value = res.data)
    }
}

function remove(id){
    fetchStore.delete('exercise/' + id)
        .then(() => exercises.value = exercises.value.filter(x => x.id !== id))
}

</script>

<style scoped>
    input{
        margin-right: 5px;
    }
</style>