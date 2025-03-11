<script lang="ts" setup>

import { ref } from "vue"
import Button from "@/components/basics/Button.vue"
import NewBookModal from "@/components/NewBookModal.vue"
import type { Entry } from "@/models/entry"
import IconPlus from "./icons/IconPlus.vue"


const exampleItems: Entry[] =
  [
    {
      kind: "Book",
      name: "Kirja 1",
      author: "Minä"
    },
    {
      kind: "Book",
      name: "Kirja 2",
      author: "Sinä",
      translator: "Hän"
    }
  ]

const buttonDisplay = "Uusi kirja"

const items = ref(exampleItems)
const showDialog = ref(false)

function openNewEntryDialog() {
  showDialog.value = true
}

function addNewEntry(entry: Entry): void {
  showDialog.value = false
  items.value.push(entry)
}

function closeModals(): void {
  showDialog.value = false
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
        <div class="bg-brand-primary w-30 rounded ">
          <button class="w-full h-full flex justify-center cursor-pointer" :onClick="openNewEntryDialog">
            <IconPlus class="w-20 text-white"></IconPlus>
          </button>
        </div>
        <div v-for="entry in items" :key="entry.name" class="flex flex-col bg-white shadow-lg rounded p-10">
          <p>{{ entry.name }}</p>
          <p> {{ entry.author }}</p>
          <p> {{ entry.translator }}</p>
        </div>
      </div>

    </div>
    <NewBookModal :is-modal-open="showDialog" :onSubmit="addNewEntry" :onClose="closeModals" />

  </div>


</template>
