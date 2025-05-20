<script setup lang="ts">
import { RouterView } from 'vue-router'

import NavBar from '@/components/NavBar.vue';
</script>

<template>
  <div class="flex h-screen w-screen flex-col">
    <NavBar />
    <RouterView class="h-full bg-brand-very-light-orange"/>
  </div>



</template>
