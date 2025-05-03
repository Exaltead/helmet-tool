<script lang="ts" setup>
import IconBack from "@/components/icons/IconBack.vue"
import IconDelete from "@/components/icons/IconDelete.vue"

type IconName = "back" | "delete"

defineProps<{
  onClick: () => void
  text?: string
  isSubmitting?: boolean
  icon?: IconName
  disabled?: boolean
}>()

function createButtonStyle(disabled: boolean){
  return disabled
    ? "bg-brand-disabled text-text-disabled cursor-not-allowed py-1 px-2"
    : "bg-brand-primary text-text-primary font-bold rounded cursor-pointer py-1 px-2"
}

</script>

<template>
  <button @click="onClick" :class="createButtonStyle(disabled ??false)" type="button" :disabled="disabled">
    <div class="flex flex-row gap-2 items-center">
      <div v-if="isSubmitting"
        class="bg-brand-primary text-text-primary mr-3 size-5 animate-spin rounded-full border-4 border-white border-t-transparent">
      </div>
      <div v-if="icon && icon === 'back'">
        <IconBack class="text-text-primary w-4 h-fit" />
      </div>
      <div v-if="icon && icon === 'delete'">
        <IconDelete class="text-text-primary w-4 h-fit" />
      </div>
      <span v-if="text" class="px-4">{{ text }}</span>
    </div>
  </button>
</template>
