<script lang="ts" setup>
import type { Challenge } from '@/models/challenge';
import BrandedButton from '../basics/BrandedButton.vue';

const { challenges } = defineProps<{ challenges: Challenge[] }>();
const emit = defineEmits<{
  (e: 'edit-challenge', challenge: Challenge): void;
  (e: 'create-challenge'): void;
}>();

function handleEdit(challenge: Challenge): void {
  emit('edit-challenge', challenge);
}
</script>

<template>
  <div class="p-10">
    <BrandedButton text="Luo uusi haaste" :onClick="() => emit('create-challenge')" />
    <ul class="flex flex-col gap-4">
      <li v-for="challenge in challenges" :key="challenge.id">
        <div class="flex flex-row gap-10 items-center ">
          <span>{{ challenge.name }}</span>
          <BrandedButton text="Muokkaa" :onClick="() => { handleEdit(challenge) }" />
        </div>

      </li>
    </ul>
  </div>
</template>
