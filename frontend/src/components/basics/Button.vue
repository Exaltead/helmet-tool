<!-- eslint-disable vue/multi-word-component-names -->
<script lang="ts" setup>

defineProps<{
  onClick: () => void
  display?: string
}>()


</script>

<template>
  <button @click="onClick" class="bg-brand-primary text-white font-bold rounded cursor-pointer py-1 px-2">
    <slot>{{ display }}</slot>
  </button>
</template>
