<script lang="ts" setup>

import { ref } from "vue"
import Button from "@/components/basics/Button.vue"
import EditEntryModal from "@/components/EntryListing/EditEntryModal.vue"
import type { Entry } from "@/models/entry"
import IconPlus from "@/components/icons/IconPlus.vue"


const exampleItems: Entry[] =
  [
    {
      kind: "Book",
      id: "1",
      name: "Kirja 1",
      author: "Minä"
    },
    {
      kind: "Book",
      id: "2",
      name: "Kirja 2",
      author: "Sinä",
      translator: "Hän"
    }
  ]

const buttonDisplay = "Uusi kirja"
const selectedEntry = ref<Entry | undefined>(undefined)

const items = ref(exampleItems)
const showDialog = ref(false)

function openNewEntryDialog() {
  showDialog.value = true
}

function addNewEntry(entry: Entry): void {
  // This should be refeshed from backend, but for now just fake it
  const index = items.value.findIndex(t => t.id === entry.id)
  if (index !== -1) {
    items.value[index] = entry
  }
  else {
    items.value.push(entry)
  }
  if (!selectedEntry.value) {

  }
  showDialog.value = false
  selectedEntry.value = undefined
}

function closeModals(): void {
  selectedEntry.value = undefined
  showDialog.value = false
}
function editEntry(entry: Entry): void {
  console.log("For edit", entry)

  selectedEntry.value = entry
  showDialog.value = true
}

</script>

<template>
  <div>
    <div class="ml-10 flex flex-col">
      <div class="flex flex-row gap-10">
        <h1 class="text-brand-primary text-bold text-lg">Luetut kirjat</h1>
        <Button hidden :onClick="openNewEntryDialog" :display="buttonDisplay"></Button>
      </div>

      <div class="flex gap-4 mt-6">
        <div class="bg-brand-primary w-30 rounded">
          <button class="w-full h-full flex justify-center cursor-pointer" :onClick="openNewEntryDialog">
            <IconPlus class="w-20 text-white"></IconPlus>
          </button>
        </div>
        <div v-for="entry in items" :key="entry.id" class=" bg-white shadow-lg rounded ">
          <button class="flex flex-col h-full w-full cursor-pointer p-10" @click="editEntry(entry)">
            <p>{{ entry.name }}</p>
            <p> {{ entry.author }}</p>
            <p> {{ entry.translator }}</p>
          </button>

        </div>
      </div>

    </div>

    <EditEntryModal :is-modal-open="showDialog" :onSubmit="addNewEntry" :onClose="closeModals"
      :selected-entry="selectedEntry" />

  </div>


</template>
