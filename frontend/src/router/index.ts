import { createRouter, createWebHistory } from 'vue-router'
import EntryListingView from '@/views/EntryListingView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: EntryListingView,
    },
  ],
})

export default router
