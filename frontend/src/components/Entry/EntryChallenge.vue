<script setup lang="ts">
import { fetchQuestions } from "@/api/questionsApi";
import { type Question } from "@/models/question";
import { RadioGroup, RadioGroupLabel, RadioGroupOption } from "@headlessui/vue"
import { computed, ref } from "vue";



const questions = ref<Question[]>([])

getQuestions()

const answers = computed(() => questions.value.map(() => "no"))
function makeRadioStyle(checked: boolean) {
  const adds = checked ? 'bg-brand-primary text-white' : 'border border-brand-primary'

  return "rounded flex items-center w-full justify-center h-full p-1" + " " + adds
}

async function getQuestions() {
  const data = await fetchQuestions()

  questions.value = data
}
</script>


<template>
  <div>
    <h1>Haaste kysymykset</h1>

    <div v-for="(question, index) in questions" :key="index" class="bg-light-gray p-2">
      <RadioGroup v-model="answers[index]" class="flex flex-col gap-1">
        <RadioGroupLabel>{{ question.question }}</RadioGroupLabel>
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
