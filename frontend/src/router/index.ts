import { createRouter, createWebHistory } from 'vue-router'
import EntryListingView from '@/views/EntryListingView.vue'
import EntryView from '@/views/EntryView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: EntryListingView,
    },
    {
      path: '/library/:id',
      name: 'libraryItem',
      component: EntryView,
    },
  ],
})

export default router
