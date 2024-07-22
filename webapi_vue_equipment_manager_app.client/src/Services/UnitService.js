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
                Authorization : await getAccessToken()

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
            assignedUsers : unit.assigedUsers
        }        
        }
    node.children = unit.children.map(child => {return toTreeNode(child)})
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
        let tree = toTreeNode(structure[0])
        return tree;
    }
    else{
        console.log("Failed to retrieve org structure. Response: " + response.statusText);
        console.log(await response.json())
    }}
    catch(ex){
        console.log(ex.message)
    }
}