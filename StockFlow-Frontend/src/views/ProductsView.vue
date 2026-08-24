<template>
  <div class="container mt-4 fade-in">
    <div class="page-header">
      <div>
        <h1>Produtos em Estoque</h1>
        <p class="subtitle">Visualize todos os produtos cadastrados no sistema</p>
      </div>
    </div>

    <!-- Filtros e Busca -->
    <div class="card mb-3">
      <div class="flex-between">
        <div class="form-group" style="flex: 1; margin-bottom: 0; margin-right: 1rem;">
          <input 
            v-model="searchQuery" 
            type="text" 
            placeholder="🔍 Buscar por nome, SKU ou categoria..." 
            style="margin-bottom: 0;"
          >
        </div>
        <div style="display: flex; gap: 0.5rem;">
          <button @click="filterStock = 'all'" :class="['btn', filterStock === 'all' ? 'btn-primary' : 'btn-outline']">
            Todos
          </button>
          <button @click="filterStock = 'low'" :class="['btn', filterStock === 'low' ? 'btn-primary' : 'btn-outline']">
            Estoque Baixo
          </button>
          <button @click="filterStock = 'ok'" :class="['btn', filterStock === 'ok' ? 'btn-primary' : 'btn-outline']">
            Estoque OK
          </button>
        </div>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="productStore.loading" class="text-center">
      <div class="spinner"></div>
    </div>

    <!-- Tabela de Produtos -->
    <div v-else class="card">
      <table>
        <thead>
          <tr>
            <th>Nome</th>
            <th>SKU</th>
            <th>Categoria</th>
            <th>Preço</th>
            <th>Em Estoque</th>
            <th>Mínimo</th>
            <th>Status</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="filteredProducts.length === 0">
            <td colspan="8" class="text-center" style="padding: 3rem;">
              <p style="color: var(--gray); font-size: 1.125rem;">Nenhum produto encontrado</p>
            </td>
          </tr>
          <tr v-for="product in filteredProducts" :key="product.id">
            <td><strong>{{ product.name }}</strong></td>
            <td><code>{{ product.sku }}</code></td>
            <td>{{ product.category }}</td>
            <td><strong style="color: var(--primary);">R$ {{ product.price.toFixed(2) }}</strong></td>
            <td><strong>{{ product.quantityInStock }}</strong></td>
            <td>{{ product.minimumStock }}</td>
            <td>
              <span v-if="product.quantityInStock === 0" class="badge badge-danger">
                Sem Estoque
              </span>
              <span v-else-if="(product.quantityInStock - product.minimumStock) <= 2" class="badge badge-warning">
                Estoque Baixo
              </span>
              <span v-else class="badge badge-success">
                Normal
              </span>
            </td>
            <td>
              <div style="display: flex; gap: 0.5rem;">
                <button @click="editProduct(product)" class="btn btn-small" style="background-color: #3b82f6; color: white;">
                  Editar
                </button>
                <button @click="handleDelete(product.id)" class="btn btn-small btn-danger">
                  Deletar
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal de Edição -->
    <div v-if="showEditModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal-content">
        <div class="modal-header">
          <h2>Editar Produto</h2>
          <button @click="closeModal" class="btn-close">&times;</button>
        </div>
        <form @submit.prevent="handleUpdate">
          <div class="form-group">
            <label>Nome</label>
            <input v-model="editForm.name" type="text" required>
          </div>

          <div class="form-group">
            <label>SKU</label>
            <input v-model="editForm.sku" type="text" required>
          </div>

          <div class="form-group">
            <label>Categoria</label>
            <input v-model="editForm.category" type="text" required>
          </div>

          <div class="form-group">
            <label>Preço</label>
            <input v-model.number="editForm.price" type="number" step="0.01" required>
          </div>

          <div class="form-group">
            <label>Quantidade em Estoque</label>
            <input v-model.number="editForm.quantityInStock" type="number" min="0" required>
            <small style="color: var(--gray); font-size: 0.875rem; display: block; margin-top: 0.25rem;">
              ⚠️ Para registrar entrada/saída oficial, use a página "Movimentações"
            </small>
          </div>

          <div class="form-group">
            <label>Estoque Mínimo</label>
            <input v-model.number="editForm.minimumStock" type="number" required>
          </div>

          <div style="display: flex; gap: 1rem;">
            <button type="submit" class="btn btn-primary" style="flex: 1;">Salvar Alterações</button>
            <button type="button" @click="closeModal" class="btn btn-outline" style="flex: 1;">Cancelar</button>
          </div>
        </form>
      </div>
    </div>

    <!-- Mensagens -->
    <div v-if="message" :class="['alert', `alert-${message.type}`]" style="margin-top: 1rem;">
      {{ message.text }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import { useProductStore } from '../stores/products'
import api from '../api/axios'

const productStore = useProductStore()
const showEditModal = ref(false)
const searchQuery = ref('')
const filterStock = ref('all')
const editForm = reactive({
  id: 0,
  name: '',
  sku: '',
  category: '',
  price: 0,
  quantityInStock: 0,
  minimumStock: 0
})
const message = ref<any>(null)

const filteredProducts = computed(() => {
  let products = productStore.products

  // Filtro de busca
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    products = products.filter(p => 
      p.name.toLowerCase().includes(query) ||
      p.sku.toLowerCase().includes(query) ||
      p.category.toLowerCase().includes(query)
    )
  }

  // Filtro de estoque
  if (filterStock.value === 'low') {
    products = products.filter(p => (p.quantityInStock - p.minimumStock) <= 2 && p.quantityInStock > 0)
  } else if (filterStock.value === 'ok') {
    products = products.filter(p => (p.quantityInStock - p.minimumStock) > 2)
  }

  return products
})

onMounted(() => {
  productStore.fetchProducts()
})

const editProduct = (product: any) => {
  editForm.id = product.id
  editForm.name = product.name
  editForm.sku = product.sku
  editForm.category = product.category
  editForm.price = product.price
  editForm.quantityInStock = product.quantityInStock
  editForm.minimumStock = product.minimumStock
  showEditModal.value = true
}

const closeModal = () => {
  showEditModal.value = false
}

const handleUpdate = async () => {
  try {
    await api.put(`/product/${editForm.id}`, {
      name: editForm.name,
      sku: editForm.sku,
      category: editForm.category,
      price: editForm.price,
      quantityInStock: editForm.quantityInStock,
      minimumStock: editForm.minimumStock
    })
    message.value = { type: 'success', text: 'Produto atualizado com sucesso!' }
    showEditModal.value = false
    await productStore.fetchProducts()
    setTimeout(() => message.value = null, 3000)
  } catch (error: any) {
    message.value = { type: 'danger', text: error.response?.data || 'Erro ao atualizar produto' }
  }
}

const handleDelete = async (id: number) => {
  if (confirm('Tem certeza que deseja deletar este produto?')) {
    try {
      await productStore.deleteProduct(id)
      message.value = { type: 'success', text: 'Produto deletado com sucesso!' }
      setTimeout(() => message.value = null, 3000)
    } catch (error) {
      message.value = { type: 'danger', text: 'Erro ao deletar produto' }
    }
  }
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

/* Modal */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  animation: fadeIn 0.3s ease;
}

.modal-content {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  max-width: 500px;
  width: 90%;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: var(--shadow-lg);
  animation: slideUp 0.3s ease;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.modal-header h2 {
  margin: 0;
}

.btn-close {
  background: none;
  border: none;
  font-size: 2rem;
  cursor: pointer;
  color: var(--gray);
  line-height: 1;
  padding: 0;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  transition: var(--transition);
}

.btn-close:hover {
  background: var(--light-gray);
  color: var(--dark);
}
</style>
