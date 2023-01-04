<template>
    <p class="title has-text-centered mt-2">New articles</p>
    <div class="columns is-multiline">
        <div class="column is-3" v-for="article in articles" :key="article.id">
            <ArticleItem :article="article"/>
        </div>
    </div>
</template>

<script setup>
import ArticleItem from '@/components/ArticleItem.vue';
import { useFetchStore } from '@/stores/fetchStore';
import { ref } from 'vue';

const articles = ref([])

useFetchStore().getWithParams('article', { pageSize: 8, pageIndex: 0 }, res => articles.value = res.data)


</script>