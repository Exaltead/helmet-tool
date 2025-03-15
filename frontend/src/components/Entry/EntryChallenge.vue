<script setup lang="ts">
import { RadioGroup, RadioGroupLabel, RadioGroupOption } from "@headlessui/vue"
import { ref } from "vue";

type Question = {
  questionId: number;
  question: string;

}



const questions: Question[] = [
  {
    question: "Oliko kirja hyvä?",
    questionId: 1
  },
  {
    question: "Kuinka monta tähteä annat kirjalle?",
    questionId: 2
  },
  {
    question: "Suosittelisitko kirjaa?",
    questionId: 3
  }

]

const answers = ref<string[]>(questions.map(() => "no"))

function makeRadioStyle(checked: boolean) {
  const adds = checked ? 'bg-brand-primary text-white' : 'border-xxs border-brand-primary'

  return "rounded flex items-center w-full justify-center h-full p-1" + " " + adds
}
</script>


<template>
  <div>
    <h1>Haaste kysymykset</h1>

    <div v-for="(question, index) in questions" :key="index" class="bg-light-gray">
      <RadioGroup v-model="answers[index]">
        <RadioGroupLabel>{{ question.question }}</RadioGroupLabel>
        <div class="flex flex-row gap-4 pl-8">
          <RadioGroupOption v-slot="{ checked }" class="min-w-12 flex" value="yes">
            <span :class="makeRadioStyle(checked)">Kyllä</span>
          </RadioGroupOption>
          <RadioGroupOption value="no" v-slot="{ checked }" class="min-w-12 flex">
            <span :class="makeRadioStyle(checked)">Ei</span>
          </RadioGroupOption>

        </div>

      </RadioGroup>
    </div>

  </div>

</template>
