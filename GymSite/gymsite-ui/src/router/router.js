import { createRouter, createWebHistory } from 'vue-router'

const path = (route, name, view) => ({ path: '/' + route, name, component: () => import(`@/views/${view}View.vue`)})

const routes = [
    path('', 'home', 'Home'),
    path('register', 'register', 'Register'),
    path('login', 'login', 'Login'),
    path('user/:id', 'user', 'Profile'),
    path('user/update', 'update-user', 'UpdateProfile'),
    path('admin', 'admin', 'Admin'),
    path('exercise', 'exercise-list', 'ExerciseList'),
    path('exercise/:id', 'exercise', 'Exercise'),
    path('exercise/add', 'add-exercise', 'AddExercise'),
    path('workout', 'workout-list', 'WorkoutList'),
    path('workout/add', 'add-workout', 'AddWorkout'),
    path('workout/:id', 'workout', 'Workout'),
]

export default createRouter({
    routes,
    history: createWebHistory(),
})
