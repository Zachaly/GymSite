<template>
    <div class="columns is-centered mt-2">
        <div class="column is-4">
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
            <div class="field" v-for="filter in filters" :key="filter.id">
                <input type="checkbox" :value="filter.id" :id="filter.id" v-model="exerciseModel.filterIds">
                <label :for="filter.id">{{ filter.name }}</label>
            </div>
            <div class="field">
                <button class="button is-success" @click="confirm">Add</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/authStore';
import { useFetchStore } from '@/stores/fetchStore';
import { ref } from '@vue/reactivity';
import { useRouter } from 'vue-router';

const authStore = useAuthStore()
const fetchStore = useFetchStore()
const router = useRouter()
const filters = ref([])

const exerciseModel = ref({
    userId: authStore.userId,
    description: '',
    name: '',
    filterIds: []
})

fetchStore.get('exercise-filter', res => filters.value = res.data)

const confirm = () => {
    fetchStore.postNoContent('exercise', exerciseModel.value)
    .then(() => router.push('/exercise'))
}

</script>