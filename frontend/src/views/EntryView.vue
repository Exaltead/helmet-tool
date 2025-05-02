<script lang="ts" setup>
import { fetchChallenges } from '@/api/challengeApi';
import { fetchLibraryItem } from '@/api/libraryApi';
import EntryBasics from '@/components/Entry/EntryBasics.vue';
import EntryChallenge from '@/components/Entry/EntryChallenge.vue';
import IconBack from '@/components/icons/IconBack.vue';
import type { Challenge } from '@/models/challenge';
import type { Entry } from '@/models/entry';
import { TabGroup, TabList, TabPanel, TabPanels, Tab } from '@headlessui/vue';
import { ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { z } from 'zod';

const route = useRoute("libraryItem")
const itemId = z.string().parse(route.params.id)

const router = useRouter()

function toLibrary() {
  router.push({ name: "home" })
}

const challenges = ref<Challenge[]>([])
async function getChallenges() {
  challenges.value = await fetchChallenges()
}
getChallenges()

const item = ref<Entry | undefined>(undefined)
async function getItem() {
  item.value = await fetchLibraryItem(itemId)
}

getItem()


const challengeIds = [
  "42e7fe61-a73d-48cf-9b0a-7dfc83cbc00a",
  "912b69ca-1394-4109-aa92-9432f380dacd"
]

function makeTabStyle(selected: boolean) {
  return selected
    ? "bg-brand-primary text-white"
    : "bg-white text-brand-primary hover:bg-brand-primary hover:text-white"
}

</script>

<template>
  <main>
    <div>
      <button :onClick="router.back" class="p-4">
        <IconBack class="text-brand-primary w-8 h-fit cursor-pointer" />
      </button>
      <div class="px-4 md:px-10 flex flex-col gap-10" v-if="item">
        <EntryBasics @objectDeleted="toLibrary" :challenges="challenges" @object-edited="getItem" :item="item" />
        <TabGroup>
          <TabList class="flex flex-row gap-6">
            <Tab v-for="challengeId, i in challengeIds" :key="challengeId" v-slot="{ selected }">
              <button :class="makeTabStyle(selected)">
                <span>{{ `Haaste ${i}` }}</span>
              </button>

            </Tab>
          </TabList>
          <TabPanels>
            <TabPanel v-for="challengeId in challengeIds" :key="challengeId">
              <EntryChallenge :itemId="itemId" :challengeId="challengeId" />
            </TabPanel>
          </TabPanels>
        </TabGroup>
      </div>
    </div>
  </main>
</template>
