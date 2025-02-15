import { UpdateUnits } from "@/Store/Store";
import { getAccessToken } from "./UserService"

const route = "api/units"

export async function addUnit(unit){
    try{
        const response = await fetch(route, {
            method: "POST",
            headers : {
                "Content-Type" : "application/json",
                Authorization : await getAccessToken()

            },
            body: JSON.stringify(unit)
        });
        if(response.ok){
            UpdateUnits()
            return {successful: true}
        }
        else{
            console.warn(response)
            return {successful: false, error: response.status, message: response.statusText}
        }
    }
    catch(ex){
        console.warn(ex.message)
        return {successful: false, error: ex.message}
    }

}

export async function getPublicUnits(){
    try{
        const response = await fetch(route + "/public", 
           { method: "Get"}
        )
        if(response.ok){
            let units = await response.json()
            return units
        }
        console.warn("No public organisations found " + response.statusText)
        console.warn(response)
        return []
    }
    catch(ex){
        console.warn("No public organisations found " + ex.message)
        return []
    }
}

export function toTreeNode(unit){
    let node = {
        key: unit.id,
        label: unit.name,
        data: {
            id : unit.id,
            name : unit.name,
            room : unit.room,
            type: 'unit',
            building : unit.building,
            address : unit.address,
            parentId :unit.parentId,
            assignedUsers : unit.assigedUsers,
            isPublic : unit.isPublic
        }        
        }
    if(unit.children){
        node.children = unit.children.map(child => {return toTreeNode(child)})
    }
    return node;
}

export async function GetOrgStructure(){
    try{
    let response = await fetch(route + "?adminOnly=true&flat=false", {
        method: "GET",
        headers: {
            "Authorization" : await getAccessToken()
        }
    });
    if(response.ok){
        let structure = await response.json()
        let tree = structure.map(unit => toTreeNode(unit))
        return tree;
    }
    else{
        console.warn("Failed to retrieve org structure. Response: " + response.statusText);
        console.warn(await response.json())
    }}
    catch(ex){
        console.warn(ex.message)
    }
}

export async function EditUnit(unit){
    let edited = JSON.stringify(unit)
    try{
        let response = await fetch(route, {
            method: "PUT",
            headers : {
                "Authorization" : await getAccessToken(),
                "Content-Type": "application/json"

            },
            body: edited
        })
        if(response.ok){
            edited = await response.json()
            return {successful : true, unit: edited}
        }
        else{
            console.warn(response)
            return {successful : false, errors: response.json()}
        }
    }
    catch(ex){
        console.warn(ex)
        return {successful : false, errors : ex.message}
    }
}

export async function DeleteUnit(unit){
    try{
    let response = await fetch(route + `/${unit.id}`,{
        method:"DELETE",
        headers : {
            "Authorization" : await getAccessToken(),
            "Content-Type": "application/json"

        },
    })
    if(response.ok){
        return {successful: true}
    }
    else{
        return {successful: false, error : response.statusText, message: await response.json()}
    }
    }
    catch (ex){
        return {successful: false, error: ex.message}
    }
}