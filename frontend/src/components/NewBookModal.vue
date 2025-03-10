<script setup lang="ts">
import Modal from '@/components/basics/Modal.vue';
import type { Book } from '@/models/entry';
import Button from '@/components/basics/Button.vue';
import { ref } from 'vue'

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
      <div class="flex flex-col gap-4">
        <div>
          <label for="bookName">Kirjan nimi</label>
          <input type="text" required name="bookName" v-model="newBook.name" />
        </div>
        <div>
          <label for="author">Kirjailija</label>
          <input type="text" required name="author" v-model="newBook.author" />
        </div>
        <div>
          <label for="translator">Kääntäjä</label>
          <input type="text" name="translator" v-model="newBook.translator" />
        </div>
        <div>
          <Button :onClick="closeModal"> {{ "Peru" }}</Button>
          <Button :onClick="submitModal"> {{ "Lisää" }}</Button>
        </div>

      </div>
    </form>

  </Modal>
</template>
