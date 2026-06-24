<template>
  <div class="container">
    <h2>Comprar / Vender</h2>

    <div class="card" style="margin-bottom:24px;">
      <TransactionForm @created="onCreated" />
    </div>

    <h3>Historial de transacciones</h3>
    <div class="card">
      <div v-if="error" class="error">{{ error }}</div>
      <table>
        <thead>
          <tr>
            <th>Fecha</th>
            <th>Cripto</th>
            <th>Tipo</th>
            <th>Cantidad</th>
            <th>Precio</th>
            <th>Total</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="t in transactions" :key="t.id">
            <td>{{ new Date(t.date).toLocaleString() }}</td>
            <td><strong>{{ t.cryptoSymbol }}</strong></td>
            <td :class="t.type === 'Compra' ? 'positive' : 'negative'">{{ t.type }}</td>
            <td>{{ t.quantity }}</td>
            <td>${{ t.priceUsd.toFixed(2) }}</td>
            <td>${{ t.total.toFixed(2) }}</td>
            <td><button class="btn btn-secondary" @click="remove(t.id)">Borrar</button></td>
          </tr>
          <tr v-if="transactions.length === 0">
            <td colspan="7" style="text-align:center; color:#9aa3ad;">Todavía no registraste operaciones.</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import api from '../services/api'
import TransactionForm from '../components/TransactionForm.vue'

const transactions = ref([])
const error = ref('')

async function load() {
  error.value = ''
  try {
    const { data } = await api.getTransactions()
    transactions.value = data
  } catch (err) {
    error.value = 'No se pudieron cargar las transacciones.'
  }
}

function onCreated() {
  load()
}

async function remove(id) {
  try {
    await api.deleteTransaction(id)
    load()
  } catch (err) {
    error.value = 'No se pudo borrar la transacción.'
  }
}

onMounted(load)
</script>
