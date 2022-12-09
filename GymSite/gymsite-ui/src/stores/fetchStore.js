import { defineStore } from "pinia"

export const useFetchStore = defineStore('fetch', {
    state: () => ({ loading: false, apiPath: 'https:localhost:5001/api/', token: '' }),
    actions: {
        get(path, handler) {
            this.loading = true
            return fetch(this.apiPath + path, {
                headers: {
                    'Authorization': 'Bearer' + this.token
                },
            }).then(r => r.json())
                .then(json => {
                    handler(json)
                    this.loading = false
                })
                .catch(error => console.log(error))
        },
        post(path, body, handler) {
            this.loading = true
            return fetch(this.apiPath + path, {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + this.token
                },
                method: "POST",
                body: JSON.stringify(body)
            }).then(r => r.json())
                .then(json => {
                    handler(json)
                }).then(() => this.loading = false)
        },
        put(path, body) {
            this.loading = true
            return fetch(this.apiPath + path, {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + this.token
                },
                method: "PUT",
                body: JSON.stringify(body)
            }).catch(error => console.log(error))
            .then(() => this.loading = false)
        }
    }
})