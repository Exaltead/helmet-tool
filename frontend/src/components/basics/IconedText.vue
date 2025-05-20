<script setup lang="ts">
import IconAuthor from '@/components/icons/IconAuthor.vue';
import IconTranslator from '@/components/icons/IconTranslator.vue';
import IconBook from '@/components/icons/IconBook.vue';

type IconName = "Book" | "Author" | "Translator"

defineProps<{ text: string, iconName: IconName, heading: boolean }>();

</script>

<template>

  <div class="flex flex-row items-center justify-start gap-2">
    <IconBook v-if="iconName === 'Book'" class="text-brand-orange" />
    <IconAuthor v-else-if="iconName === 'Author'" class="text-brand-orange" />
    <IconTranslator v-else-if="iconName === 'Translator'" class="text-brand-orange" />
    <div v-if="heading" class="text-brand-rock text-lg font-bold">
      {{ text }}
    </div>
    <div v-else class="text-brand-rock text-sm font-normal">
      {{ text }}
    </div>
  </div>

</template>
