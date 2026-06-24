<template>
  <table>
    <thead>
      <tr>
        <th>Cripto</th>
        <th>Cantidad</th>
        <th>Precio prom. compra</th>
        <th>Precio actual</th>
        <th>Valor actual</th>
        <th>Ganancia / Pérdida</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="h in holdings" :key="h.cryptoSymbol">
        <td><strong>{{ h.cryptoSymbol }}</strong></td>
        <td>{{ h.quantity }}</td>
        <td>${{ h.avgBuyPriceUsd.toFixed(2) }}</td>
        <td>{{ h.currentPriceUsd ? '$' + h.currentPriceUsd.toFixed(2) : '—' }}</td>
        <td>{{ h.currentValueUsd != null ? '$' + h.currentValueUsd.toFixed(2) : '—' }}</td>
        <td :class="(h.gainLossUsd ?? 0) >= 0 ? 'positive' : 'negative'">
          <span v-if="h.gainLossUsd != null">
            {{ h.gainLossUsd >= 0 ? '+' : '' }}${{ h.gainLossUsd.toFixed(2) }}
            ({{ h.gainLossPercent.toFixed(2) }}%)
          </span>
          <span v-else>—</span>
        </td>
      </tr>
      <tr v-if="holdings.length === 0">
        <td colspan="6" style="text-align:center; color:#9aa3ad;">
          Todavía no tenés criptos en tu portafolio. Cargá una compra en "Transacciones".
        </td>
      </tr>
    </tbody>
  </table>
</template>

<script setup>
defineProps({
  holdings: {
    type: Array,
    default: () => []
  }
})
</script>
