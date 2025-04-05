<script lang="ts" setup>
import type { Entry } from '@/models/entry';
import { ref } from 'vue';
import Button from "@/components/basics/Button.vue"
import TextInput from "@/components/basics/TextInput.vue"

const itemBase: Entry = {
  kind: 'Book',
  id: '1234',
  name: 'Testi nimi',
  author: ' Testi kirjailija',
  translator: 'Testi kääntäjä'
}

const item = ref(itemBase)

const inEditMode = ref(false)

const displayText = ref("Muokkaa")

const modifyTarget = ref<Entry>({...item.value})

function toggleEdit() {
  if(inEditMode.value) {
    item.value = { ...modifyTarget.value }
    inEditMode.value = false
    displayText.value = "Muokkaa"
  } else {

    inEditMode.value = true
    displayText.value = "Tallenna"
    modifyTarget.value = { ...item.value }
  }
}

</script>

<template>
  <div class="flex flex-row items-center justify-between">
    <div class="flex flex-col gap-1"  v-if="inEditMode">
      <TextInput name="name" label="Nimi" :required=true v-model="modifyTarget.name" />
      <TextInput name="author" label="Kirjailija" :required=true v-model="modifyTarget.author" />
      <TextInput name="translator" label="Kääntäjä" :required=false v-model="modifyTarget.translator" />

    </div>
    <div v-else>
      <p>Nimi: {{ item.name }}</p>
      <p>Kirjailija: {{ item.author }}</p>
      <p>Kääntäjä: {{ item.translator }}</p>
    </div>
    <Button :onClick="toggleEdit" :text="displayText" ></Button>
  </div>

</template>
