import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '../api/axios'

export const useProductStore = defineStore('products', () => {
  const products = ref<any[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const fetchProducts = async () => {
    loading.value = true
    try {
      const response = await api.get('/product')
      products.value = response.data
    } catch (e) {
      error.value = 'Erro ao carregar produtos'
    } finally {
      loading.value = false
    }
  }

  const createProduct = async (data: any) => {
    try {
      const response = await api.post('/product', data)
      products.value.push(response.data)
      return response.data
    } catch (e) {
      throw e
    }
  }

  const updateProduct = async (id: number, data: any) => {
    try {
      const response = await api.put(`/product/${id}`, data)
      const index = products.value.findIndex(p => p.id === id)
      if (index !== -1) {
        products.value[index] = response.data
      }
      return response.data
    } catch (e) {
      throw e
    }
  }

  const deleteProduct = async (id: number) => {
    try {
      await api.delete(`/product/${id}`)
      products.value = products.value.filter(p => p.id !== id)
    } catch (e) {
      throw e
    }
  }

  return {
    products,
    loading,
    error,
    fetchProducts,
    createProduct,
    updateProduct,
    deleteProduct
  }
})
