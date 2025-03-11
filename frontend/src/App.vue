<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import TopBar from '@/components/TopBar.vue';
import NavProvider from '@/components/NavProvider.vue';
</script>

<template>
  <NavProvider>
    <RouterView />
  </NavProvider>


</template>
