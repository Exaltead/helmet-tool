<script lang="ts" setup>
import IconAuthor from '@/components/icons/IconAuthor.vue';
import IconTranslator from '@/components/icons/IconTranslator.vue';
import IconBook from '@/components/icons/IconBook.vue';
import IconBack from "@/components/icons/IconBack.vue"
import IconDelete from "@/components/icons/IconDelete.vue"
import IconPlus from "@/components/icons/IconPlus.vue"
import type { IconName } from '@/models/iconName';
import IconCross from '../icons/IconCross.vue';
import IconCheck from '../icons/IconCheck.vue';
import IconFavoriteEmpty from '../icons/IconFavoriteEmpty.vue';
import IconFavoriteSelected from '../icons/IconFavoriteSelected.vue';
import IconGame from '../icons/IconGame.vue';

defineProps<{
  name: IconName
}>()

</script>

<template>

  <IconBook v-if="name === 'Book'" />
  <IconAuthor v-else-if="name === 'Author'" />
  <IconTranslator v-else-if="name === 'Translator'" />
  <IconPlus v-else-if="name === 'Plus'" />
  <IconBack v-else-if="name === 'Back'" />
  <IconDelete v-else-if="name === 'Delete'" />
  <IconCross v-else-if="name === 'Cross'" />
  <IconCheck v-else-if="name === 'Check'" />
  <IconFavoriteEmpty v-else-if="name === 'HeartEmpty'" />
  <IconFavoriteSelected v-else-if="name === 'HeartFull'" />
  <IconGame v-else-if="name === 'Game'" />

</template>
