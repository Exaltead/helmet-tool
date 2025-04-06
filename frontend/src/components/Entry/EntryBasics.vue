<script lang="ts" setup>
import type { Entry } from '@/models/entry';
import { ref } from 'vue';
import Button from "@/components/basics/Button.vue"
import TextInput from "@/components/basics/TextInput.vue"
import { fetchLibraryItem, updateLibraryItem, deleteLibraryItem } from '@/api/libraryApi';



const { itemId } = defineProps<{
  itemId: string
}>()

const emit = defineEmits<{
  (e: "objectEdited"): void
  (e: "objectDeleted"): void
}>()

const item = ref<Entry | undefined>(undefined)
const modifyTarget = ref<Entry | undefined>(undefined)
async function getItem(id: string) {
  item.value = await fetchLibraryItem(id)
}

getItem(itemId)
const inEditMode = ref(false)



function enableEditMode() {
  if (item.value === undefined) {

    return
  }
  inEditMode.value = true
  modifyTarget.value = { ...item.value }
}

function cancelEdit() {
  inEditMode.value = false
  modifyTarget.value = undefined
}

const isSubmitting = ref(false)
async function submit() {
  if (modifyTarget.value === undefined) {
    return
  }

  isSubmitting.value = true
  await updateLibraryItem(modifyTarget.value)
  isSubmitting.value = false
  inEditMode.value = false
  modifyTarget.value = undefined
  emit("objectEdited")

  await getItem(itemId)
}

const isDeleting = ref(false)
async function deleteItem() {
  if( item.value === undefined) {
    return
  }
  isDeleting.value = true
  await deleteLibraryItem(itemId)
  isDeleting.value = false
  emit("objectDeleted")
}

</script>

<template>
  <div>
    <div class="flex flex-row items-center justify-between" v-if="inEditMode">
      <div class="flex flex-col gap-1">
        <TextInput name="name" label="Nimi" :required=true v-model="modifyTarget!.name" />
        <TextInput name="author" label="Kirjailija" :required=true v-model="modifyTarget!.author" />
        <TextInput name="translator" label="Kääntäjä" :required=false v-model="modifyTarget!.translator" />
        <div class="flex flex-row gap-2 py-3">
          <Button :onClick="cancelEdit" text="Peruuta"></Button>
          <Button :onClick="submit" text="Tallenna"></Button>
        </div>


      </div>
    </div>
    <div class="flex flex-row items-start justify-between" v-else>
      <div class="flex flex-col gap-1" v-if="item">
        <p>Nimi: {{ item.name }}</p>
        <p>Kirjailija: {{ item.author }}</p>
        <p>Kääntäjä: {{ item.translator }}</p>
      </div>
      <div class="flex flex-row gap-2 align-top justify-start">
        <Button :onClick="enableEditMode" text="Muokkaa" :isSubmitting="isSubmitting"></Button>
        <Button :onClick="deleteItem" text="Poista" icon="delete" :isSubmitting="isDeleting"></Button>
      </div>
    </div>

  </div>

</template>
