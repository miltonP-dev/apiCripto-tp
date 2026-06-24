<template>
  <div class="container">
    <div style="display:flex; justify-content:space-between; align-items:center;">
      <h2>Mi portafolio</h2>
      <button class="btn btn-secondary" @click="loadPortfolio" :disabled="loading">
        {{ loading ? 'Actualizando...' : '🔄 Actualizar precios' }}
      </button>
    </div>

    <div v-if="error" class="error">{{ error }}</div>

    <div class="card" style="margin-bottom:20px; display:flex; gap:32px;">
      <div>
        <div style="color:#9aa3ad; font-size:13px;">Valor actual</div>
        <div style="font-size:24px; font-weight:700;">${{ portfolio.totalCurrentValueUsd.toFixed(2) }}</div>
      </div>
      <div>
        <div style="color:#9aa3ad; font-size:13px;">Invertido</div>
        <div style="font-size:24px; font-weight:700;">${{ portfolio.totalCostBasisUsd.toFixed(2) }}</div>
      </div>
      <div>
        <div style="color:#9aa3ad; font-size:13px;">Ganancia / Pérdida</div>
        <div
          style="font-size:24px; font-weight:700;"
          :class="portfolio.totalGainLossUsd >= 0 ? 'positive' : 'negative'"
        >
          {{ portfolio.totalGainLossUsd >= 0 ? '+' : '' }}${{ portfolio.totalGainLossUsd.toFixed(2) }}
        </div>
      </div>
    </div>

    <div class="card">
      <CryptoTable :holdings="portfolio.holdings" />
    </div>
  </div>
</template>

<script setup>
import { onMounted, reactive, ref } from 'vue'
import api from '../services/api'
import CryptoTable from '../components/CryptoTable.vue'

const loading = ref(false)
const error = ref('')
const portfolio = reactive({
  holdings: [],
  totalCostBasisUsd: 0,
  totalCurrentValueUsd: 0,
  totalGainLossUsd: 0
})

async function loadPortfolio() {
  loading.value = true
  error.value = ''
  try {
    const { data } = await api.getPortfolio()
    Object.assign(portfolio, data)
  } catch (err) {
    error.value = 'No se pudo cargar el portafolio. Verificá que el backend esté corriendo.'
  } finally {
    loading.value = false
  }
}

onMounted(loadPortfolio)
</script>
