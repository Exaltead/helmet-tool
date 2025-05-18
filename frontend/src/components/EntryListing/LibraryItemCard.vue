<script setup lang="ts">
import IconedText from '@/components/basics/IconedText.vue';
import IconBook from '../icons/IconBook.vue';
import IconFavoriteEmpty from '../icons/IconFavoriteEmpty.vue';
import type { Entry } from '@/models/entry';

defineProps<{
  item: Entry
}>()

</script>

<template>
  <div class="card flex flex-col items-center justify-start max-w-[400px] min-w-[250px] w-full p-2.5 pt-2 gap-2">
    <div v-if="item.kind === 'Book'" class="flex flex-row justify-between items-center w-full px-2.5 py-1 border-b border-brand-orange">
      <IconedText :text="item.name" :icon-name="'Book'" :heading="true" />
      <IconFavoriteEmpty class="text-brand-orange" />
    </div>
    <div class="flex flex-col justify-start w-full h-fit px-2.5 gap-2">
      <IconedText :text="item.author" icon-name="Author" :heading="false" />
      <IconedText v-if="item.translator" :text="item.translator" :icon-name="'Book'" :heading="false" />
    </div>
  </div>

</template>
