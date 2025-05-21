<script lang="ts" setup>

import { computed, ref } from "vue"
import type { Entry } from "@/models/entry"
import { fetchLibraryItems } from "@/api/libraryApi"
import LibraryItemCard from "./LibraryItemCard.vue"

const items = ref<Entry[]>([])

const listItems = computed(() => items.value)

async function getItems() {
  const serverItems = await fetchLibraryItems()

  items.value = serverItems
}

getItems()


</script>

<template>
  <div>
    <div class="grid md:grid-cols-2 w-full gap-3">
      <LibraryItemCard v-for="entry in listItems" :key="entry.id" :item="entry" @item-updated="getItems" />
    </div>
  </div>
</template>
