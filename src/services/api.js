import axios from 'axios'
import { useAuth } from '../stores/auth'

const api = axios.create({
  baseURL: 'http://localhost:5080/api'
})

api.interceptors.request.use((config) => {
  const { state } = useAuth()
  if (state.token) {
    config.headers.Authorization = `Bearer ${state.token}`
  }
  return config
})

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      const { clearSession } = useAuth()
      clearSession()
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export default {
  register(payload) {
    return api.post('/auth/register', payload)
  },
  login(payload) {
    return api.post('/auth/login', payload)
  },

  getTransactions() {
    return api.get('/transactions')
  },
  createTransaction(payload) {
    return api.post('/transactions', payload)
  },
  deleteTransaction(id) {
    return api.delete(`/transactions/${id}`)
  },

  getPortfolio() {
    return api.get('/portfolio')
  },

  getPrice(symbol) {
    return api.get(`/prices/${symbol}`)
  }
}
