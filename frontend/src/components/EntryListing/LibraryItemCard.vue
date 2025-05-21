<script setup lang="ts">
import IconedText from '@/components/basics/IconedText.vue';
import IconFavoriteEmpty from '../icons/IconFavoriteEmpty.vue';
import type { Entry } from '@/models/entry';
import IconChevronRight from '../icons/IconChevronRight.vue';
import { updateLibraryItem } from '@/api/libraryApi';
import CustomIcon from '../basics/CustomIcon.vue';
import { ref } from 'vue';

const props = defineProps<{
  item: Entry
}>()

const favorite = ref<boolean>(props.item.favorite)

const emit = defineEmits<{
  (e: "itemUpdated"): void
}>()

async function updateItemFavorite(): Promise<void> {
  favorite.value = !favorite.value
  const newItem = { ...props.item }
  newItem.favorite = !props.item.favorite


  await updateLibraryItem(newItem)
  emit("itemUpdated")
}

</script>

<template>
  <div class="card flex flex-col items-center justify-start md:max-w-[400px] min-w-[250px] w-full p-2.5 pt-2 gap-2">
    <div v-if="item.kind === 'Book'"
      class="flex flex-row justify-between items-center w-full px-2.5 py-1 border-b border-brand-orange">
      <IconedText :text="item.name" :icon-name="'Book'" :heading="true" />
      <button @click="updateItemFavorite" class="cursor-pointer">
        <CustomIcon v-if="favorite" name="HeartFull" class="text-brand-orange" />
        <CustomIcon v-else name="HeartEmpty" class="text-brand-orange" />
      </button>

    </div>
    <div class="flex flex-row justify-between items-center w-full px-2.5">
      <div class="flex flex-col justify-start w-full h-fit gap-2">
        <IconedText :text="item.author" icon-name="Author" :heading="false" />
        <IconedText v-if="item.translator" :text="item.translator" :icon-name="'Translator'" :heading="false" />
      </div>
      <RouterLink :to="{ name: 'libraryItem', params: { id: item.id } }">
        <IconChevronRight class="text-brand-orange h-[30px] w-[30px]" />
      </RouterLink>
    </div>

  </div>

</template>
