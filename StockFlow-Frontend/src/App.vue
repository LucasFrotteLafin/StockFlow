<template>
  <div>
    <Navbar v-if="showNavbar" />
    <router-view />
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from './stores/auth'
import Navbar from './components/Navbar.vue'

const route = useRoute()
const authStore = useAuthStore()

const showNavbar = computed(() => {
  const publicRoutes = ['/login', '/register']
  return !publicRoutes.includes(route.path) && authStore.isLoggedIn
})

onMounted(() => {
  authStore.loadUser()
})
</script>
