export default class ApiReponse{
    constructor(successfull, payload,error,message){
        this.successfull = successfull,
        this.payload = payload,
        this.error = error,
        this.message = message
    }
}