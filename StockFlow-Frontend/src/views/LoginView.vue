<template>
  <div class="auth-container">
    <div class="auth-wrapper fade-in">
      <div class="auth-card">
        <!-- Logo e Header -->
        <div class="auth-header">
          <div class="logo-circle">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
            </svg>
          </div>
          <h1>StockFlow</h1>
          <p class="subtitle">Sistema de Gerenciamento de Estoque</p>
        </div>

        <!-- Formulário -->
        <form @submit.prevent="handleLogin" class="auth-form">
          <div class="form-group">
            <label for="username">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
              </svg>
              Usuário
            </label>
            <input 
              id="username"
              v-model="username" 
              type="text" 
              placeholder="Digite seu usuário" 
              required
              autocomplete="username"
            >
          </div>

          <div class="form-group">
            <label for="password">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
              </svg>
              Senha
            </label>
            <input 
              id="password"
              v-model="password" 
              type="password" 
              placeholder="Digite sua senha" 
              required
              autocomplete="current-password"
            >
          </div>

          <button type="submit" class="btn btn-primary w-full" :disabled="loading">
            <span v-if="!loading">Entrar</span>
            <span v-else class="flex-center gap-2">
              <div class="mini-spinner"></div>
              Entrando...
            </span>
          </button>
        </form>

        <!-- Divider -->
        <div class="divider">
          <span>ou</span>
        </div>

        <!-- Botão Registrar -->
        <router-link to="/register" class="btn btn-outline w-full">
          Criar Nova Conta
        </router-link>

        <!-- Mensagem de Erro -->
        <div v-if="error" class="alert alert-danger mt-3">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
          {{ error }}
        </div>
      </div>

      <!-- Footer Info -->
      <div class="auth-footer">
        <p>Versão 1.0.0 • Desenvolvido com ❤️</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const username = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)

const handleLogin = async () => {
  try {
    error.value = ''
    loading.value = true
    await authStore.login(username.value, password.value)
    router.push('/dashboard')
  } catch (err: any) {
    error.value = err.response?.data?.message || err.response?.data || 'Erro ao fazer login. Verifique suas credenciais.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.auth-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 50%, #f093fb 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2rem;
  position: relative;
  overflow: hidden;
}

.auth-container::before {
  content: '';
  position: absolute;
  width: 500px;
  height: 500px;
  background: radial-gradient(circle, rgba(255,255,255,0.1) 0%, transparent 70%);
  border-radius: 50%;
  top: -250px;
  right: -250px;
  animation: float 6s ease-in-out infinite;
}

.auth-container::after {
  content: '';
  position: absolute;
  width: 300px;
  height: 300px;
  background: radial-gradient(circle, rgba(255,255,255,0.1) 0%, transparent 70%);
  border-radius: 50%;
  bottom: -150px;
  left: -150px;
  animation: float 8s ease-in-out infinite reverse;
}

@keyframes float {
  0%, 100% { transform: translateY(0px); }
  50% { transform: translateY(20px); }
}

.auth-wrapper {
  max-width: 460px;
  width: 100%;
  position: relative;
  z-index: 1;
}

.auth-card {
  background: white;
  padding: 3rem 2.5rem;
  border-radius: 24px;
  box-shadow: 0 30px 60px rgba(0, 0, 0, 0.3);
}

.auth-header {
  text-align: center;
  margin-bottom: 2.5rem;
}

.logo-circle {
  width: 80px;
  height: 80px;
  background: linear-gradient(135deg, var(--primary) 0%, var(--primary-dark) 100%);
  border-radius: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 1.5rem;
  box-shadow: 0 10px 30px rgba(59, 130, 246, 0.3);
  transform: rotate(-5deg);
  transition: transform 0.3s ease;
}

.logo-circle:hover {
  transform: rotate(0deg) scale(1.05);
}

.logo-circle svg {
  width: 40px;
  height: 40px;
  color: white;
}

.auth-header h1 {
  font-size: 2.5rem;
  background: linear-gradient(135deg, var(--primary) 0%, var(--primary-dark) 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 0.5rem;
}

.subtitle {
  color: var(--gray);
  font-size: 1rem;
  margin: 0;
}

.auth-form {
  margin-bottom: 1.5rem;
}

.form-group label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.5rem;
  font-weight: 600;
  color: var(--dark);
}

.form-group label svg {
  width: 18px;
  height: 18px;
  color: var(--primary);
}

.divider {
  position: relative;
  text-align: center;
  margin: 1.5rem 0;
}

.divider::before {
  content: '';
  position: absolute;
  left: 0;
  top: 50%;
  width: 100%;
  height: 1px;
  background: var(--border);
}

.divider span {
  position: relative;
  background: white;
  padding: 0 1rem;
  color: var(--gray);
  font-size: 0.875rem;
  font-weight: 500;
}

.auth-footer {
  text-align: center;
  margin-top: 2rem;
  color: white;
  font-size: 0.875rem;
  opacity: 0.9;
}

.mini-spinner {
  width: 16px;
  height: 16px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@media (max-width: 768px) {
  .auth-container {
    padding: 1rem;
  }
  
  .auth-card {
    padding: 2rem 1.5rem;
  }
  
  .auth-header h1 {
    font-size: 2rem;
  }
  
  .logo-circle {
    width: 60px;
    height: 60px;
  }
  
  .logo-circle svg {
    width: 30px;
    height: 30px;
  }
}
</style>
