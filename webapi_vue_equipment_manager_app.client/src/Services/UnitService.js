import { UpdateUnits } from "@/Store/Store";
import { getAccessToken } from "./UserService"

const route = "api/units"

export async function addUnit(unit){
    console.log(JSON.stringify(unit))
    try{
        const response = await fetch(route, {
            method: "POST",
            headers : {
                "Content-Type" : "application/json",
                Authorization : getAccessToken()

            },
            body: JSON.stringify(unit)
        });
        if(response.ok){
            UpdateUnits()
            return {successful: true}
        }
        else{
            console.log(response)
            return {successful: false, error: response.status, message: response.statusText}
        }
    }
    catch(ex){
        console.log(ex.message)
        return {successful: false, error: ex.message}
    }

}

export async function getRootUnits(){
    try{
        const response = await fetch(route + "/organisations", 
           { method: "Get"}
        )
        if(response.ok){
            let units = await response.json()
            console.log(units)
            return units
        }
        console.log("No root organisations found " + response.statusText)
        console.log(response)
        return []
    }
    catch(ex){
        console.log("No root organisations found " + ex.message)
        return []
    }
}