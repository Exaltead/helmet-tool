<script lang="ts" setup>
import { computed, ref } from 'vue';
import IconMenu from './icons/IconMenu.vue';

const links = ref([
  "Kirjat", "Pelit", "Haasteet"
])

const navOpen = ref(false)

const navClassName = computed<string>(() => {
  return navOpen.value ? "left-0" : "left-[-100%]"
})

function toggleNavOpen() {
  navOpen.value = !navOpen.value
}


</script>

<template>
  <div class=" md:flex bg-brand-primary p-2 md:flex justify-between">
    <div class="md:hidden" >
      <button @click="toggleNavOpen" class="cursor-pointer">
        <IconMenu class="w-6 text-white" />
      </button>
    </div>

    <div>
      <ul class="md:flex md:items-center md:gap-4 md:static absolute md:w-auto w-full bg-brand-primary"
        :class="navClassName">
        <li v-for="link in links" :key="link"> {{ link }}</li>
      </ul>
    </div>

  </div>
</template>
