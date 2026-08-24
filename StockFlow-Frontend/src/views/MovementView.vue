<template>
  <div class="container mt-4 fade-in">
    <div class="page-header">
      <div>
        <h1>Movimentações de Estoque</h1>
        <p class="subtitle">Registre entradas e saídas de produtos</p>
      </div>
    </div>

    <!-- Cards de Entrada/Saída e Adicionar Produto -->
    <div class="grid grid-3" style="margin-bottom: 2rem;">
      <!-- Adicionar Novo Produto -->
      <div class="card">
        <div class="card-header-icon green">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
        </div>
        <h2>Adicionar Produto</h2>
        <form @submit.prevent="handleAddProduct">
          <div class="form-group">
            <label>Nome</label>
            <input v-model="newProductForm.name" type="text" placeholder="Ex: Notebook Dell" required>
          </div>

          <div class="form-group">
            <label>SKU (Código Único)</label>
            <input v-model="newProductForm.sku" type="text" placeholder="Ex: NOTE-DELL-001" required>
          </div>

          <div class="form-group">
            <label>Categoria</label>
            <input v-model="newProductForm.category" type="text" placeholder="Ex: Eletrônicos" required>
          </div>

          <div class="form-group">
            <label>Preço (R$)</label>
            <input v-model.number="newProductForm.price" type="number" step="0.01" min="0" placeholder="0.00" required>
          </div>

          <div class="form-group">
            <label>Estoque Mínimo</label>
            <input v-model.number="newProductForm.minimumStock" type="number" min="0" placeholder="5" required>
          </div>

          <div class="form-group">
            <label>Quantidade Inicial</label>
            <input v-model.number="newProductForm.initialStock" type="number" min="0" placeholder="10" required>
          </div>

          <button type="submit" class="btn btn-secondary w-full">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" style="width: 20px; height: 20px;">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
            Adicionar Produto
          </button>
        </form>
      </div>

      <!-- Registrar Entrada -->
      <div class="card">
        <div class="card-header-icon blue">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 11l5-5m0 0l5 5m-5-5v12" />
          </svg>
        </div>
        <h2>Registrar Entrada</h2>
        <form @submit.prevent="handleEntrada">
          <div class="form-group">
            <label>Selecionar Produto</label>
            <select v-model="entradaForm.productId" required>
              <option value="">Escolha um produto</option>
              <option v-for="product in products" :key="product.id" :value="product.id">
                {{ product.name }} - Estoque: {{ product.quantityInStock }}
              </option>
            </select>
          </div>

          <div class="form-group">
            <label>Quantidade</label>
            <input v-model.number="entradaForm.quantity" type="number" min="1" placeholder="Ex: 10" required>
          </div>

          <div class="form-group">
            <label>Motivo</label>
            <input v-model="entradaForm.reason" type="text" placeholder="Ex: Compra ao fornecedor" required>
          </div>

          <button type="submit" class="btn btn-primary w-full">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" style="width: 20px; height: 20px;">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
            </svg>
            Registrar Entrada
          </button>
        </form>
      </div>

      <!-- Registrar Saída -->
      <div class="card">
        <div class="card-header-icon orange">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 13l-5 5m0 0l-5-5m5 5V6" />
          </svg>
        </div>
        <h2>Registrar Saída</h2>
        <form @submit.prevent="handleSaida">
          <div class="form-group">
            <label>Selecionar Produto</label>
            <select v-model="saidaForm.productId" required>
              <option value="">Escolha um produto</option>
              <option 
                v-for="product in productsWithStock" 
                :key="product.id" 
                :value="product.id"
              >
                {{ product.name }} - Disponível: {{ product.quantityInStock }}
              </option>
            </select>
          </div>

          <div class="form-group">
            <label>Quantidade</label>
            <input v-model.number="saidaForm.quantity" type="number" min="1" placeholder="Ex: 5" required>
          </div>

          <div class="form-group">
            <label>Motivo</label>
            <input v-model="saidaForm.reason" type="text" placeholder="Ex: Venda ao cliente" required>
          </div>

          <button type="submit" class="btn btn-danger w-full">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" style="width: 20px; height: 20px;">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
            </svg>
            Registrar Saída
          </button>
        </form>
      </div>
    </div>

    <!-- Histórico de Movimentações -->
    <div class="card">
      <div class="flex-between mb-3">
        <h2 style="margin: 0;">Histórico de Movimentações</h2>
        <button @click="fetchMovements" class="btn btn-outline btn-small">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" style="width: 18px; height: 18px;">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
          </svg>
          Atualizar
        </button>
      </div>
      
      <table>
        <thead>
          <tr>
            <th>Data</th>
            <th>Responsável</th>
            <th>Produto</th>
            <th>Tipo</th>
            <th>Quantidade</th>
            <th>Motivo</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="movements.length === 0">
            <td colspan="6" class="text-center" style="padding: 3rem;">
              <p style="color: var(--gray); font-size: 1.125rem;">Nenhuma movimentação registrada</p>
            </td>
          </tr>
          <tr v-for="movement in movements" :key="movement.id">
            <td>{{ formatDate(movement.movementDate) }}</td>
            <td><strong style="color: var(--primary);">{{ movement.userName }}</strong></td>
            <td><strong>{{ getProductName(movement.productId) }}</strong></td>
            <td>
              <span v-if="movement.type === 'Entrada'" class="badge badge-success">
                ↑ Entrada
              </span>
              <span v-else class="badge badge-danger">
                ↓ Saída
              </span>
            </td>
            <td><strong>{{ movement.quantity }}</strong></td>
            <td>{{ movement.reason }}</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Mensagens -->
    <div v-if="message" :class="['alert', `alert-${message.type}`]" style="margin-top: 1.5rem;">
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" style="width: 20px; height: 20px;">
        <path v-if="message.type === 'success'" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
        <path v-else stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
      {{ message.text }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import { useAuthStore } from '../stores/auth'
import api from '../api/axios'

const authStore = useAuthStore()
const products = ref<any[]>([])
const movements = ref<any[]>([])
const message = ref<any>(null)

const productsWithStock = computed(() => {
  return products.value.filter(p => p.quantityInStock > 0)
})

const newProductForm = reactive({
  name: '',
  sku: '',
  category: '',
  price: 0,
  minimumStock: 0,
  initialStock: 0
})

const entradaForm = reactive({
  productId: '',
  quantity: 1,
  reason: '',
  type: 'Entrada'
})

const saidaForm = reactive({
  productId: '',
  quantity: 1,
  reason: '',
  type: 'Saída'
})

onMounted(async () => {
  await fetchProducts()
  await fetchMovements()
})

const fetchProducts = async () => {
  try {
    const response = await api.get('/product')
    products.value = response.data
  } catch (error) {
    console.error('Erro ao carregar produtos:', error)
  }
}

const fetchMovements = async () => {
  try {
    const response = await api.get('/movement')
    movements.value = response.data.sort((a: any, b: any) => 
      new Date(b.movementDate).getTime() - new Date(a.movementDate).getTime()
    )
  } catch (error) {
    console.error('Erro ao carregar movimentações:', error)
  }
}

const handleAddProduct = async () => {
  try {
    const authStore = useAuthStore()
    const userId = authStore.user?.id
    
    if (!userId) {
      message.value = { type: 'danger', text: 'Usuário não autenticado. Faça login novamente.' }
      return
    }

    const response = await api.post('/product', {
      name: newProductForm.name,
      sku: newProductForm.sku,
      category: newProductForm.category,
      price: newProductForm.price,
      minimumStock: newProductForm.minimumStock
    })

    // Se tem estoque inicial, registrar entrada
    if (newProductForm.initialStock > 0) {
      await api.post('/movement', {
        productId: response.data.id,
        quantity: newProductForm.initialStock,
        type: 'Entrada',
        reason: 'Estoque inicial',
        userId: userId
      })
    }

    message.value = { type: 'success', text: 'Produto adicionado com sucesso!' }
    
    // Limpar formulário
    newProductForm.name = ''
    newProductForm.sku = ''
    newProductForm.category = ''
    newProductForm.price = 0
    newProductForm.minimumStock = 0
    newProductForm.initialStock = 0

    await fetchProducts()
    await fetchMovements()
    setTimeout(() => message.value = null, 3000)
  } catch (error: any) {
    console.error('Erro completo:', error)
    const errorMsg = error.response?.data || error.message || 'Erro ao adicionar produto'
    message.value = { type: 'danger', text: errorMsg }
    setTimeout(() => message.value = null, 5000)
  }
}

const handleEntrada = async () => {
  try {
    const authStore = useAuthStore()
    const userId = authStore.user?.id
    
    if (!userId) {
      message.value = { type: 'danger', text: 'Usuário não autenticado' }
      return
    }

    await api.post('/movement', {
      ...entradaForm,
      userId: userId
    })
    message.value = { type: 'success', text: 'Entrada registrada com sucesso!' }
    entradaForm.productId = ''
    entradaForm.quantity = 1
    entradaForm.reason = ''
    await fetchProducts()
    await fetchMovements()
    setTimeout(() => message.value = null, 3000)
  } catch (error: any) {
    message.value = { type: 'danger', text: error.response?.data || 'Erro ao registrar entrada' }
    setTimeout(() => message.value = null, 5000)
  }
}

const handleSaida = async () => {
  const product = products.value.find(p => p.id == saidaForm.productId)
  if (product && saidaForm.quantity > product.quantityInStock) {
    message.value = { 
      type: 'danger', 
      text: `Quantidade indisponível! Estoque atual: ${product.quantityInStock}` 
    }
    setTimeout(() => message.value = null, 5000)
    return
  }

  try {
    const authStore = useAuthStore()
    const userId = authStore.user?.id
    
    if (!userId) {
      message.value = { type: 'danger', text: 'Usuário não autenticado' }
      return
    }

    await api.post('/movement', {
      ...saidaForm,
      userId: userId
    })
    message.value = { type: 'success', text: 'Saída registrada com sucesso!' }
    saidaForm.productId = ''
    saidaForm.quantity = 1
    saidaForm.reason = ''
    await fetchProducts()
    await fetchMovements()
    setTimeout(() => message.value = null, 3000)
  } catch (error: any) {
    message.value = { type: 'danger', text: error.response?.data || 'Erro ao registrar saída' }
    setTimeout(() => message.value = null, 5000)
  }
}

const getProductName = (productId: number) => {
  const product = products.value.find(p => p.id === productId)
  return product?.name || 'Produto não encontrado'
}

const formatDate = (date: string) => {
  const d = new Date(date)
  return d.toLocaleDateString('pt-BR') + ' ' + d.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' })
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

.card-header-icon {
  width: 56px;
  height: 56px;
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 1.25rem;
}

.card-header-icon svg {
  width: 28px;
  height: 28px;
  color: white;
}

.card-header-icon.blue {
  background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%);
}

.card-header-icon.green {
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
}

.card-header-icon.orange {
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
}

.card h2 {
  font-size: 1.25rem;
  margin-bottom: 1.25rem;
}

@media (max-width: 1024px) {
  .grid-3 {
    grid-template-columns: 1fr;
  }
}
</style>
