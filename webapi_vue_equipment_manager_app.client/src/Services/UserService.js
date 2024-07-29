import { PopulateStartingData, toggleLogIn, toggleLogOut } from "@/Store/Store";
import { renderSlot } from "vue";

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
        PopulateStartingData()
        return {successfull : true}
    }
    else{
        let result = {successfull : false, errors : ""}
        if(response.status === 401 ){
            result.errors = "Invalid Username or Password"
        }
        else{
            result.errors = response.statusText
        }
        console.log(result)
        console.log("login Error : " + response.statusText)
        return result
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
        body: JSON.stringify({"refreshToken" : t.token.refreshToken}),
    })
    if(response.ok){
        let t = await response.json()
        localStorage.setItem("accessToken", JSON.stringify({ token : t, timestamp : Date.now() }))
        return `Bearer ${t.accessToken}`
    }
    else{
        console.log("Refresh Error : " + response.statusText)
        userLogout();
    }
}

export async function CheckRefreshToken(){
    try{
    let tString = localStorage.getItem("accessToken")
    let t = JSON.parse(tString)
    const response = await fetch("api/refresh", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({"refreshToken" : t.token.refreshToken}),
    })
    if(response.ok){
        let t = await response.json()
        localStorage.setItem("accessToken", JSON.stringify({ token : t, timestamp : Date.now() }))
        return `Bearer ${t.accessToken}`
    }
    else{
        console.warn("Refresh Error : " + response.statusText)
        userLogout();
    }
}catch(e){
    console.warn("Could not refresh : ")
    userLogout()
}
}

export async function userLogout(){
    toggleLogOut()
    localStorage.removeItem("accessToken")
}

export async function GetUsers(){
    try{
    let response = await fetch('api/Users',
        {
            method: "GET",
            headers: {
               "Authorization":  await getAccessToken()
            }
        }
    )
    if(response.ok){
        let users = await response.json()
        return users;
    }
    else{
        console.log("Coulld not get public users. Response : " + response.statusText)
    }
    }
    catch(ex){
        console.log("Could not get public users. Exception : " + ex.message) 
    }
}

export async function AssignUser(usId, roId, unId) {
    let assignment = JSON.stringify({userId: usId, roleId: roId, unitiD: unId})
    try{
    let response = await fetch('api/Users/assignments', {
        method: "Post",
        headers: {
            "Authorization" : await getAccessToken(),
            "Content-Type": "application/json"

            
        },
        body: assignment
    })
    if(response.ok){
        let result = await response.json()
        return {successfull: true , assignment: result}
    }
    let result = await response.json()
    console.log(result.detail)
    return {successfull: false, errors: response.statusText , message : result?.detail}
    }
    catch(ex){
        console.log(ex.message)
        return {successfull: false, errors: ex.message}
    }
}

export async function DeleteAssignment(assignment){
    let deleted = JSON.stringify(assignment)
    try{
        let response = await fetch('api/users/assignments',{
            method : "DELETE",
            headers:{
                Authorization : await getAccessToken(),
                "Content-Type": "application/json"

            },
            body: deleted
        })
        if(response.ok){
            return {successfull:true}
        }
        else{
            let result = await response.json()
            console.log(result.detail)
            return {successfull : false, errors: response.statusText, message : result?.detail}
        }
    }
    catch(ex){
        return {successfull : false, errors: ex.message}
    }
}

export async function EditAssignment(assignment){
    let edited = JSON.stringify(assignment)
    try{
        let response = await fetch('api/users/assignments',{
            method : "PUT",
            headers:{
                Authorization : await getAccessToken(),
                "Content-Type": "application/json"
            },
            body: edited
        })
        if(response.ok){
            return {successfull:true, updated: await response.json()}
        }
        else{
            let result = await response.json()
            return {successfull : false, errors: response.statusText, message : result?.detail}
        }
    }
    catch(ex){
        return {successfull : false, errors: ex.message}
    }
}
