<template>
  <div class="people-container">
    <!-- Cabeçalho -->
    <div class="people-header">
      <div>
        <h1>Gerenciamento de Pessoas</h1>
        <p>Gerencie solicitações de acesso e usuários ativos do sistema</p>
      </div>
      <div class="header-stats">
        <div class="stat-badge pending">
          <span class="stat-number">{{ peopleStore.pendingRequests.length }}</span>
          <span class="stat-label">Pendentes</span>
        </div>
        <div class="stat-badge approved">
          <span class="stat-number">{{ peopleStore.approvedUsers.length }}</span>
          <span class="stat-label">Aprovados</span>
        </div>
      </div>
    </div>

    <!-- Toast de feedback -->
    <transition name="toast">
      <div v-if="toast.show" :class="['toast', `toast-${toast.type}`]">
        <svg v-if="toast.type === 'success'" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
        </svg>
        <svg v-else xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
        </svg>
        {{ toast.message }}
      </div>
    </transition>

    <!-- Solicitações Pendentes -->
    <div class="section">
      <div class="section-header">
        <h2 class="section-title">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
          Solicitações Pendentes
          <span v-if="peopleStore.pendingRequests.length > 0" class="count-badge count-warning">
            {{ peopleStore.pendingRequests.length }}
          </span>
        </h2>
      </div>

      <div v-if="loadingInitial" class="loading-state">
        <div class="spinner"></div>
        <p>Carregando solicitações...</p>
      </div>

      <div v-else-if="peopleStore.pendingRequests.length === 0" class="empty-state">
        <div class="empty-icon">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
        </div>
        <p>Nenhuma solicitação pendente</p>
        <span>Tudo em dia!</span>
      </div>

      <div v-else class="requests-list">
        <div
          v-for="request in peopleStore.pendingRequests"
          :key="request.id"
          class="request-card"
          :class="{ 'is-processing': processingId === request.id }"
        >
          <div class="card-left">
            <div class="user-avatar avatar-pending">
              {{ request.username.substring(0, 2).toUpperCase() }}
            </div>
            <div class="user-details">
              <h3>{{ request.username }}</h3>
              <p class="meta">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                </svg>
                Solicitado em {{ formatDate(request.requestDate) }}
              </p>
            </div>
          </div>
          <div class="card-right">
            <span class="status-tag status-pending">Aguardando</span>
            <div class="action-buttons">
              <button
                @click="handleApprove(request.id, request.username)"
                class="btn btn-approve"
                :disabled="peopleStore.loading"
                title="Aprovar acesso"
              >
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                </svg>
                Aprovar
              </button>
              <button
                @click="handleDeny(request.id, request.username)"
                class="btn btn-deny"
                :disabled="peopleStore.loading"
                title="Rejeitar solicitação"
              >
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                </svg>
                Rejeitar
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Usuários com Acesso -->
    <div class="section">
      <div class="section-header">
        <h2 class="section-title">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z" />
          </svg>
          Usuários com Acesso
          <span v-if="peopleStore.approvedUsers.length > 0" class="count-badge count-success">
            {{ peopleStore.approvedUsers.length }}
          </span>
        </h2>
      </div>

      <div v-if="peopleStore.approvedUsers.length === 0" class="empty-state">
        <div class="empty-icon">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
          </svg>
        </div>
        <p>Nenhum usuário aprovado ainda</p>
        <span>Aprove as solicitações acima para liberar o acesso.</span>
      </div>

      <div v-else class="users-grid">
        <div v-for="user in peopleStore.approvedUsers" :key="user.id" class="user-card">
          <div class="user-avatar avatar-approved">
            {{ user.username.substring(0, 2).toUpperCase() }}
          </div>
          <div class="user-info">
            <h3>{{ user.username }}</h3>
            <span class="status-tag status-approved">{{ user.role }}</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Erro global -->
    <div v-if="peopleStore.error" class="alert alert-danger">
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
      {{ peopleStore.error }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { usePeopleStore } from '../stores/people'

const peopleStore = usePeopleStore()

const loadingInitial = ref(true)
const processingId = ref<number | null>(null)

const toast = ref<{ show: boolean; message: string; type: 'success' | 'error' }>({
  show: false,
  message: '',
  type: 'success'
})

const showToast = (message: string, type: 'success' | 'error' = 'success') => {
  toast.value = { show: true, message, type }
  setTimeout(() => { toast.value.show = false }, 3500)
}

onMounted(async () => {
  try {
    await Promise.all([
      peopleStore.fetchPendingRequests(),
      peopleStore.fetchApprovedUsers()
    ])
  } finally {
    loadingInitial.value = false
  }
})

const formatDate = (dateString: string) => {
  return new Intl.DateTimeFormat('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  }).format(new Date(dateString))
}

const handleApprove = async (requestId: number, username: string) => {
  if (!confirm(`Aprovar o acesso de "${username}"?`)) return
  processingId.value = requestId
  try {
    await peopleStore.approveUser(requestId)
    showToast(`Acesso de "${username}" aprovado com sucesso!`, 'success')
  } catch {
    showToast('Erro ao aprovar. Tente novamente.', 'error')
  } finally {
    processingId.value = null
  }
}

const handleDeny = async (requestId: number, username: string) => {
  if (!confirm(`Rejeitar a solicitação de "${username}"? Esta ação não pode ser desfeita.`)) return
  processingId.value = requestId
  try {
    await peopleStore.denyUser(requestId)
    showToast(`Solicitação de "${username}" rejeitada.`, 'success')
  } catch {
    showToast('Erro ao rejeitar. Tente novamente.', 'error')
  } finally {
    processingId.value = null
  }
}
</script>

<style scoped>
.people-container {
  max-width: 1100px;
  margin: 0 auto;
  padding: 2rem;
}

/* Cabeçalho */
.people-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1.5rem;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}

.people-header h1 {
  font-size: 1.875rem;
  color: var(--dark);
  margin: 0 0 0.35rem 0;
}

.people-header p {
  color: var(--gray);
  font-size: 0.95rem;
  margin: 0;
}

.header-stats {
  display: flex;
  gap: 1rem;
}

.stat-badge {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 0.75rem 1.25rem;
  border-radius: 12px;
  min-width: 80px;
}

.stat-badge.pending {
  background: #fff7ed;
  border: 1px solid #fed7aa;
}

.stat-badge.approved {
  background: #f0fdf4;
  border: 1px solid #bbf7d0;
}

.stat-number {
  font-size: 1.75rem;
  font-weight: 700;
  line-height: 1;
}

.stat-badge.pending .stat-number { color: #ea580c; }
.stat-badge.approved .stat-number { color: #16a34a; }

.stat-label {
  font-size: 0.75rem;
  font-weight: 600;
  margin-top: 0.25rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.stat-badge.pending .stat-label { color: #9a3412; }
.stat-badge.approved .stat-label { color: #15803d; }

/* Toast */
.toast {
  position: fixed;
  top: 1.5rem;
  right: 1.5rem;
  z-index: 9999;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1rem 1.5rem;
  border-radius: 12px;
  font-weight: 600;
  font-size: 0.95rem;
  box-shadow: 0 8px 24px rgba(0,0,0,0.15);
  max-width: 380px;
}

.toast svg {
  width: 22px;
  height: 22px;
  flex-shrink: 0;
}

.toast-success {
  background: #f0fdf4;
  border: 1px solid #86efac;
  color: #15803d;
}

.toast-error {
  background: #fef2f2;
  border: 1px solid #fca5a5;
  color: #dc2626;
}

.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s ease;
}

.toast-enter-from,
.toast-leave-to {
  opacity: 0;
  transform: translateX(100%);
}

/* Seções */
.section {
  background: white;
  border-radius: 16px;
  padding: 1.75rem 2rem;
  margin-bottom: 1.5rem;
  box-shadow: 0 2px 8px rgba(0,0,0,0.07);
  border: 1px solid var(--border, #e5e7eb);
}

.section-header {
  margin-bottom: 1.5rem;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 0.625rem;
  font-size: 1.125rem;
  color: var(--dark);
  margin: 0;
  font-weight: 700;
}

.section-title svg {
  width: 22px;
  height: 22px;
  color: var(--primary, #3b82f6);
  flex-shrink: 0;
}

.count-badge {
  margin-left: 0.25rem;
  padding: 0.15rem 0.6rem;
  border-radius: 999px;
  font-size: 0.78rem;
  font-weight: 700;
}

.count-warning {
  background: #fff7ed;
  color: #ea580c;
  border: 1px solid #fed7aa;
}

.count-success {
  background: #f0fdf4;
  color: #16a34a;
  border: 1px solid #bbf7d0;
}

/* Estados */
.loading-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 3rem 2rem;
  color: var(--gray);
  gap: 1rem;
}

.spinner {
  width: 36px;
  height: 36px;
  border: 3px solid var(--light-gray, #f3f4f6);
  border-top-color: var(--primary, #3b82f6);
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}

@keyframes spin { to { transform: rotate(360deg); } }

.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 2.5rem 2rem;
  color: var(--gray);
  gap: 0.5rem;
  text-align: center;
}

.empty-state p {
  font-weight: 600;
  color: var(--dark);
  margin: 0;
}

.empty-state span {
  font-size: 0.875rem;
}

.empty-icon {
  width: 52px;
  height: 52px;
  border-radius: 50%;
  background: var(--light-gray, #f3f4f6);
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 0.5rem;
}

.empty-icon svg {
  width: 26px;
  height: 26px;
  color: var(--gray);
}

/* Lista de solicitações */
.requests-list {
  display: flex;
  flex-direction: column;
  gap: 0.875rem;
}

.request-card {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
  padding: 1.25rem 1.5rem;
  border: 1.5px solid var(--border, #e5e7eb);
  border-radius: 12px;
  transition: all 0.25s ease;
  background: #fafafa;
}

.request-card:hover {
  border-color: var(--primary, #3b82f6);
  background: white;
  box-shadow: 0 2px 10px rgba(59,130,246,0.08);
}

.request-card.is-processing {
  opacity: 0.6;
  pointer-events: none;
}

.card-left {
  display: flex;
  align-items: center;
  gap: 1rem;
  flex: 1;
  min-width: 0;
}

.card-right {
  display: flex;
  align-items: center;
  gap: 0.875rem;
  flex-shrink: 0;
}

.user-avatar {
  width: 46px;
  height: 46px;
  border-radius: 50%;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 0.875rem;
  flex-shrink: 0;
}

.avatar-pending {
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
}

.avatar-approved {
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
}

.user-details h3 {
  font-size: 0.975rem;
  color: var(--dark);
  margin: 0 0 0.2rem 0;
  font-weight: 600;
}

.meta {
  display: flex;
  align-items: center;
  gap: 0.35rem;
  color: var(--gray);
  font-size: 0.8rem;
  margin: 0;
}

.meta svg {
  width: 13px;
  height: 13px;
  flex-shrink: 0;
}

/* Status tags */
.status-tag {
  padding: 0.25rem 0.75rem;
  border-radius: 999px;
  font-size: 0.78rem;
  font-weight: 600;
  white-space: nowrap;
}

.status-pending {
  background: #fff7ed;
  color: #ea580c;
  border: 1px solid #fed7aa;
}

.status-approved {
  background: #f0fdf4;
  color: #15803d;
  border: 1px solid #bbf7d0;
}

/* Botões de ação */
.action-buttons {
  display: flex;
  gap: 0.5rem;
}

.btn {
  display: flex;
  align-items: center;
  gap: 0.375rem;
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  font-size: 0.85rem;
  cursor: pointer;
  transition: all 0.2s ease;
  white-space: nowrap;
}

.btn svg {
  width: 16px;
  height: 16px;
}

.btn:disabled {
  opacity: 0.45;
  cursor: not-allowed;
  transform: none !important;
}

.btn-approve {
  background: #dcfce7;
  color: #15803d;
  border: 1px solid #86efac;
}

.btn-approve:hover:not(:disabled) {
  background: #16a34a;
  color: white;
  border-color: #16a34a;
  transform: translateY(-1px);
  box-shadow: 0 3px 8px rgba(22,163,74,0.25);
}

.btn-deny {
  background: #fee2e2;
  color: #dc2626;
  border: 1px solid #fca5a5;
}

.btn-deny:hover:not(:disabled) {
  background: #dc2626;
  color: white;
  border-color: #dc2626;
  transform: translateY(-1px);
  box-shadow: 0 3px 8px rgba(220,38,38,0.25);
}

/* Grid de usuários aprovados */
.users-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 1rem;
}

.user-card {
  display: flex;
  align-items: center;
  gap: 0.875rem;
  padding: 1.125rem 1.25rem;
  border: 1.5px solid var(--border, #e5e7eb);
  border-radius: 12px;
  background: #fafafa;
  transition: all 0.25s ease;
}

.user-card:hover {
  border-color: #10b981;
  background: white;
  box-shadow: 0 2px 10px rgba(16,185,129,0.08);
}

.user-info h3 {
  font-size: 0.925rem;
  color: var(--dark);
  margin: 0 0 0.3rem 0;
  font-weight: 600;
}

/* Alerta de erro */
.alert {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1rem 1.25rem;
  border-radius: 10px;
  font-weight: 500;
}

.alert svg {
  width: 20px;
  height: 20px;
  flex-shrink: 0;
}

.alert-danger {
  background: #fef2f2;
  border: 1px solid #fca5a5;
  color: #dc2626;
}

/* Responsivo */
@media (max-width: 768px) {
  .people-container {
    padding: 1rem;
  }

  .people-header {
    flex-direction: column;
  }

  .section {
    padding: 1.25rem;
  }

  .request-card {
    flex-direction: column;
    align-items: flex-start;
  }

  .card-right {
    width: 100%;
    flex-direction: column;
    align-items: flex-start;
    gap: 0.75rem;
  }

  .action-buttons {
    width: 100%;
  }

  .btn {
    flex: 1;
    justify-content: center;
  }

  .users-grid {
    grid-template-columns: 1fr;
  }
}
</style>
