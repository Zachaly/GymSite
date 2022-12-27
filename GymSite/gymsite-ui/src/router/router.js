import { createRouter, createWebHistory } from 'vue-router'

const path = (route, name, component) => ({ path: route, name, component: () => import(`@/views/${component}.vue`)})

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
        name: 'update-profile',
        component: () => import('@/views/UpdateProfileView.vue')
    },
    {
        path: '/admin',
        name: 'admin',
        component: () => import('@/views/AdminView.vue')
    },
    {
        path: '/exercise',
        name: 'exercise-list',
        component: () => import("@/views/ExerciseListView.vue")
    },
    {
        path: '/exercise/:id',
        name: 'exercise',
        component: () => import('@/views/ExerciseView.vue')
    },
    {
        path: '/exercise/add',
        name: 'add-exercise',
        component: () => import('@/views/AddExerciseView.vue')
    },
    path('/workout', 'workout-list', 'WorkoutListView'),
    path('/workout/add', 'add-workout', 'AddWorkoutView'),
    path('/workout/:id', 'workout', 'WorkoutView'),
]

export default createRouter({
    routes,
    history: createWebHistory(),
})
