import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '../api/axios'

export interface UserRequest {
  id: number
  username: string
  status: 'Pendente' | 'Aprovado' | 'Rejeitado'
  requestDate: string
}

export interface ApprovedUser {
  id: number
  username: string
  role: string
}

export const usePeopleStore = defineStore('people', () => {
  const pendingRequests = ref<UserRequest[]>([])
  const approvedUsers = ref<ApprovedUser[]>([])
  const loading = ref(false)
  const error = ref('')

  const fetchPendingRequests = async () => {
    try {
      loading.value = true
      error.value = ''
      const response = await api.get('/userrequest')
      pendingRequests.value = response.data
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Erro ao carregar solicitações'
      throw err
    } finally {
      loading.value = false
    }
  }

  const fetchApprovedUsers = async () => {
    try {
      loading.value = true
      error.value = ''
      const response = await api.get('/userrequest/approved')
      approvedUsers.value = response.data
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Erro ao carregar usuários aprovados'
      throw err
    } finally {
      loading.value = false
    }
  }

  const approveUser = async (requestId: number) => {
    try {
      loading.value = true
      error.value = ''
      await api.post(`/userrequest/${requestId}/approve`)
      pendingRequests.value = pendingRequests.value.filter(r => r.id !== requestId)
      await fetchApprovedUsers()
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Erro ao aprovar usuário'
      throw err
    } finally {
      loading.value = false
    }
  }

  const denyUser = async (requestId: number) => {
    try {
      loading.value = true
      error.value = ''
      await api.post(`/userrequest/${requestId}/reject`)
      pendingRequests.value = pendingRequests.value.filter(r => r.id !== requestId)
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Erro ao rejeitar usuário'
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    pendingRequests,
    approvedUsers,
    loading,
    error,
    fetchPendingRequests,
    fetchApprovedUsers,
    approveUser,
    denyUser
  }
})
