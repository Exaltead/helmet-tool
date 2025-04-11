<script lang="ts" setup>
import { computed, ref } from 'vue';
import IconMenu from './icons/IconMenu.vue';
import { useRouter } from 'vue-router';
import { clearTokens } from '@/modules/auth-store';

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

const router = useRouter()

const showNavLinks = computed(() => {
  return router.currentRoute.value.name !== "login"
})

function doLogout() {
  clearTokens()
  navOpen.value = false
  router.push({ name: "login" })
}

</script>

<template>
  <div v-if="showNavLinks" class="md:flex bg-brand-primary p-2 md:flex justify-between md:items-center">
    <div class=" flex flex-row gap-6 md:items-center">
      <button @click="toggleNavOpen" class="cursor-pointer md:hidden">
        <IconMenu class="w-6 text-white" />
      </button>
      <h1 class="font-bold text-white">Lukuhaaste</h1>
    </div>


    <div class="left-0 md:static absolute md:w-auto bg-brand-primary w-full" :class="navClassName">
      <div class="md:flex md:flex-row p-4 md:py-0 md:gap-10 items-center">
        <ul class="md:flex md:items-center gap-4 ">
          <li class="text-white" v-for="link in links" :key="link"> {{ link }}</li>
        </ul>
        <div class="h-fit">
          <RouterLink :to="{ name: 'manageChallenges' }" class="text-white">
            <span class="cursor-pointer">Hallinnoi haasteita</span>
          </RouterLink>
        </div>


        <div class="mt-10 md:mt-0 md:pl-4 text-white border w-fit p-2 rounded-lg">
          <button :onClick="doLogout" class="cursor-pointer">Kirjaudu ulos</button>
        </div>
      </div>

    </div>

  </div>
  <div v-else class="bg-brand-primary p-2">
    <h1 class="font-bold text-white">Tikeli-li</h1>
  </div>
</template>
