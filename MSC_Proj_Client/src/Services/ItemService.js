const route = "api/Items"
import { Item } from "@/Models/Item";
import { FormatDate } from "./FormatService";
import { getAccessToken } from "./UserService";
import { UpdateItemData } from "@/Store/Store";
import ApiReponse from "@/Models/ApiResponse";

export async function addItem(item, image) {

    const formdata = new FormData();
    Object.keys(item).forEach(key => {if(item[key]) formdata.append(key,item[key])})
    Object.keys(item.model).forEach(key => {if(item.model[key]) formdata.append(key,item.model[key])})
    if(image) {formdata.append('image',image)}
    try{
    const response = await fetch(route, {
        method: 'POST',
        headers : {
            "Authorization" : await getAccessToken()
        },
        body: formdata,
    })
    if(response.ok) {
        let added = await response.json();
        return {successful : true , newItem: added}
    }
    else{
        const errorDetails = await response.json()
        console.warn(`HTTP RESPONSE:${response.status} ${response.statusText}`)
        console.warn('Error Details:', errorDetails)
        UpdateItemData()
        return {successful : false, error: errorDetails}
    }
    }catch(err){
        console.error('Fetch Error: ',err)
        return {successful : false, error: err}
    }
}

export async function UpdateItem(item){
    if(item.date_of_commissioning){
        let cDate = new Date(item.date_of_commissioning)
        cDate = cDate.toISOString()
        item.date_of_commissioning = cDate
    }
    if(item.date_of_reciept){
        let rDate = new Date(item.date_of_reciept)
        rDate = rDate.toISOString()
        item.date_of_reciept = rDate
    }
    try{
    const response = await fetch(route, {
        method: 'PUT',
        headers: {
            "Content-Type" : "application/json",
            "Authorization" : await getAccessToken()

        },
        body: JSON.stringify(item),
    })
    if(response.ok) {
        let updated = await response.json();
        UpdateItemData()
        return {successful : true , item: updated}
    }
    else{
        const errorDetails = await response.json()
        console.warn(`HTTP RESPONSE:${response.status} ${response.statusText}`)
        console.warn('Error Details:', errorDetails)
        return {successful : false, error: errorDetails}
    }
    }catch(err){
        console.error('Fetch Error: ',err)
        return {successful : false, error: err}
    }
}

export async function DeleteItem(itemId){
    try{
    let response = await fetch(route + "/" + itemId,{
        method : "DELETE",
        headers:{
            "Authorization" : await getAccessToken()
        }
    })
    if(response.ok){
        UpdateItemData()
        return new ApiReponse(true)
    }
    else{
        let err = await response.json()
        return new ApiReponse(false,response.statusText, err)
    }
}
catch(e)
{
    return new ApiReponse(false,e.message)
}
}

export async function getAllItems() {
    let list = []
    await fetch(route,
        {
           method : "GET",
            headers: {
                "Content-Type" : "application/json",
                "Authorization" : await getAccessToken()

            }
        }).then((response) => response.json()).then((data)=>
        list.push(...data))
    return list;
}

export async function GetItem() {

    const response = await fetch(route + "/" + 11,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization" : await getAccessToken()

            }
        })
    let list = response.json()

    return list;
}

export async function QueryItems(key, value){

    let query = `?${key}=${value}`
    const response = await fetch(route + query, 
        {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
             "accept" :"application/json",
             "Authorization" : await getAccessToken()

        },

    })
    if(response.status == 404){
        return []
    }
    let list = await response.json()
    return list;
}

export async function searchItemsByProperties(propertiesObject){
    let queryParams = "?"
    console.log(propertiesObject)
    for(const key in propertiesObject){
        if(propertiesObject[key]){
        queryParams = queryParams + `${key}=${propertiesObject[key]}&`
        }
    }
    console.log(queryParams)
    const response = await fetch(route + queryParams,
        {method: "Get",
            headers: {
                "Content-Type": "application/json",
                 "accept" :"application/json",
                 "Authorization" : await getAccessToken()

            },
        }
    )
    if(response.status == 404){
        return []
    }
    let list = await response.json()
    return list;
}

export async function UploadImage(itemid, image){
    let data = new FormData()
    data.append('image', image)
    try{
    const response = await fetch(route +"/" +itemid + "/image",{
        method: 'POST',
        headers : {
            "Authorization" : await getAccessToken()
        },
        body:data})
        if(response.ok){
            let result = await response.text()
            return {successful : true, url: result}
        }
        else{
            return {successful : false, error: response.statusText}
        }
    }
    catch(err){
        return {successful : false, error: err}

    }
}



export async function queryLabelImage(image){
    const data = new FormData()
    if(image) {
        data.append('image', image)
    }
    else{
        return {isValidLabel: false, errors: "submitted image is empty"}
    }
    try{
        const response = await fetch("api/ai",{
            method: "POST",
            headers : {
                "Authorization" : await getAccessToken()
            },
            body: data
        })
        if(response.ok){            
            var queryResponse =  await response.json();            
            if(queryResponse.isValidLabel){
                return {
                    isValidLabel : true,
                    item : {
                        serialNumber : queryResponse.query.serialNumber,
                        modelName : queryResponse.query.modelName,
                        modelNumber : queryResponse.query.modelNumber,
                        manufacturer : queryResponse.query.manufacturer,
                        category : queryResponse.query.category,
                        weight : queryResponse.query.weight,
                        height : queryResponse.query.height,
                        length : queryResponse.query.length,
                        width : queryResponse.query.width,
                    },
                    errors :  queryResponse.errors
                }                
            }
            else{
                return {isValidLabel : false, errors : queryResponse.errors}
            }
        }
        else{
            return {isValidLabel : false, errors : [`Request to server failed: ${response.statusText}`]}
        }
    }
    catch(err){
        return {isValidLabel: false, errors: [err]}
    }
}
