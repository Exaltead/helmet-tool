<script lang="ts" setup>
defineProps<{
  name: string,
  label: string,
  required: boolean
}>()

const model = defineModel()
</script>

<template>
  <div class="flex flex-col gap-2">
    <label v-bind:for="name"> {{ label }}</label>
    <input type="text" v-bind:required="required" v-model="model"
      class="rounded border border-brand-primary bg-light-gray" />
  </div>
</template>
