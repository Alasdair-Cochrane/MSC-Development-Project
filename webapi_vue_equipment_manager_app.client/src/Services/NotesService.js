import ApiReponse from "@/Models/ApiResponse";
import { getAccessToken } from "./UserService";
import { GetFromAPI } from "@/Store/Store";

export async function AddNote(note){
    try{
    let data = JSON.stringify(note)
    var response = await fetch('api/items/notes',
        {
            method: "POST",
            headers:{
                "Authorization": await getAccessToken(),
                "Content-Type" : "application/json",

            },
            body: data
        })
        if(response.ok){
            let result = await response.json()
            return new ApiReponse(true,result)
        }
        else{
            return new ApiReponse(false,null,response.statusText)
        }
    }
    catch(e){
        return new ApiReponse(false, null, e.message)
    }
}

export async function GetNotesForItem(itemId){
    let result = await GetFromAPI(`api/items/${itemId}/notes`,`Could not retrieve notes for item ${itemId}`)
    return result
}
export async function DeleteNote(noteId){
    try{
    let result = await fetch(`api/items/notes/${noteId}`,{
        method: "DELETE",
        headers:{
            "AUTHORIZATION": await getAccessToken()
        }
    })
    if(result.ok){
        return new ApiReponse(true)
    }
    else{
        return new ApiReponse(false,null,result.statusText)
    }
    }
    catch(e){
        return new ApiReponse(false,null,e.message)
    }
}

