<script setup lang="ts">

import type { Answer, Question } from "@/models/challenge";
import { RadioGroup, RadioGroupLabel, RadioGroupOption } from "@headlessui/vue"
import { v4 } from "uuid";
import { computed, ref } from "vue";
import BrandedButton from "../basics/BrandedButton.vue";
import { fetchChallenges } from "@/api/challengeApi";
import { addAnswer, getAnswer, updateAnswer } from "@/api/answerApi";


const props = defineProps<{ itemId: string, challengeId: string }>()
const emits = defineEmits<{
  (e: 'submit', answers: Answer[]): void
}>()


const answersMap = ref<Map<string, Answer>>(new Map([]))
const questions = ref<Question[]>([])
let answerSetId: string | undefined = undefined
const challengeName = ref<string>("Haaste")
async function refreshData(): Promise<void> {
  const { id, answers } = await getAnswer(props.challengeId, props.itemId)
  answerSetId = id
  const challenge = (await fetchChallenges())
    .find(t => t.id === props.challengeId)
  if(challenge) {
    challengeName.value = challenge.name
  }

  const newQuestions = challenge?.questions ?? []
  const newMap = new Map<string, Answer>(answers.map((answer) => [answer.id, { ...answer }]))
  for (const question of newQuestions) {
    if (answers.find(t => t.questionId === question.id)) {
      continue
    }
    const id = v4()

    newMap.set(id, {
      kind: "Boolean",
      id: v4(),
      questionId: question.id,
      answered: true,
      answer: "no"
    })
  }

  questions.value = newQuestions
  answersMap.value = newMap
}


refreshData()

type DisplayAnswer = {
  id: string
  number: number
  question: string
  answer: "yes" | "no"
  answerId: string
}

const displayAnswers = computed(() => {


  return [...answersMap.value].map(([id, answer]) => {
    const question = questions.value.find((q) => q.id === answer.questionId)
    if (question) {
      return {
        id: question.id,
        answerId: id,
        number: question.number,
        question: question.question,
        answer: answer.answer
      }
    }
    return null
  }).filter((item): item is DisplayAnswer => item !== null)
    .sort((a, b) => a.number - b.number)
})

function makeRadioStyle(checked: boolean) {
  const adds = checked ? 'bg-brand-primary text-white' : 'border border-brand-primary'

  return "rounded flex items-center w-full justify-center h-full p-1 cursor-pointer" + " " + adds
}

const isSubmitting = ref(false)
async function submit() {
  const answers: Answer[] = [...answersMap.value.values()]
  isSubmitting.value = true
  if (!answerSetId) {
    await addAnswer(answers, props.challengeId, props.itemId)

  }
  else {
    await updateAnswer(answerSetId, answers, props.challengeId, props.itemId)
  }
  isSubmitting.value = false

  emits('submit', [...answersMap.value.values()])
}


</script>


<template>
  <div>

    <div class="flex flex-row gap-2 justify-between items-center bg-light-gray p-2">
      <h1>{{ challengeName }}</h1>
      <BrandedButton text="Tallenna" class="mt-4" @click="submit" :isSubmitting="isSubmitting" />
    </div>

    <div v-for="(displayAnswer, index) in displayAnswers" :key="index" class="bg-light-gray p-2">
      <RadioGroup v-model="answersMap.get(displayAnswer.answerId)!.answer" class="flex flex-col gap-1">
        <RadioGroupLabel>{{ displayAnswer.question }}</RadioGroupLabel>
        <div class="flex flex-row gap-12 pl-8 justify-end pr-4">
          <RadioGroupOption value="no" v-slot="{ checked }" class="min-w-12 flex">
            <span :class="makeRadioStyle(checked)">Ei</span>
          </RadioGroupOption>
          <RadioGroupOption v-slot="{ checked }" class="min-w-12 flex" value="yes">
            <span :class="makeRadioStyle(checked)">Kyllä</span>
          </RadioGroupOption>
        </div>

      </RadioGroup>
    </div>

  </div>

</template>
