<script setup lang="ts">
import Modal from '@/components/basics/PopupModal.vue';
import type { Book } from '@/models/entry';
import BrandedButton from '@/components/basics/BrandedButton.vue';
import { computed, ref } from 'vue'
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

const isInvalidValid = computed(() => {
  const hasName = model.value.name.length > 0
  const hasAuthor = model.value.author.length > 0

  return !(hasName && hasAuthor) || isSubmitting.value
})


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
    activatedChallengeIds: [],
    favorite: false
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
        <TextInput name="name" label="Nimi" v-model="model.name" icon="Book" />
        <TextInput name="author" label="Kirjailija" :required="true" v-model="model.author" icon="Author" />
        <TextInput name="translator" label="Kääntäjä" :required="false" v-model="model.translator" icon="Translator" />
        <div class="flex flex-row justify-between py-2 gap-3">
          <BrandedButton :onClick="closeModal" text="Peru" icon="Cross" :styling="{
            isPill: true,
            bold: true,
            iconColor: 'text-white'
          }" />
          <BrandedButton :onClick="submitModal" :disabled="isInvalidValid" text="Tallenna" icon="Check"
            :is-submitting="isSubmitting" :styling="{
              isPill: true,
              bold: true,
              iconColor: 'text-white'
            }" />
        </div>
      </div>
    </form>
  </Modal>
</template>
