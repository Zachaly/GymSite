<template class="content">
    <div class="columns is-centered">
        <div class="column is-6">
            <table class="table is-fullwidth">
                <tr>
                    <th class="has-text-centered">Name</th>
                    <th class="has-text-centered">Go to</th>
                </tr>
                <tr v-for="exercise in exercises" :key="exercise.id">
                    <td class="has-text-centered is-vcentered">{{exercise.name}}</td>
                    <td class="has-text-centered"><router-link class="button" :to="'/exercise/' + exercise.id">Go to</router-link></td>
                    <td v-if="exercise.removable" class="hax-text-centered"><button @click="remove(exercise.id)" class="button is-danger is-fullwidth">Remove</button></td>
                </tr>
            </table>
            <button class="button is-success is-fullwidth" @click="router.push('/exercise/add')">Add exercise</button>
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

fetchStore.get('exercise/user/' + authStore.userId, res => exercises.value = res.data)

function remove(id){
    fetchStore.delete('exercise/' + id)
        .then(() => exercises.value = exercises.value.filter(x => x.id !== id))
}

</script>