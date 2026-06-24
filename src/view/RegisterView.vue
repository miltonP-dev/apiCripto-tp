<template>
  <div class="container" style="max-width: 400px; margin-top: 60px;">
    <div class="card">
      <h2>Crear cuenta</h2>
      <div v-if="error" class="error">{{ error }}</div>

      <form @submit.prevent="handleRegister">
        <label>Usuario</label>
        <input v-model="form.username" type="text" required />

        <label>Email</label>
        <input v-model="form.email" type="email" required />

        <label>Contraseña (mínimo 6 caracteres)</label>
        <input v-model="form.password" type="password" minlength="6" required />

        <button class="btn" type="submit" :disabled="loading" style="width:100%">
          {{ loading ? 'Creando...' : 'Crear cuenta' }}
        </button>
      </form>

      <p style="margin-top:16px; font-size:14px;">
        ¿Ya tenés cuenta? <router-link to="/login">Iniciá sesión</router-link>
      </p>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import api from '../services/api'
import { useAuth } from '../stores/auth'

const router = useRouter()
const { setSession } = useAuth()

const form = reactive({ username: '', email: '', password: '' })
const loading = ref(false)
const error = ref('')

async function handleRegister() {
  loading.value = true
  error.value = ''
  try {
    const { data } = await api.register(form)
    setSession(data.token, data.username)
    router.push('/dashboard')
  } catch (err) {
    error.value = err.response?.data?.message || 'No se pudo crear la cuenta.'
  } finally {
    loading.value = false
  }
}
</script>
