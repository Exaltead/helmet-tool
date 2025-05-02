<script lang="ts" setup>
import type { Entry } from '@/models/entry';
import { computed, ref } from 'vue';
import Button from "@/components/basics/BrandedButton.vue"
import TextInput from "@/components/basics/TextInput.vue"
import { updateLibraryItem, deleteLibraryItem } from '@/api/libraryApi';
import type { Challenge } from '@/models/challenge';
import ManagementList from '../basics/ManagementList.vue';



const { item, challenges } = defineProps<{
  item: Entry
  challenges: Challenge[]
}>()

const emit = defineEmits<{
  (e: "objectEdited"): void
  (e: "objectDeleted"): void
  (e: "editModeChanged", inEditMode: boolean): void
}>()


const modifyTarget = ref<Entry | undefined>(undefined)

const inEditMode = ref(false)


function enableEditMode() {
  inEditMode.value = true
  modifyTarget.value = { ...item }
  emit("editModeChanged", true)
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
  emit("editModeChanged", false)

}

const isDeleting = ref(false)
async function deleteItem() {
  isDeleting.value = true
  await deleteLibraryItem(item.id)
  isDeleting.value = false
  emit("objectDeleted")
  emit("editModeChanged", false)
}

const activatedChallengeList = computed(() => {
  return item.activatedChallengeIds.map((id) => {
    const challenge = challenges.find((challenge) => challenge.id === id)
    if (challenge) {
      return {
        value: challenge.id,
        display: challenge.name
      }
    }
    return undefined
  }).filter((challenge): challenge is { value: string, display: string } => challenge !== null)
})

const allChallenges = computed(() => {
  return challenges.map((challenge) => {
    return {
      value: challenge.id,
      display: challenge.name
    }
  })
})

function onChallengeStateChanged(challengeIds: string[]) {
  if (modifyTarget.value) {
    modifyTarget.value.activatedChallengeIds = challengeIds
  }
}

</script>

<template>
  <div>
    <div v-if="inEditMode" class="flex flex-col gap-2">
      <div class="flex flex-row items-center justify-between">
        <div class="flex flex-col gap-1">
          <TextInput name="name" label="Nimi" :required=true v-model="modifyTarget!.name" />
          <TextInput name="author" label="Kirjailija" :required=true v-model="modifyTarget!.author" />
          <TextInput name="translator" label="Kääntäjä" :required=false v-model="modifyTarget!.translator" />
        </div>

      </div>
      <div>
        <h2>Aktiiviset haasteet</h2>
        <ManagementList :items="activatedChallengeList" :allOptions="allChallenges"
          @stateChanged="onChallengeStateChanged" />
      </div>

      <div class="flex flex-row gap-2 py-3">
        <Button :onClick="cancelEdit" text="Peruuta"></Button>
        <Button :onClick="submit" text="Tallenna"></Button>
      </div>
    </div>
    <div v-else>
      <div class="flex flex-row items-start justify-between">
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


  </div>

</template>
