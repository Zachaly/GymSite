export default {
    // eslint-disable-next-line no-unused-vars
    install: (app, options) => {
        app.config.globalProperties.$gender = (num) => {
            let res = 'Not specified'

            if(num === 1){
                res = 'Male'
            } else if (num === 2){
                res = 'Female'
            }

            return res
        }
    }
}