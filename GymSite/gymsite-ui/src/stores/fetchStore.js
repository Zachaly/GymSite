import { defineStore } from "pinia"

export const useFetchStore = defineStore('fetch', {
    state: () => ({ loading: false, apiPath: 'https:localhost:5001/api/' }),
    actions: {
        get(path, handler) {
            this.loading = true
            fetch(this.apiPath + path)
                .then(r => r.json())
                .then(json => {
                    handler(json)
                    this.loading = false
                }).catch(error => console.log(error))
        },
        post(path, body, handler){
            this.loading = true
            fetch(this.apiPath + path, {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                  },
                method: "POST",
                body: JSON.stringify(body)
            }).then(r => r.json())
            .then(json => {
                handler(json)
                this.loading = false
            })
        }
    }
})