<script lang="ts" setup>
import { computed, ref } from 'vue';
import Button from '@/components/basics/Button.vue';
import { postLogin } from '@/api/authApi';
import { saveTokens } from '@/modules/auth-store';
import { useRouter } from 'vue-router';

const router = useRouter()

const userName = ref<string>("")
const password = ref<string>("")

const invalidLogin = computed(() => {
  return userName.value.length === 0 || password.value.length === 0
})

async function doLogin() {
  const token = await postLogin(userName.value, password.value)
  if(token) {
    saveTokens(token)
    router.push({ name: "home" })
  }
}

</script>

<template>
  <main>
    <div class="flex flex-row w-full justify-center pt-10">
      <div
        class="flex flex-col justify-center w-fit border border-brand-primary bg-light-gray rounded-lg shadow-lg p-10 pt-4 gap-10">

        <p class="mx-auto text-lg  font-bold">Kirjaudu sisään Tekili-lin</p>
        <div class="flex flex-col gap-10 max-w-md mx-auto">

          <div class="flex flex-col gap-2">
            <label for="username"> Käyttäjätunnus</label>
            <input id="username" type="text" required v-model="userName"
              class="rounded border border-brand-primary bg-white" />
          </div>
          <div class="flex flex-col gap-2">
            <label for="Password"> Salasana</label>
            <input id="password" type="password" required v-model="password"
              class="rounded border border-brand-primary bg-white" />
          </div>

        </div>

        <Button :disabled="invalidLogin" :onClick="doLogin"> Kirjaudu sisään</Button>
      </div>
    </div>


  </main>
</template>
