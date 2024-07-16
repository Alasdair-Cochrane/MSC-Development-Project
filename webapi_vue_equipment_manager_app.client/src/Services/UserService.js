import { toggleLogIn, toggleLogOut } from "@/Store/Store";

export async function userLogin(login){
    try{
    var response = await fetch('api/login',
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(login),
        })
    if(response.ok){
        toggleLogIn()
        let t = await response.json()
        localStorage.setItem("accessToken", JSON.stringify({ token : t, timestamp : Date.now() }))
        return {successfull : true}
    }
    else{
        let result = await response.json()
        if(result.detail === "Failed"){
            result.detail = "Invalid Username or Password"
        }
        console.log(result)
        console.log("login Error : " + result.detail)
        return {successfull : false, errors: result.detail,}

    }
    }
    catch(ex){
        return {successfull: false, errors: ex.message}
    }
}

export async function getAccessToken(){
    let tString = localStorage.getItem("accessToken")
    let t = JSON.parse(tString)
    if(Date.now() < t.timestamp + (t.token.expiresIn * 1000)){
        return `Bearer ${t.token.accessToken}`;
    }
    
    const response = await fetch("api/refresh", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({refreshToken : t.token.refreshToken}),
    })
    if(response.ok){
        let t = await response.json()
        localStorage.setItem("accessToken", JSON.stringify({ token : t, timestamp : Date.now() }))
        return `Bearer ${t.accessToken}`
    }
    else{
        console.log("Refresh Error : " + response.statusText)
    }
}

export async function userLogout(){
    toggleLogOut()
    localStorage.removeItem("accessToken")
}