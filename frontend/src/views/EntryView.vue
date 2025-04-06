<script lang="ts" setup>
import EntryBasics from '@/components/Entry/EntryBasics.vue';
import EntryChallenge from '@/components/Entry/EntryChallenge.vue';
import IconBack from '@/components/icons/IconBack.vue';
import { useRoute, useRouter } from 'vue-router';
import { z } from 'zod';

const route = useRoute("libraryItem")
const itemId = z.string().parse(route.params.id)

const router = useRouter()

</script>

<template>
  <main>
    <div>
      <button :onClick="router.back" class="p-4">
        <IconBack class="text-brand-primary w-8 h-fit cursor-pointer" />
      </button>
      <div class="px-4 md:px-10 flex flex-col gap-10">
        <EntryBasics :item-id="itemId" />
        <EntryChallenge />
      </div>
    </div>
  </main>
</template>
