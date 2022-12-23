<template>
    <div class="columns is-centered">
        <div class="column is-4 has-text-centered">
            <p class="title">{{ exercise.name }}</p>
            <p class="subtitle">{{ exercise.description }}</p>
            <p class="subtitle"><span v-for="filter in exercise.filters" :key="filter">{{ filter }}</span></p>
            <table class="table is-fullwidth">
                <label class="title has-text-centered">Records</label>
                <tr>
                    <th>Record</th>
                </tr>
                <tr v-for="record in exercise.records" :key="record.id">
                    <td>{{record.weight}} x {{record.reps}}</td>
                </tr>
            </table>
            <div class="columns">
                <div class="column">
                    <input placeholder="weigth" class="input" v-model="recordModel.weight"/>
                </div>
                <div class="column">
                    <input placeholder="reps" class="input" v-model="recordModel.reps"/>
                </div>
                <div class="column">
                    <button class="button is-success" @click="addRecord">Add record</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useRoute } from 'vue-router';
import { useFetchStore } from '@/stores/fetchStore';
import { reactive, ref } from '@vue/reactivity';
import { useAuthStore } from '@/stores/authStore';

const id = useRoute().params.id

const fetchStore = useFetchStore()
const authStore = useAuthStore()

const exercise = ref({})
const recordModel = reactive({
    reps: '',
    weight: ''
})

fetchStore.getWithParams('exercise/' + id, { userId: authStore.userId }, res => exercise.value = res.data)


function addRecord(){
    fetchStore.post('exercise-record', { 
        reps: parseInt(recordModel.reps),
        weight: parseFloat(recordModel.weight),
        exerciseId: id,
        userId: authStore.userId
    }, res => exercise.value.records.push(res.data))
    recordModel.reps = ''
    recordModel.weight = ''
}

</script>


<style scoped>
    span{
        margin: 5px;
    }
</style>