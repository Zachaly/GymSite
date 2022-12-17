import { defineStore } from "pinia"

export const useFetchStore = defineStore('fetch', {
    state: () => ({ apiPath: 'https:localhost:5001/api/', token: '' }),
    actions: {
        get(path, handler) {
            return fetch(this.apiPath + path, {
                headers: {
                    'Authorization': `Bearer ${this.token}`
                },
            }).then(r => r.json())
                .then(json => {
                    handler(json)
                }).catch(error => console.log(error))
        },
        post(path, body, handler) {
            return fetch(this.apiPath + path, {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${this.token}`
                },
                method: "POST",
                body: JSON.stringify(body)
            }).then(r => r.json())
                .then(json => {
                    handler(json)
                })
        },
        postNoContent(path, body) {
            return fetch(this.apiPath + path, {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + this.token
                },
                method: "POST",
                body: JSON.stringify(body)
            })
        },
        put(path, body) {
            return fetch(this.apiPath + path, {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + this.token
                },
                method: "PUT",
                body: JSON.stringify(body)
            }).catch(error => console.log(error))
        },
        delete(path){
            return fetch(this.apiPath + path, {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + this.token
                },
                method: "DELETE",
            }).catch(error => console.log(error))
        }
    }
})