import axios from 'axios'

const api = axios.create({
  baseURL: 'http://localhost:5244/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  },
  withCredentials: true // Necessário quando o backend usa AllowCredentials()
})

// Interceptor para requests
api.interceptors.request.use(
  (config) => {
    console.log('Making request to:', config.url)
    return config
  },
  (error) => {
    console.error('Request error:', error)
    return Promise.reject(error)
  }
)

// Interceptor para responses
api.interceptors.response.use(
  (response) => {
    console.log('Response received:', response.status)
    return response
  },
  (error) => {
    console.error('Response error:', error.response?.status, error.response?.data)
    return Promise.reject(error)
  }
)

export default api
