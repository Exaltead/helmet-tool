<script lang="ts" setup>
import EntryBasics from '@/components/Entry/EntryBasics.vue';
import EntryChallenge from '@/components/Entry/EntryChallenge.vue';
import IconBack from '@/components/icons/IconBack.vue';
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


const challengeId = ref<string>("42e7fe61-a73d-48cf-9b0a-7dfc83cbc00a")

</script>

<template>
  <main>
    <div>
      <button :onClick="router.back" class="p-4">
        <IconBack class="text-brand-primary w-8 h-fit cursor-pointer" />
      </button>
      <div class="px-4 md:px-10 flex flex-col gap-10">
        <EntryBasics :item-id="itemId" @objectDeleted="toLibrary" />
        <TabGroup>
          <TabList>
            <Tab>Haaste 1</Tab>
            <Tab>Haaste 2</Tab>
          </TabList>
          <TabPanels>
            <TabPanel>
              <EntryChallenge :itemId="itemId" :challengeId="challengeId" />
            </TabPanel>
            <TabPanel>
              <EntryChallenge :itemId="itemId" :challengeId="challengeId" />
            </TabPanel>
          </TabPanels>
        </TabGroup>
      </div>
    </div>
  </main>
</template>
