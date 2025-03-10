<script lang="ts" setup>

import { ref } from "vue"
import Button from "@/components/basics/Button.vue"
import Modal from "./basics/Modal.vue"
import NewBookModal from "@/components/NewBookModal.vue"
import type { Entry } from "@/models/entry"


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
    <h1> Toimii</h1>
    <div class="ml-10 flex flex-col">
      <div class="flex flex-row gap-10">
        <h1 class="text-brand-primary text-bold text-lg">Luetut kirjat</h1>
        <Button :onClick="openNewEntryDialog" :display="buttonDisplay"></Button>
      </div>

      <table class="text-left">
        <thead>
          <tr>
            <th>Nimi</th>
            <th>Kirjailija</th>
            <th>Kääntäjä</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(entry, index) in items" :key="index">
            <td> {{ entry.name }}</td>
            <td> {{ entry.author }}</td>
            <td> {{ entry.translator ?? "" }}</td>
          </tr>

        </tbody>
      </table>

    </div>
    <NewBookModal :is-modal-open="showDialog" :onSubmit="addNewEntry" :onClose="closeModals" />

  </div>


</template>
