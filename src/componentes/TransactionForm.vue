<template>
  <form @submit.prevent="submit">
    <div v-if="error" class="error">{{ error }}</div>

    <label>Cripto (símbolo)</label>
    <select v-model="form.cryptoSymbol">
      <option value="BTC">BTC - Bitcoin</option>
      <option value="ETH">ETH - Ethereum</option>
      <option value="USDT">USDT - Tether</option>
      <option value="SOL">SOL - Solana</option>
      <option value="XRP">XRP - Ripple</option>
      <option value="ADA">ADA - Cardano</option>
      <option value="DOGE">DOGE - Dogecoin</option>
      <option value="LTC">LTC - Litecoin</option>
    </select>

    <label>Operación</label>
    <select v-model="form.type">
      <option value="0">Compra</option>
      <option value="1">Venta</option>
    </select>

    <label>Cantidad</label>
    <input v-model.number="form.quantity" type="number" step="0.00000001" min="0.00000001" required />

    <label>Precio por unidad en USD (opcional - si lo dejás vacío usamos el precio actual de CriptoYa)</label>
    <input v-model.number="form.priceUsd" type="number" step="0.01" min="0" />

    <button class="btn" type="submit" :disabled="loading" style="width:100%">
      {{ loading ? 'Guardando...' : 'Registrar operación' }}
    </button>
  </form>
</template>

<script setup>
import { reactive, ref } from 'vue'
import api from '../services/api'

const emit = defineEmits(['created'])

const form = reactive({
  cryptoSymbol: 'BTC',
  type: '0',
  quantity: null,
  priceUsd: null
})

const loading = ref(false)
const error = ref('')

async function submit() {
  loading.value = true
  error.value = ''
  try {
    const payload = {
      cryptoSymbol: form.cryptoSymbol,
      type: Number(form.type),
      quantity: form.quantity,
      priceUsd: form.priceUsd || null
    }
    const { data } = await api.createTransaction(payload)
    emit('created', data)
    form.quantity = null
    form.priceUsd = null
  } catch (err) {
    error.value = err.response?.data?.message || 'No se pudo registrar la operación.'
  } finally {
    loading.value = false
  }
}
</script>
