<script setup lang="ts">
import Modal from '@/components/basics/Modal.vue';
import type { Book, Entry } from '@/models/entry';
import Button from '@/components/basics/Button.vue';
import { computed } from 'vue'
import TextInput from "@/components/basics/TextInput.vue"

interface Props {
  isModalOpen: boolean
  onSubmit: (book: Book) => void
  onClose: () => void
  selectedEntry: Entry | undefined
}

const { isModalOpen, onClose, onSubmit, selectedEntry } = defineProps<Props>()

function createEditable(target: Entry | undefined): Entry {
  if (!target) {
    return {
      kind: "Book",
      id: "",
      author: "",
      name: "",
      translator: ""
    }
  }

  return { ...target }


}

const targetEntry = computed(() => createEditable(selectedEntry))

function closeModal(): void {
  onClose()
}

function submitModal(): void {
  const value = targetEntry.value
  // This is a hack to make the UI work without the backend
  if (value.id === "") {
    value.id = value.name + Math.floor(Math.random() * 10000000)
  }

  onSubmit(value)
}

</script>

<template>
  <Modal :show-modal="isModalOpen">
    <form>
      <div class="flex flex-col gap-4 p-4">
        <TextInput name="name" label="Nimi" :required="true" v-model="targetEntry.name" />
        <TextInput name="author" label="Kirjailija" :required="true" v-model="targetEntry.author" />
        <TextInput name="translator" label="Kääntäjä" :required="true" v-model="targetEntry.translator" />
        <div class="flex flex-row justify-between py-2">
          <Button :onClick="closeModal"> {{ "Peru" }}</Button>
          <Button :onClick="submitModal"> {{ "Tallenna" }}</Button>
        </div>

      </div>
    </form>

  </Modal>
</template>
