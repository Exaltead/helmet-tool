<script lang="ts" setup>
import type { Challenge } from '@/models/challenge';
import { computed, ref } from 'vue';
import TextInput from '../basics/TextInput.vue';
import { v4 } from 'uuid';
import IconPlus from "@/components/icons/IconPlus.vue"
import IconBack from '@/components/icons/IconBack.vue';
import BrandedButton from '../basics/BrandedButton.vue';
const { target } = defineProps<{ target: Challenge | undefined }>()

function createEditTarget(target: Challenge | undefined): Challenge {
  if (target === undefined) {
    return {
      id: "",
      name: "",
      questions: [],
      status: "active",
      targetMedia: "Book"
    }
  } else {
    return {
      ...target,
      questions: target.questions.map((question) => ({
        ...question
      })).sort((a, b) => a.number - b.number)
    }

  }
}
const emit = defineEmits<{
  (e: "submitComplete", id: string): void
  (e: "close"): void
}>()

const isSubmitting = ref(false)

const editTarget = ref<Challenge>(createEditTarget(target))
const activeCheckBox = ref(editTarget.value.status === "active")


const selectOptions = computed(() => {
  return [...Array(editTarget.value.questions.length ?? 0).keys()]

})

function addNewQuestion() {
  const newQuestion = {
    id: v4(),
    question: "",
    number: editTarget.value.questions.length
  }
  editTarget.value.questions.push(newQuestion)
  editTarget.value.questions.sort((a, b) => a.number - b.number)
}

function removeQuestion(id: string) {
  if (editTarget.value.questions.length <= 1) {
    return
  }
  const index = editTarget.value.questions.findIndex((question) => question.id === id)
  if (index === -1) {
    return
  }
  editTarget.value.questions.splice(index, 1)
  editTarget.value.questions.forEach((question, i) => {
    question.number = i
  })

}

function submit() {
  if (editTarget.value === undefined) {
    return
  }

  isSubmitting.value = true
  // Simulate an API call
  setTimeout(() => {
    isSubmitting.value = false
    emit("submitComplete", editTarget.value.id)
  }, 2000)
}

</script>

<template>
  <div class="p-2">
    <button :onClick="() => emit('close')" class="top-0 left-0 relative">
      <IconBack class="text-brand-primary w-8 h-fit cursor-pointer" />
    </button>
    <div class="px-10 bg-white shadow-lg rounded px-4 py-2 bg-light-gray">
      <div class="flex flex-col gap-4">
        <div class="flex flex-row gap-4 justify-between items-center">
          <TextInput class="basis-4/5" v-model="editTarget.name" name="Nimi" label="Haasteen nimi" />
          <BrandedButton text="Tallenna" :onClick="submit" class="h-fit" :isSubmitting="isSubmitting" />
        </div>
        <div class="flex flex-row gap-4">
          <label for="kind"> Haasteeseen soveltuvat: </label>
          <select name="kind" v-model="editTarget.targetMedia">
            <option value="Book">Kirja</option>
          </select>
        </div>

        <div class="flex flex-row gap-4">
          <input type="checkbox" v-model="activeCheckBox" name="active" />
          <label for="active">Aktiivinen</label>
        </div>

        <h2>Kysymykset</h2>
        <ul class="flex flex-col gap-2">
          <li v-for="question in editTarget.questions" :key="question.id"
            class="bg-white shadow-lg rounded px-4 py-2 flex flex-col gap-2">
            <TextInput v-model="question.question" name="kysymys" label="Kysymys" />
            <div class="flex flex-row gap-4">
              <label for="number">Järjestysnumero</label>
              <select name="number" v-model="question.number">
                <option v-for="option in selectOptions" :key="option" :value="option">
                  {{ option + 1 }}
                </option>
              </select>
            </div>
            <div class="flex flex-row justify-end">
              <BrandedButton icon="delete" class="h-fit" :onClick="() => removeQuestion(question.id)" />
            </div>
          </li>
          <button class="mt-4 ml-4 w-full rounded h-full flex justify-center cursor-pointer bg-brand-primary"
            :onClick="addNewQuestion">
            <IconPlus class="w-10 text-white"></IconPlus>
          </button>
        </ul>
      </div>
    </div>

  </div>

</template>
