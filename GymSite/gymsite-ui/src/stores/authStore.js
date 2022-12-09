import { defineStore } from "pinia";
import { useFetchStore } from "./fetchStore";

export const useAuthStore = defineStore('auth', {
    state: () => ({ authorized: false, userId: '', fetchStore: useFetchStore() }),
    actions: {
        login(credentials) {
            return this.fetchStore.post('auth/login', credentials, res => {
                this.userId = res.data.userId
                this.fetchStore.token = res.data.authToken
                this.authorized = true
            })
        },
        logout() {
            this.authorized = false
            this.userId = ''
            this.fetchStore.token = ''
        }
    }
})