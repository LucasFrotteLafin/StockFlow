import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '../api/axios'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<any>(null)
  const token = ref<string | null>(null)
  const isLoggedIn = computed(() => !!user.value && !!token.value)

  const register = async (username: string, password: string) => {
    try {
      const response = await api.post('/userrequest/register', { username, password })
      return response.data
    } catch (error) {
      throw error
    }
  }

  const login = async (username: string, password: string) => {
    try {
      const response = await api.post('/user/login', { username, password })
      user.value = response.data
      token.value = response.data.token
      
      sessionStorage.setItem('user', JSON.stringify(user.value))
      sessionStorage.setItem('token', response.data.token)
      
      api.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`
      
      return response.data
    } catch (error) {
      throw error
    }
  }

  const logout = () => {
    user.value = null
    token.value = null
    sessionStorage.removeItem('user')
    sessionStorage.removeItem('token')
    delete api.defaults.headers.common['Authorization']
  }

  const loadUser = () => {
    const storedUser = sessionStorage.getItem('user')
    const storedToken = sessionStorage.getItem('token')
    
    if (storedUser && storedToken) {
      user.value = JSON.parse(storedUser)
      token.value = storedToken
      api.defaults.headers.common['Authorization'] = `Bearer ${storedToken}`
    }
  }

  return {
    user,
    token,
    isLoggedIn,
    login,
    register,
    logout,
    loadUser
  }
})
