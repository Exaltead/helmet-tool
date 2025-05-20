<script lang="ts" setup>

import { computed, ref } from "vue"
import Button from "@/components/basics/BrandedButton.vue"
import EditEntryModal from "@/components/EntryListing/NewItemModal.vue"
import type { Entry } from "@/models/entry"
import IconPlus from "@/components/icons/IconPlus.vue"
import { RouterLink, useRouter } from "vue-router"
import { fetchLibraryItems } from "@/api/libraryApi"
import LibraryItemCard from "./LibraryItemCard.vue"

const router = useRouter()
const selectedEntry = ref<Entry | undefined>(undefined)

const items = ref<Entry[]>([])

const listItems = computed(() => items.value)

async function getItems() {
  const serverItems = await fetchLibraryItems()

  items.value = serverItems
}

getItems()


const showDialog = ref(false)

function openNewEntryDialog() {
  showDialog.value = true
}


async function onNewItemSubmitComplete(id: string): Promise<void> {
  showDialog.value = false
  selectedEntry.value = undefined

  router.push({ name: "libraryItem", params: { id } })
}

function closeModals(): void {
  selectedEntry.value = undefined
  showDialog.value = false
}

</script>

<template>
  <div>
    <div class="grid md:grid-cols-2 w-full gap-3">
        <LibraryItemCard v-for="entry in listItems" :key="entry.id" :item="entry" />

    </div>

    <EditEntryModal :is-modal-open="showDialog" @submitComplete="onNewItemSubmitComplete" @close="closeModals"
      :selected-entry="selectedEntry" />

  </div>


</template>
