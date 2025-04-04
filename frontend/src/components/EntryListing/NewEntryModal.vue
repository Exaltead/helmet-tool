<script setup lang="ts">
import Modal from '@/components/basics/Modal.vue';
import type { Book } from '@/models/entry';
import Button from '@/components/basics/Button.vue';
import { ref } from 'vue'
import TextInput from "@/components/basics/TextInput.vue"

interface Props {
  isModalOpen: boolean
  onSubmit: (book: Omit<Book, "id">) => void
  onClose: () => void
}

const { isModalOpen, onClose, onSubmit } = defineProps<Props>()

type NewBook = {
  name: string,
  author: string,
  translator: string
}

const newBookBase: NewBook = {
  name: "",
  author: "",
  translator: ""
}

const model = ref<NewBook>({ ...newBookBase })


function closeModal(): void {
  model.value = { ...newBookBase }
  onClose()
}

async function submitModal(): Promise<void> {
  const newBook: Omit<Book, "id"> = {
    kind: "Book",
    name: model.value.name,
    author: model.value.author,
    translator: model.value.translator,
  }

  console.log("submitModal")
  model.value = { ...newBookBase }
  onSubmit(newBook)
}


</script>

<template>
  <Modal :show-modal="isModalOpen">
    <form>
      <div class="flex flex-col gap-4 p-4">
        <TextInput name="name" label="Nimi" v-model="model.name" />
        <TextInput name="author" label="Kirjailija" :required="true" v-model="model.author" />
        <TextInput name="translator" label="Kääntäjä" :required="false" v-model="model.translator" />
        <div class="flex flex-row justify-between py-2">
          <Button :onClick="closeModal"> {{ "Peru" }}</Button>
          <Button :onClick="submitModal"> {{ "Tallenna" }}</Button>
        </div>
      </div>
    </form>
  </Modal>
</template>
