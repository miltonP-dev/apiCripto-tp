<template>
  <div class="auth-page">
    <!-- Fondo animado -->
    <div class="bg-grid"></div>
    <div class="bg-glow"></div>

    <div class="auth-container">
      <!-- Logo / marca -->
      <div class="auth-brand">
        <div class="brand-icon">
          <svg width="40" height="40" viewBox="0 0 40 40" fill="none">
            <circle cx="20" cy="20" r="20" fill="url(#g1)"/>
            <path d="M13 20l5 5 9-10" stroke="#fff" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"/>
            <defs>
              <linearGradient id="g1" x1="0" y1="0" x2="40" y2="40" gradientUnits="userSpaceOnUse">
                <stop stop-color="#3B82F6"/>
                <stop offset="1" stop-color="#06B6D4"/>
              </linearGradient>
            </defs>
          </svg>
        </div>
        <h1 class="brand-title">CryptoVault</h1>
        <p class="brand-subtitle">Tu monedero digital seguro</p>
      </div>

      <!-- Card de login -->
      <div class="auth-card">
        <h2 class="form-title">Iniciar sesión</h2>

        <div v-if="error" class="alert alert-error">
          <span>⚠</span> {{ error }}
        </div>

        <form @submit.prevent="handleLogin" novalidate>
          <div class="field">
            <label for="email">Email</label>
            <input
              id="email"
              v-model="form.email"
              type="email"
              placeholder="tu@email.com"
              autocomplete="email"
              required
              :class="{ 'input-error': touched.email && !form.email }"
              @blur="touched.email = true"
            />
          </div>

          <div class="field">
            <label for="password">
              Contraseña
              <router-link to="/forgot" class="forgot-link">¿Olvidaste?</router-link>
            </label>
            <div class="input-wrapper">
              <input
                id="password"
                v-model="form.password"
                :type="showPassword ? 'text' : 'password'"
                placeholder="••••••••"
                autocomplete="current-password"
                required
              />
              <button type="button" class="toggle-pw" @click="showPassword = !showPassword">
                {{ showPassword ? '🙈' : '👁' }}
              </button>
            </div>
          </div>

          <button
            type="submit"
            class="btn btn-primary btn-full"
            :disabled="loading"
          >
            <span v-if="loading" class="spinner"></span>
            {{ loading ? 'Verificando...' : 'Ingresar' }}
          </button>
        </form>

        <div class="divider">o</div>

        <p class="auth-footer">
          ¿No tenés cuenta?
          <router-link to="/register" class="link-accent">Crear cuenta</router-link>
        </p>
      </div>

      <!-- Hint de seguridad -->
      <p class="security-hint">
        🔒 Conexión cifrada con TLS 1.3
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

// Estado del formulario
const form = reactive({ email: '', password: '' })
const loading = ref(false)
const error = ref('')
const showPassword = ref(false)
const touched = reactive({ email: false })

// Función principal de login
async function handleLogin() {
  if (!form.email || !form.password) {
    error.value = 'Completá todos los campos.'
    return
  }

  loading.value = true
  error.value = ''

  try {
    const { data } = await api.login(form)
    setSession(data.token, data.username)
    router.push('/dashboard')
  } catch (err) {
    const msg = err.response?.data?.message
    if (err.response?.status === 401) {
      error.value = 'Email o contraseña incorrectos.'
    } else if (err.response?.status === 429) {
      error.value = 'Demasiados intentos. Esperá un momento.'
    } else {
      error.value = msg || 'No se pudo conectar con el servidor.'
    }
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.auth-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  overflow: hidden;
  padding: 24px;
}

/* Fondo decorativo */
.bg-grid {
  position: absolute;
  inset: 0;
  background-image:
    linear-gradient(rgba(59,130,246,0.03) 1px, transparent 1px),
    linear-gradient(90deg, rgba(59,130,246,0.03) 1px, transparent 1px);
  background-size: 48px 48px;
  pointer-events: none;
}

.bg-glow {
  position: absolute;
  top: -200px;
  left: 50%;
  transform: translateX(-50%);
  width: 600px;
  height: 600px;
  background: radial-gradient(circle, rgba(59,130,246,0.12) 0%, transparent 70%);
  pointer-events: none;
}

/* Contenedor central */
.auth-container {
  width: 100%;
  max-width: 420px;
  position: relative;
  z-index: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 28px;
}

/* Marca */
.auth-brand {
  text-align: center;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
}

.brand-icon {
  filter: drop-shadow(0 0 20px rgba(59,130,246,0.5));
}

.brand-title {
  font-family: var(--font-mono);
  font-size: 26px;
  font-weight: 700;
  background: linear-gradient(135deg, #3B82F6, #06B6D4);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.brand-subtitle {
  color: var(--text-muted);
  font-size: 14px;
}

/* Card */
.auth-card {
  background: var(--bg-surface);
  border: 1px solid var(--border);
  border-radius: var(--radius-lg);
  padding: 32px;
  width: 100%;
  box-shadow: var(--shadow-card);
}

.form-title {
  font-size: 20px;
  font-weight: 600;
  color: var(--text-primary);
  margin-bottom: 24px;
}

/* Label con link a la derecha */
.field label {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.forgot-link {
  font-size: 12px;
  color: var(--accent-blue);
  text-decoration: none;
  font-weight: 400;
  text-transform: none;
  letter-spacing: 0;
}
.forgot-link:hover { text-decoration: underline; }

/* Input con botón de ver contraseña */
.input-wrapper {
  position: relative;
}

.input-wrapper input {
  background: var(--bg-elevated);
  border: 1px solid var(--border);
  border-radius: var(--radius-sm);
  color: var(--text-primary);
  font-family: var(--font-ui);
  font-size: 15px;
  padding: 12px 44px 12px 14px;
  transition: border-color 0.2s, box-shadow 0.2s;
  outline: none;
  width: 100%;
}
.input-wrapper input:focus {
  border-color: var(--accent-blue);
  box-shadow: 0 0 0 3px rgba(59,130,246,0.15);
}

.toggle-pw {
  position: absolute;
  right: 10px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  cursor: pointer;
  font-size: 16px;
  padding: 4px;
  opacity: 0.6;
  transition: opacity 0.2s;
}
.toggle-pw:hover { opacity: 1; }

/* Input con error */
.input-error {
  border-color: var(--accent-red) !important;
}

/* Auth footer */
.auth-footer {
  text-align: center;
  font-size: 14px;
  color: var(--text-secondary);
}

.link-accent {
  color: var(--accent-blue);
  font-weight: 600;
  text-decoration: none;
}
.link-accent:hover { text-decoration: underline; }

/* Security hint */
.security-hint {
  font-size: 12px;
  color: var(--text-muted);
}
</style>
