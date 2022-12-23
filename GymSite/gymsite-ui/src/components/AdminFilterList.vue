<template>
    <div class="columns is-centered">
        <div class="column is-half">
            <table class="table is-fullwidth">
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                </tr>
                <tr v-for="filter in filters" :key="filter.id">
                    <td>{{filter.id}}</td>
                    <td>{{filter.name}}</td>
                    <td><button class="button is-danger" @click="remove(filter.id)">Remove</button></td>
                </tr>
            </table> 
            <div>
                <div class="field">
                    <label class="label">Name</label>
                    <div class="control">
                        <input class="input" v-model="newFilter.name"/>
                    </div>
                </div>
                <div class="field">
                    <button class="button is-success" @click="add">Add</button>
                </div>        
            </div>
        </div>
    </div>
</template>

<script setup>
import { useFetchStore } from '@/stores/fetchStore';
import { ref } from 'vue';

const fetchStore = useFetchStore()
const filters = ref([])
const newFilter = ref({
    name: ''
})

const loadData = () => fetchStore.get('exercise-filter', res => filters.value = res.data)

loadData()

function remove(id){
    fetchStore.delete('exercise-filter/' + id).then(() => filters.value = filters.value.filter(x => x.id !== id))
}

function add(){
    fetchStore.post('exercise-filter', newFilter.value, res => filters.value.push(res.data))
}

</script>