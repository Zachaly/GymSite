import { defineStore } from "pinia";
import { ref } from "vue";
import { useFetchStore } from "./fetchStore";

export const useAuthStore = defineStore('auth', () => {
    const authorized = ref(false)
    const userId = ref('')
    const claims = ref([])
    const fetchStore = useFetchStore()

    function login(credentials){
        return fetchStore.post('auth/login', credentials, res => {
            userId.value = res.data.userId
            fetchStore.token = res.data.authToken
            authorized.value = true
            claims.value = res.data.claims
        })
    }

    function logout(){
        authorized.value = false
        userId.value = ''
        fetchStore.token = ''
        claims.value = []
    }

    return { authorized, userId, claims, login, logout }
})