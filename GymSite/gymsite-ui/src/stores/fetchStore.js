import { defineStore } from "pinia"
import { ref } from "vue"

export const useFetchStore = defineStore('fetch', () => {
    const apiPath = 'https:localhost:5001/api/'
    const token = ref('')

    const jsonHeaders = () => ({
        'Authorization': `Bearer ${token.value}`,
        'Accept': 'application/json',
        'Content-Type': 'application/json',
    })

    const authHeaders = () => ({ 'Authorization': `Bearer ${token.value}` })

    function get(path, handler) {
        return fetch(apiPath + path, {
            headers: authHeaders()
        }).then(r => r.json())
        .then(json => {
            handler(json)
        }).catch(error => console.log(error))
    }

    function post(path, body, handler) {
        return fetch(apiPath + path, {
            headers: jsonHeaders(),
            method: "POST",
            body: JSON.stringify(body)
        }).then(r => r.json())
        .then(json => {
            handler(json)
        }).catch(error => console.log(error))
    }

    function postNoContent(path, body) {
        return fetch(apiPath + path, {
            headers: jsonHeaders(),
            method: "POST",
            body: JSON.stringify(body)
        }).catch(error => console.log(error))
    }

    function putNoContent(path, body) {
        return fetch(apiPath + path, {
            headers: jsonHeaders(),
            method: "PUT",
            body: JSON.stringify(body)
        }).catch(error => console.log(error))
    }

    function put(path, body, handler){
        return fetch(apiPath + path, {
            headers: jsonHeaders(),
            method: "PUT",
            body: JSON.stringify(body)
        }).then(res => res.json())
        .then(res => {
            handler(res)
        }).catch(error => console.log(error))
    }

    function del(path) {
        return fetch(apiPath + path, {
            headers:  jsonHeaders(),
            method: "DELETE",
        }).catch(error => console.log(error))
    }

    return { get, post, postNoContent, put, delete: del, putNoContent, token }
})