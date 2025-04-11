<script lang="ts" setup>
import EditChallenge from '@/components/ChallengeManagement/EditChallenge.vue';
import ManageChallenges from '@/components/ChallengeManagement/ManageChallenges.vue';
import type { Challenge } from '@/models/challenge';
import { ref } from 'vue';

// Mockup data for challenges
const data: Challenge[] = [
  {
    id: "1",
    name: 'Haaste 1',
    questions: [
      {
        id: "1",
        question: "Oliko hyvä kirja?",
        number: 0
      },
      {
        id: "2",
        question: "Sisälsikö kettuja?",
        number: 1
      },
      {
        id: "3",
        question: "Oliko LimaMetroja?",
        number: 2
      }

    ],
    status: 'active',
    targetMedia: "Book"
  },
  {
    id: "2",
    name: 'Haaste 2',
    questions: [
      {
        id: "1",
        question: "Oliko huono kirja?",
        number: 0
      },
      {
        id: "2",
        question: "Sisälsikö mursuja?",
        number: 1
      },
      {
        id: "3",
        question: "Oliko limasieniä?",
        number: 2
      }

    ],
    status: 'active',
    targetMedia: "Book"
  }
];

const isEditing = ref(false);
const selectedItem = ref<Challenge | undefined>(undefined);

function handleEdit(challenge: Challenge): void {
  selectedItem.value = challenge;
  isEditing.value = true;
}
function handleCreate(): void {
  selectedItem.value = undefined;
  isEditing.value = true;
}
function handleClose(): void {
  isEditing.value = false;
  selectedItem.value = undefined;
}
function handleSubmitComplete(): void {
  isEditing.value = false;
  selectedItem.value = undefined;
}
</script>

<template>
  <div class="">
    <EditChallenge v-if="isEditing" :target="selectedItem" @submitComplete="() => { handleSubmitComplete() }"
      @close="handleClose" />
    <ManageChallenges v-else :challenges="data" @edit-challenge="(challenge) => handleEdit(challenge)"
      @create-challenge="handleCreate" />

  </div>
</template>
