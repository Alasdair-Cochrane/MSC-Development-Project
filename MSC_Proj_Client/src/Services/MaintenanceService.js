import { getAccessToken } from "./UserService";
import ApiReponse from "@/Models/ApiResponse";
export async function DeleteMaintenance(id){
    try{
    let response = await fetch(`api/items/maintenance/${id}`,{
        method:"DELETE",
        headers:{
            Authorization : await getAccessToken()
        }  
    })
    if(response.ok){
        return new ApiReponse(true)
    }
    else{
        return new ApiReponse(false,null,response.status)
    }
}
catch(e){
    console.warn(e.message)
    return new ApiReponse(false,null,e.message)
}
}