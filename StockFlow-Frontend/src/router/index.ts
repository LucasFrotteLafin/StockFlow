import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const routes = [
  {
    path: '/',
    redirect: '/dashboard'
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('../views/LoginView.vue')
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('../views/RegisterView.vue')
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: () => import('../views/DashboardView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/products',
    name: 'Products',
    component: () => import('../views/ProductsView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/movements',
    name: 'Movements',
    component: () => import('../views/MovementView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/reports',
    name: 'Reports',
    component: () => import('../views/ReportsView.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/people',
    name: 'People',
    component: () => import('../views/PeopleView.vue'),
    meta: { requiresAuth: true, requiresAdmin: true }
  },
  {
    // rota legada — redireciona para /people
    path: '/admin',
    redirect: '/people'
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  authStore.loadUser()

  if (to.meta.requiresAuth && !authStore.isLoggedIn) {
    next('/login')
  } else if (to.meta.requiresAdmin && authStore.user?.role !== 'Admin') {
    next('/dashboard')
  } else if (to.path === '/login' && authStore.isLoggedIn) {
    next('/dashboard')
  } else if (to.path === '/register' && authStore.isLoggedIn) {
    next('/dashboard')
  } else {
    next()
  }
})

export default router
