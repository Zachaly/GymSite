<template>
    <div class="columns is-centered">
        <div class="column is-half">
            <p class="title has-text-centered">{{article.title}}</p>
            <p class="subtitle has-text-centered">{{article.creatorName}}</p>
            <p class="subtitle has-text-centered">{{article.description}}</p>
            <p style="width: 100%; overflow-wrap: break-word; overflow: unset;">
                {{article.content}}
            </p>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue';
import { useFetchStore } from '@/stores/fetchStore';
import { useRoute } from 'vue-router';

const article = ref({})

useFetchStore().get('article/' + useRoute().params.id, res => article.value = res.data)
</script>

<style scoped>
p{
    width: 100%;
    overflow-wrap: break-word;
    overflow: unset;
}
</style>