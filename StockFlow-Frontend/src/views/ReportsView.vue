<template>
  <div class="container mt-4 fade-in">
    <div class="page-header">
      <div>
        <h1>Relatórios e Análises</h1>
        <p class="subtitle">Visualize estatísticas e métricas do seu estoque</p>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="text-center">
      <div class="spinner"></div>
    </div>

    <!-- Gráficos -->
    <div v-else class="charts-grid">
      <!-- Gráfico: Produtos que Mais Saem vs Menos Saem -->
      <div class="card chart-card">
        <h2>Produtos Mais Vendidos</h2>
        <p class="chart-subtitle">Top produtos com mais vendas</p>
        <div class="chart-container">
          <canvas ref="exitChart"></canvas>
        </div>
        <div class="chart-legend">
          <div v-for="(item, index) in exitData" :key="index" class="legend-item">
            <div class="legend-color" :style="{ backgroundColor: exitColors[index] }"></div>
            <span>{{ item.name }}: <strong>{{ item.value }} vendas</strong></span>
          </div>
        </div>
      </div>

      <!-- Gráfico: Produtos com Mais vs Menos Quantidade em Estoque -->
      <div class="card chart-card">
        <h2>Quantidade em Estoque</h2>
        <p class="chart-subtitle">Produtos com maior quantidade atual</p>
        <div class="chart-container">
          <canvas ref="stockChart"></canvas>
        </div>
        <div class="chart-legend">
          <div v-for="(item, index) in stockData" :key="index" class="legend-item">
            <div class="legend-color" :style="{ backgroundColor: stockColors[index] }"></div>
            <span>{{ item.name }}: <strong>{{ item.value }} unidades</strong></span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../api/axios'

const loading = ref(true)
const exitChart = ref<HTMLCanvasElement | null>(null)
const stockChart = ref<HTMLCanvasElement | null>(null)

const exitData = ref<{ name: string; value: number }[]>([])
const stockData = ref<{ name: string; value: number }[]>([])

const exitColors = ['#ef4444', '#f59e0b', '#10b981', '#3b82f6', '#8b5cf6', '#ec4899', '#14b8a6', '#f97316']
const stockColors = ['#3b82f6', '#8b5cf6', '#10b981', '#f59e0b', '#ef4444', '#ec4899', '#14b8a6', '#f97316']

onMounted(async () => {
  await fetchData()
  drawCharts()
})

const fetchData = async () => {
  try {
    const [productsRes, movementsRes] = await Promise.all([
      api.get('/product'),
      api.get('/movement')
    ])

    const products = productsRes.data
    const movements = movementsRes.data

    // Calcular saídas por produto - SEM DUPLICATAS DE SKU
    const exitCounts: { [key: string]: { count: number, name: string, sku: string } } = {}
    
    movements
      .filter((m: any) => m.type === 'Saída')
      .forEach((m: any) => {
        const product = products.find((p: any) => p.id === m.productId)
        if (product) {
          const sku = product.sku
          if (!exitCounts[sku]) {
            exitCounts[sku] = {
              count: 0,
              name: product.name,
              sku: product.sku
            }
          }
          exitCounts[sku].count += m.quantity
        }
      })

    // Ordenar por saídas (mais para menos) e pegar top 8
    const sortedExits = Object.values(exitCounts)
      .sort((a, b) => b.count - a.count)
      .slice(0, 8)

    exitData.value = sortedExits.map(p => ({ 
      name: `${p.name} (${p.sku})`, 
      value: p.count 
    }))

    // Produtos ordenados por quantidade em estoque - SEM DUPLICATAS DE SKU
    const stockBySku: { [key: string]: { name: string, sku: string, value: number } } = {}
    
    products
      .filter((p: any) => p.quantityInStock > 0)
      .forEach((p: any) => {
        if (!stockBySku[p.sku]) {
          stockBySku[p.sku] = {
            name: p.name,
            sku: p.sku,
            value: p.quantityInStock
          }
        } else {
          // Se já existe, somar as quantidades
          stockBySku[p.sku].value += p.quantityInStock
        }
      })

    // Ordenar e pegar top 8
    const sortedStock = Object.values(stockBySku)
      .sort((a, b) => b.value - a.value)
      .slice(0, 8)

    stockData.value = sortedStock.map(p => ({ 
      name: `${p.name} (${p.sku})`, 
      value: p.value 
    }))

    loading.value = false
  } catch (error) {
    console.error('Erro ao carregar dados:', error)
    loading.value = false
  }
}

const drawCharts = () => {
  if (exitChart.value && exitData.value.length > 0) {
    drawPieChart(exitChart.value, exitData.value, exitColors)
  }
  if (stockChart.value && stockData.value.length > 0) {
    drawPieChart(stockChart.value, stockData.value, stockColors)
  }
}

const drawPieChart = (canvas: HTMLCanvasElement, data: { name: string; value: number }[], colors: string[]) => {
  const ctx = canvas.getContext('2d')
  if (!ctx) return

  // Definir tamanho exato e quadrado
  const size = 400
  canvas.width = size
  canvas.height = size

  const total = data.reduce((sum, item) => sum + item.value, 0)
  const centerX = size / 2
  const centerY = size / 2
  const radius = (size / 2) - 40

  let currentAngle = -Math.PI / 2

  // Limpar canvas
  ctx.clearRect(0, 0, size, size)

  data.forEach((item, index) => {
    const sliceAngle = (item.value / total) * 2 * Math.PI

    // Desenhar fatia
    ctx.beginPath()
    ctx.moveTo(centerX, centerY)
    ctx.arc(centerX, centerY, radius, currentAngle, currentAngle + sliceAngle)
    ctx.closePath()
    ctx.fillStyle = colors[index % colors.length]
    ctx.fill()
    ctx.strokeStyle = '#fff'
    ctx.lineWidth = 4
    ctx.stroke()

    currentAngle += sliceAngle
  })
}
</script>

<style scoped>
.page-header {
  margin-bottom: 2rem;
}

.page-header h1 {
  margin-bottom: 0.5rem;
}

.subtitle {
  color: var(--gray);
  font-size: 1.125rem;
  margin: 0;
}

.charts-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(500px, 1fr));
  gap: 2rem;
}

.chart-card {
  padding: 2rem;
}

.chart-card h2 {
  font-size: 1.5rem;
  margin-bottom: 0.5rem;
  color: var(--dark);
}

.chart-subtitle {
  color: var(--gray);
  font-size: 0.95rem;
  margin-bottom: 2rem;
}

.chart-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 2rem;
  min-height: 400px;
}

.chart-container canvas {
  width: 400px !important;
  height: 400px !important;
  max-width: 100%;
}

.chart-legend {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.legend-item {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 0.95rem;
}

.legend-color {
  width: 20px;
  height: 20px;
  border-radius: 4px;
  flex-shrink: 0;
}

@media (max-width: 1200px) {
  .charts-grid {
    grid-template-columns: 1fr;
  }
}
</style>
