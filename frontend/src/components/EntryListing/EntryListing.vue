<script lang="ts" setup>

import { computed, ref } from "vue"
import Button from "@/components/basics/Button.vue"
import EditEntryModal from "@/components/EntryListing/NewEntryModal.vue"
import type { Book, Entry } from "@/models/entry"
import IconPlus from "@/components/icons/IconPlus.vue"
import { RouterLink } from "vue-router"
import { addLibraryItem, fetchLibraryItems } from "@/api/libraryApi"


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



async function addNewEntry(item: Omit<Book, "id">): Promise<void> {

  await addLibraryItem(item)
  showDialog.value = false
  selectedEntry.value = undefined

  await getItems()
}

function closeModals(): void {
  selectedEntry.value = undefined
  showDialog.value = false
}


</script>

<template>
  <div>
    <div class="ml-10 flex flex-col">
      <div class="flex flex-row gap-10">
        <h1 class="text-brand-primary text-bold text-lg">Luetut kirjat</h1>
        <Button hidden :onClick="openNewEntryDialog" display="Uusi kirja"></Button>
      </div>

      <div class="flex gap-4 mt-6 flex-wrap">
        <div class="bg-brand-primary w-30 rounded">
          <button class="w-full h-full flex justify-center cursor-pointer" :onClick="openNewEntryDialog">
            <IconPlus class="w-20 text-white"></IconPlus>
          </button>
        </div>
        <div v-for="entry in listItems" :key="entry.id" class=" bg-white shadow-lg rounded ">
          <RouterLink :to="{ name: 'libraryItem', params: { id: entry.id } }">
            <div class="flex flex-col h-full w-full cursor-pointer p-10">
              <p>{{ entry.name }}</p>
              <p> {{ entry.author }}</p>
              <p> {{ entry.translator }}</p>
            </div>
          </RouterLink>


        </div>
      </div>

    </div>

    <EditEntryModal :is-modal-open="showDialog" :onSubmit="addNewEntry" :onClose="closeModals"
      :selected-entry="selectedEntry" />

  </div>


</template>
