import { createRouter, createWebHistory } from "vue-router"
import EntryListingView from "@/views/EntryListingView.vue"
import EntryView from "@/views/EntryView.vue"
import LoginView from "@/views/LoginView.vue"
import { isLoggedIn } from "@/modules/auth-store"

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: EntryListingView,
    },
    {
      path: "/library/:id",
      name: "libraryItem",
      component: EntryView,
    },
    {
      path: "/login",
      name: "login",
      component: LoginView,
    },
  ],
})

router.beforeEach((to) => {
  const isAuthenticated = isLoggedIn()

  if (to.name !== "login" && !isAuthenticated) {
    return { name: "login" }
  }
  if (to.name === "login" && isAuthenticated) {
    return { name: "home" }
  }
})

export default router
