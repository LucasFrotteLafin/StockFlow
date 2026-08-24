<template>
  <div class="dashboard-container fade-in">
    <div class="dashboard-header">
      <div>
        <h1>Dashboard</h1>
        <p class="subtitle">Bem-vindo de volta! Aqui está um resumo do seu estoque.</p>
      </div>
    </div>

    <!-- Cards de Estatísticas -->
    <div class="stats-grid">
      <div class="stat-card blue">
        <div class="stat-icon">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
          </svg>
        </div>
        <div class="stat-content">
          <h3>Total de Produtos</h3>
          <p class="stat-number">{{ stats.totalProducts }}</p>
          <span class="stat-label">Produtos cadastrados</span>
        </div>
      </div>

      <div class="stat-card green">
        <div class="stat-icon">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 11l5-5m0 0l5 5m-5-5v12" />
          </svg>
        </div>
        <div class="stat-content">
          <h3>Em Estoque</h3>
          <p class="stat-number">{{ stats.inStock }}</p>
          <span class="stat-label">Itens disponíveis</span>
        </div>
      </div>

      <div class="stat-card orange">
        <div class="stat-icon">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
          </svg>
        </div>
        <div class="stat-content">
          <h3>Estoque Baixo</h3>
          <p class="stat-number">{{ stats.lowStock }}</p>
          <span class="stat-label">Requer atenção</span>
        </div>
      </div>

      <div class="stat-card purple">
        <div class="stat-icon">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16V4m0 0L3 8m4-4l4 4m6 0v12m0 0l4-4m-4 4l-4-4" />
          </svg>
        </div>
        <div class="stat-content">
          <h3>Movimentações</h3>
          <p class="stat-number">{{ stats.movements }}</p>
          <span class="stat-label">Últimos 30 dias</span>
        </div>
      </div>
    </div>

    <!-- Ações Rápidas -->
    <div class="quick-actions">
      <h2>Ações Rápidas</h2>
      <div class="actions-grid">
        <router-link to="/products" class="action-card">
          <div class="action-icon blue">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
          </div>
          <h3>Adicionar Produto</h3>
          <p>Cadastre novos produtos no sistema</p>
        </router-link>

        <router-link to="/movements" class="action-card">
          <div class="action-icon purple">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
            </svg>
          </div>
          <h3>Registrar Movimentação</h3>
          <p>Entrada ou saída de produtos</p>
        </router-link>

        <router-link to="/reports" class="action-card">
          <div class="action-icon orange">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
            </svg>
          </div>
          <h3>Ver Relatórios</h3>
          <p>Análise completa do estoque</p>
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../api/axios'

const stats = ref({
  totalProducts: 0,
  inStock: 0,
  lowStock: 0,
  movements: 0
})

const fetchStats = async () => {
  try {
    // Buscar produtos
    const productsRes = await api.get('/product')
    const products = productsRes.data
    
    stats.value.totalProducts = products.length
    stats.value.inStock = products.reduce((sum: number, p: any) => sum + p.quantityInStock, 0)
    stats.value.lowStock = products.filter((p: any) => p.quantityInStock <= p.minimumStock && p.quantityInStock > 0).length

    // Buscar movimentações
    const movementsRes = await api.get('/movement')
    const movements = movementsRes.data
    
    // Contar movimentações dos últimos 30 dias
    const thirtyDaysAgo = new Date()
    thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30)
    
    stats.value.movements = movements.filter((m: any) => 
      new Date(m.movementDate) >= thirtyDaysAgo
    ).length
  } catch (error) {
    console.error('Erro ao carregar estatísticas:', error)
  }
}

onMounted(() => {
  fetchStats()
  
  // Atualizar estatísticas a cada 30 segundos
  setInterval(fetchStats, 30000)
})
</script>

<style scoped>
.dashboard-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 2.5rem 2rem;
}

.dashboard-header {
  margin-bottom: 2.5rem;
}

.dashboard-header h1 {
  font-size: 2.5rem;
  margin-bottom: 0.5rem;
}

.subtitle {
  color: var(--gray);
  font-size: 1.125rem;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 3rem;
}

.stat-card {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  box-shadow: var(--shadow);
  display: flex;
  align-items: flex-start;
  gap: 1.5rem;
  transition: var(--transition);
  border: 1px solid var(--border);
}

.stat-card:hover {
  transform: translateY(-8px);
  box-shadow: var(--shadow-lg);
}

.stat-icon {
  width: 64px;
  height: 64px;
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.stat-icon svg {
  width: 32px;
  height: 32px;
  color: white;
}

.stat-card.blue .stat-icon {
  background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%);
  box-shadow: 0 8px 24px rgba(59, 130, 246, 0.3);
}

.stat-card.green .stat-icon {
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
  box-shadow: 0 8px 24px rgba(16, 185, 129, 0.3);
}

.stat-card.orange .stat-icon {
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
  box-shadow: 0 8px 24px rgba(245, 158, 11, 0.3);
}

.stat-card.purple .stat-icon {
  background: linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%);
  box-shadow: 0 8px 24px rgba(139, 92, 246, 0.3);
}

.stat-content h3 {
  font-size: 0.875rem;
  color: var(--gray);
  font-weight: 600;
  margin-bottom: 0.5rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.stat-number {
  font-size: 2.5rem;
  font-weight: 700;
  color: var(--dark);
  margin-bottom: 0.25rem;
  line-height: 1;
}

.stat-label {
  font-size: 0.875rem;
  color: var(--gray);
}

.quick-actions {
  margin-top: 3rem;
}

.quick-actions h2 {
  font-size: 2rem;
  margin-bottom: 1.5rem;
}

.actions-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.5rem;
}

.action-card {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  text-decoration: none;
  box-shadow: var(--shadow);
  transition: var(--transition);
  border: 1px solid var(--border);
}

.action-card:hover {
  transform: translateY(-8px);
  box-shadow: var(--shadow-lg);
}

.action-icon {
  width: 56px;
  height: 56px;
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 1.25rem;
}

.action-icon svg {
  width: 28px;
  height: 28px;
  color: white;
}

.action-icon.blue {
  background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%);
}

.action-icon.green {
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
}

.action-icon.purple {
  background: linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%);
}

.action-icon.orange {
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
}

.action-card h3 {
  font-size: 1.25rem;
  color: var(--dark);
  margin-bottom: 0.5rem;
}

.action-card p {
  color: var(--gray);
  font-size: 0.95rem;
  margin: 0;
}

@media (max-width: 768px) {
  .dashboard-container {
    padding: 1.5rem 1rem;
  }

  .dashboard-header h1 {
    font-size: 2rem;
  }

  .stats-grid,
  .actions-grid {
    grid-template-columns: 1fr;
  }

  .stat-number {
    font-size: 2rem;
  }
}
</style>
