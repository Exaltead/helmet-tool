<script lang="ts" setup>
import { ref } from 'vue';
import TopBar from './TopBar.vue';


const navOpen = ref(false)

function openNav(){
  navOpen.value = true
}

</script>

<template>
  <div>
    <TopBar :onMenuClick="openNav"/>
    <div v-if="navOpen" class="bg-brand-primary inset-0 w-full h-screen md:w-40"></div>
    <slot></slot>
  </div>


</template>
