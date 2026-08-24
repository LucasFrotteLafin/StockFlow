import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '../api/axios'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<any>(null)
  const isLoggedIn = computed(() => !!user.value)

  const login = async (username: string, password: string) => {
    try {
      const response = await api.post('/user/login', { username, password })
      user.value = response.data
      localStorage.setItem('user', JSON.stringify(user.value))
      return response.data
    } catch (error) {
      throw error
    }
  }

  const register = async (username: string, password: string) => {
    try {
      const response = await api.post('/user/register', { username, password })
      return response.data
    } catch (error) {
      throw error
    }
  }

  const logout = () => {
    user.value = null
    localStorage.removeItem('user')
  }

  const loadUser = () => {
    const stored = localStorage.getItem('user')
    if (stored) {
      user.value = JSON.parse(stored)
    }
  }

  return {
    user,
    isLoggedIn,
    login,
    register,
    logout,
    loadUser
  }
})
