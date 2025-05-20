<script setup lang="ts">
import IconPlus from '@/components/icons/IconPlus.vue';
import EntryListing from '../components/EntryListing/EntryListing.vue'
import BrandedButton from '@/components/basics/BrandedButton.vue';

function createNew() {
  console.log("Creating new entry");
}

</script>

<template>
  <main>
    <div>

    </div>
    <div class="flex flex-col gap-1 w-full h-full">
      <div class="flex flex-row justify-start items-center py-2 px-1.5">
        <BrandedButton text="Lisää uusi" :onClick="createNew" icon="plus"
          :styling="{ isPill: true, backgroundColor: 'warm-white', bold: true }" />
      </div>



      <EntryListing class="px-1" />
    </div>


  </main>
</template>
