import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    {
        path: '/',
        name: 'home',
        component: () => import('../views/HomeView.vue')
    },
    {
        path: '/register',
        name: 'register',
        component: () => import('@/views/RegisterView.vue')
    },
    {
        path: '/login',
        name: 'login',
        component: () => import('@/views/LoginView.vue')
    },
    {
        path: '/user/:id',
        name: 'user',
        component: () => import('@/views/ProfileView.vue'),
    },
    {
        path: '/user/update',
        name: 'updateprofile',
        component: () => import('@/views/UpdateProfileView.vue')
    },
    {
        path: '/admin',
        name: 'admin',
        component: () => import('@/views/AdminView.vue')
    }
]

export default createRouter({
    routes,
    history: createWebHistory(),
})
