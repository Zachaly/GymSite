<template>
  <NavBar/>
  <div class="container mt-1">
    <LoadingBar v-if="fetchStore.loading"/>
    <router-view v-else/>
  </div>

</template>

<script setup>
import LoadingBar from "./components/LoadingBar.vue";
import NavBar from "./components/NavBar.vue"

const { useFetchStore }=require("./stores/fetchStore");

const fetchStore = useFetchStore()

</script>
