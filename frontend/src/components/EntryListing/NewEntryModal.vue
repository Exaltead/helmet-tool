<script setup lang="ts">
import Modal from '@/components/basics/Modal.vue';
import type { Book } from '@/models/entry';
import Button from '@/components/basics/Button.vue';
import { ref } from 'vue'
import TextInput from "@/components/basics/TextInput.vue"
import { addLibraryItem } from '@/api/libraryApi';


const { isModalOpen } = defineProps<{
  isModalOpen: boolean
}>()

const emit = defineEmits<{
  (e: "submitComplete", id: string): void
  (e: "close"): void
}>()

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

const isSubmitting = ref(false)


function closeModal(): void {
  model.value = { ...newBookBase }
  emit("close")
}

async function submitModal(): Promise<void> {
  const newBook: Omit<Book, "id"> = {
    kind: "Book",
    name: model.value.name,
    author: model.value.author,
    translator: model.value.translator,
  }


  isSubmitting.value = true
  const newId = await addLibraryItem(newBook)
  if (newId === undefined) {

    //TODO: show error to user
    isSubmitting.value = false
    return
  }
  isSubmitting.value = false
  emit("submitComplete", newId)

  model.value = { ...newBookBase }
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
          <Button :onClick="closeModal" text="Peru"></Button>
          <Button :onClick="submitModal" text="Tallenna"></Button>
        </div>
      </div>
    </form>
  </Modal>
</template>
