<template>
    <div class="card p-4">
        <div class="card-content">
            <div class="media-content">
                <router-link :to="'/article/' + article.id" class="title">{{article.title}}</router-link>
                <p class="subtitle mt-2">{{article.creatorName}}</p>
                <p class="subtitle">{{article.description}}</p>
            </div>
        </div>
    </div>
</template>

<script setup>
import { defineProps } from 'vue';

// eslint-disable-next-line no-unused-vars
const props = defineProps({
    article: Object
})

</script>