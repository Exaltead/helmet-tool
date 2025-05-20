<script lang="ts" setup>
import IconBack from "@/components/icons/IconBack.vue"
import IconDelete from "@/components/icons/IconDelete.vue"
import IconPlus from "@/components/icons/IconPlus.vue"

type IconName = "back" | "delete" | "plus"

type ButtonStyle = {
  isPill?: boolean
  backgroundColor?: "primary" | "warm-white"
  bold?: boolean
}

defineProps<{
  onClick: () => void
  text?: string
  isSubmitting?: boolean
  icon?: IconName
  disabled?: boolean
  styling?: ButtonStyle
}>()

function createButtonStyle(disabled: boolean, style: ButtonStyle | undefined) {
  let baseStyle = "py-1 pl-2 pr-3"

  let backgroundColor = "bg-brand-primary"
  if (style?.backgroundColor === "warm-white") {
    backgroundColor = "bg-brand-warm-white"
  }

  baseStyle = baseStyle + " " + backgroundColor

  if (style?.isPill) {
    baseStyle = baseStyle + " rounded-full border border-brand-black"
  } else {
    baseStyle = baseStyle + " rounded border border-brand-black"
  }

  if (disabled) {
    if (!style?.backgroundColor) {
      baseStyle = baseStyle + " bg-brand-disabled"
    }
    baseStyle = baseStyle + " cursor-not-allowed text-text-disabled"
  } else {
    baseStyle = baseStyle + " cursor-pointer text-black"
  }
  if (style?.bold) {
    baseStyle = baseStyle + " font-bold"
  }

  return baseStyle
}

</script>

<template>
  <button @click="onClick" :class="createButtonStyle(disabled ?? false, styling)" type="button" :disabled="disabled">
    <div class="flex flex-row gap-2 items-center justify-start">
      <div v-if="isSubmitting"
        class="bg-brand-primary text-text-primary mr-3 size-5 animate-spin rounded-full border-4 border-white border-t-transparent">
      </div>
      <div v-if="icon && icon === 'plus'">
        <IconPlus class="text-brand-primary w-[22px] h-fit" />
      </div>
      <div v-if="icon && icon === 'back'">
        <IconBack class="text-text-primary w-[22px] h-fit" />
      </div>
      <div v-if="icon && icon === 'delete'">
        <IconDelete class="text-text-primary w-[22px] h-fit" />
      </div>
      <span v-if="text" class="text-black text-nowrap text-center">{{ text }}</span>
    </div>
  </button>
</template>
