<!-- eslint-disable vue/multi-word-component-names -->
<script lang="ts" setup>
import IconBack from "@/components/icons/IconBack.vue"

type IconName = "back"

defineProps<{
  onClick: () => void
  text?: string
  isSubmitting?: boolean
  icon?: IconName
}>()


</script>

<template>
  <button @click="onClick" class="bg-brand-primary text-white font-bold rounded cursor-pointer py-1 px-2" type="button">
    <div class="flex flex-row gap-2 items-center">
      <div v-if="isSubmitting"
        class="bg-brand-primary text-white mr-3 size-5 animate-spin rounded-full border-4 border-white border-t-transparent">
      </div>
      <div v-if="icon && icon === 'back'" >
        <IconBack class="text-white w-4 h-fit"/>
      </div>
      <span v-if="text">{{ text }}</span>
    </div>
  </button>
</template>
