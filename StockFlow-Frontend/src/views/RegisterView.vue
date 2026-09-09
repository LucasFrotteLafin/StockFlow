<template>
  <div class="auth-container">
    <div class="auth-wrapper fade-in">
      <div class="auth-card">
        <!-- Logo e Header -->
        <div class="auth-header">
          <div class="logo-circle">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
            </svg>
          </div>
          <h1>StockFlow</h1>
          <p class="subtitle">Criar Nova Conta</p>
        </div>

        <!-- Formulário -->
        <form @submit.prevent="handleRegister" class="auth-form">
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
              placeholder="Escolha um usuário" 
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
              placeholder="Crie uma senha" 
              required
              autocomplete="new-password"
            >
          </div>

          <div class="form-group">
            <label for="confirmPassword">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
              </svg>
              Confirme a Senha
            </label>
            <input 
              id="confirmPassword"
              v-model="confirmPassword" 
              type="password" 
              placeholder="Confirme a senha" 
              required
              autocomplete="new-password"
            >
          </div>

          <button type="submit" class="btn btn-primary w-full" :disabled="loading">
            <span v-if="!loading">Criar Conta</span>
            <span v-else class="flex-center gap-2">
              <div class="mini-spinner"></div>
              Criando...
            </span>
          </button>
        </form>

        <!-- Mensagem de Sucesso -->
        <div v-if="success" class="alert alert-success mt-3">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
          {{ success }}
        </div>

        <!-- Mensagem de Erro -->
        <div v-if="error" class="alert alert-danger mt-3">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
          {{ error }}
        </div>

        <!-- Link para Login -->
        <div class="auth-link mt-4">
          <p>Já tem conta? <router-link to="/login" class="link">Fazer login</router-link></p>
        </div>
      </div>

      <!-- Footer Info -->
      <div class="auth-footer">
        <p>Versão 1.0.0</p>
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
const confirmPassword = ref('')
const error = ref('')
const success = ref('')
const loading = ref(false)

const handleRegister = async () => {
  try {
    error.value = ''
    success.value = ''

    if (password.value !== confirmPassword.value) {
      error.value = 'As senhas não coincidem'
      return
    }

    if (password.value.length < 6) {
      error.value = 'A senha deve ter no mínimo 6 caracteres'
      return
    }

    loading.value = true
    await authStore.register(username.value, password.value)
    // Senha enviada em texto puro — o backend criptografa automaticamente antes de salvar
    success.value = `Solicitação enviada com sucesso! Sua conta "${username.value}" aguarda aprovação do administrador. Enquanto não for aprovada, o login não funcionará. Redirecionando para o login...`

    // Limpa os campos para evitar reenvio acidental
    username.value = ''
    password.value = ''
    confirmPassword.value = ''

    setTimeout(() => {
      router.push('/login')
    }, 4000)
  } catch (err: any) {
    error.value = err.response?.data?.message || err.response?.data || 'Erro ao enviar solicitação. Tente novamente.'
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

.auth-footer {
  text-align: center;
  margin-top: 2rem;
  color: white;
  font-size: 0.875rem;
  opacity: 0.9;
}

.auth-link {
  text-align: center;
  color: var(--gray);
  font-size: 0.875rem;
}

.auth-link .link {
  color: var(--primary);
  text-decoration: none;
  font-weight: 600;
  transition: color 0.3s ease;
}

.auth-link .link:hover {
  color: var(--primary-dark);
  text-decoration: underline;
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
