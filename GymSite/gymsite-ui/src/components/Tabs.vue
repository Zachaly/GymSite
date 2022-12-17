<template>
    <div class="tabs is-centered">
        <ul>
            <li 
            v-for="(tab, index) in props.tabs" :key="tab" 
            :class="{'is-active': currentIndex === index}"
            >
                <a @click="changeTab(index)">{{tab}}</a>
            </li>
        </ul>
    </div>
</template>

<script>
export default {
    name: 'tabs-part'
}
</script>

<script setup>
import { defineProps, defineEmits, ref } from 'vue';

const props = defineProps({
    tabs: Array
})

const currentIndex = ref(0)

const emit = defineEmits(['change-tab'])

function changeTab(index){
    currentIndex.value = index
    emit('change-tab', index)
}

</script>