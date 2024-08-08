import { getAccessToken } from "./UserService"

export async function Download(doc){
    try{
    const response = await fetch('api/documents/' + doc.url,{
        method: "GET",
        headers :{
            "Authorization" : await getAccessToken()
        }
    })
    if(response.ok){
        let result = await response.blob()
        let fileUrl = URL.createObjectURL(result)

        const a = document.createElement('a');
        a.href = fileUrl
        a.download = doc.fileName
        a.target = "_blank"
        document.body.appendChild(a)
        a.click()
        document.body.removeChild(a)
        return {successfull: true}
    } 
    else{
        let err = await response.json()
        console.warn(err)
        return {successfull: false, status : response.status, errors: err}
    }
}
catch(e){
    return {successfull: false, errors : e.message}
}
}

export async function UploadItemFile(file, itemId){
    let route = `api/items/${itemId}/documents`
    return await uploadFile(route, file)

}

export async function UploadMaintenanceFile(file, mainId){
    let route = `api/items/maintenance/${mainId}/documents`
    return await uploadFile(route,file)
}

async function uploadFile(route, file){
    try{
    let data = new FormData()
    data.append('file',file )
    let response = await fetch(route, {
        method: "POST",
        headers:{
            "Authorization": await getAccessToken(),
        },
        body: data
    })
    if(response.ok){
        let result = await response.json()     
        return {successfull: true, file: result}   
    }
    else{
        let err = await response.json()
        return {successfull: false, error: response.status, message : err ?? null }
    }
}
catch(e){
    return {successfull: false, error: e.message}
}
}

export async function DeleteFile(file){
    try{
        const response = await fetch('api/documents/' + file.url,{
            method: "DELETE",
            headers :{
                "Authorization" : await getAccessToken()
            }
        })
        if(response.ok){
            return{successfull : true}
        }
        else{
            return{successfull: false , error : response.status }
        }
    }
        catch(e){
            return{ successfull: false, error : e.message}
        }
}

