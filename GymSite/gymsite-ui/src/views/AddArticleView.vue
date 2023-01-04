<template>
    <div class="columns is-centered">
        <div class="column is-half">
            <div class="field">
                <label class="label">Title</label>
                <input class="input" v-model="articleModel.title">
            </div>
            <div class="field">
                <label class="label">Description</label>
                <textarea rows="2" class="textarea" v-model="articleModel.description"></textarea>
            </div>
            <div class="field">
                <label class="label">Content</label>
                <textarea rows="20" class="textarea" v-model="articleModel.content"></textarea>
            </div>
            <div class="field">
                <button class="button is-success" @click="add">Add article</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { useFetchStore } from '@/stores/fetchStore';
import { useAuthStore } from '@/stores/authStore';
import { ref } from '@vue/reactivity';
import { useRouter } from 'vue-router';

const fetchStore = useFetchStore()
const router = useRouter()

const articleModel = ref({
    creatorId: useAuthStore().userId,
    content: '',
    description: '',
    title: ''
})

function add(){
    fetchStore.postNoContent('article', articleModel.value)
    .then(() => router.push('/'))
}
</script>