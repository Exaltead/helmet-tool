<script lang="ts" setup>
import { computed, ref } from 'vue';
import IconMenu from './icons/IconMenu.vue';

const links = ref([
  "Kirjat", "Pelit", "Haasteet"
])

const navOpen = ref(false)

const navClassName = computed<string>(() => {
  return navOpen.value ? "md:block" : "hidden md:block"
})

function toggleNavOpen() {
  navOpen.value = !navOpen.value
}


</script>

<template>
  <div class=" md:flex bg-brand-primary p-2 md:flex justify-between">
    <div class=" flex flex-row gap-6">
      <button @click="toggleNavOpen" class="cursor-pointer md:hidden">
        <IconMenu class="w-6 text-white" />
      </button>
      <h1 class="font-bold text-white">Lukuhaaste</h1>
    </div>


    <div class="left-0 md:static absolute md:w-auto bg-brand-primary w-full" :class="navClassName">
      <ul class="md:flex md:items-center md:gap-4 ">
        <li class="text-white" v-for="link in links" :key="link"> {{ link }}</li>
      </ul>
    </div>

  </div>
</template>
