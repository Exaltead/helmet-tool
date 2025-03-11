<script setup lang="ts">
import Modal from '@/components/basics/Modal.vue';
import type { Book } from '@/models/entry';
import Button from '@/components/basics/Button.vue';
import { ref } from 'vue'
import TextInput from "@/components/basics/TextInput.vue"

interface Props {
  isModalOpen: boolean
  onSubmit: (book: Book) => void
  onClose: () => void
}

const { isModalOpen, onClose, onSubmit } = defineProps<Props>()

function createEmptyBook(): Book {
  return {
    kind: "Book",
    author: "",
    name: "",
    translator: ""
  }
}


const newBook = ref(createEmptyBook())

function closeModal(): void {
  onClose()
}

function submitModal(): void {
  const value = newBook.value
  newBook.value = createEmptyBook()
  onSubmit(value)
}

</script>

<template>
  <Modal :show-modal="isModalOpen">
    <form>
      <div class="flex flex-col gap-4 p-4">
        <TextInput name="name" label="Nimi" :required="true" v-model="newBook.name" />
        <TextInput name="author" label="Kirjailija" :required="true" v-model="newBook.author" />
        <TextInput name="translator" label="Kääntäjä" :required="true" v-model="newBook.translator" />
        <div class="flex flex-row justify-between py-2">
          <Button :onClick="closeModal"> {{ "Peru" }}</Button>
          <Button :onClick="submitModal"> {{ "Lisää" }}</Button>
        </div>

      </div>
    </form>

  </Modal>
</template>
