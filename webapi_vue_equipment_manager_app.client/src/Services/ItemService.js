const route = "api/Items"
import { Item } from "@/Models/Item";
import { FormatDate } from "./FormatService";
import { getAccessToken } from "./UserService";

export async function addItem(item, image) {

    item.date_of_commissioning = FormatDate(item.date_of_commissioning)
    item.date_of_reciept = FormatDate(item.date_of_reciept)

    const formdata = new FormData();
    Object.keys(item).forEach(key => {if(item[key]) formdata.append(key,item[key])})
    Object.keys(item.model).forEach(key => {if(item.model[key]) formdata.append(key,item.model[key])})
    if(image) {formdata.append('image',image)}

    for(let pair of formdata.entries()){
        console.log(pair[0] + ": " + pair[1]);
    }
    
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
        return {successful : true , item: added}
    }
    else{
        const errorDetails = await response.json()
        console.log(`HTTP RESPONSE:${response.status} ${response.statusText}`)
        console.log('Erorror Details:', errorDetails)
        return {successful : false, error: errorDetails}
    }
    }catch(err){
        console.error('Fetch Error: ',err)
        return {successful : false, error: err}
    }
}

export async function updateItem(item){
    item.date_of_commissioning = FormatDate(item.date_of_commissioning)
    item.date_of_reciept = FormatDate(item.date_of_reciept)    
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
        console.log("update successful" + JSON.stringify(updated))
        return {successful : true , item: updated}
    }
    else{
        const errorDetails = await response.json()
        console.log(`HTTP RESPONSE:${response.status} ${response.statusText}`)
        console.log('Erorror Details:', errorDetails)
        return {successful : false, error: errorDetails}
    }
    }catch(err){
        console.error('Fetch Error: ',err)
        return {successful : false, error: err}
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
    console.log(route)

    const response = await fetch(route + "/" + 11,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization" : await getAccessToken()

            }
        })
    let list = response.json()
    console.log(list)


    return list;
}

export async function QueryItems(key, value){

    let query = `?${key}=${value}`
    console.log(route + query)
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
    let queryParams = new URLSearchParams(propertiesObject).toString()
    console.log(queryParams)
    const response = await fetch(route + "?" + queryParams,
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

export async function UploadImage(image){
    try{
    const response = await fetch(route +"/image",{
        method: 'POST',
        headers : {
            "Authorization" : await getAccessToken()
        },
        body:image})
        if(response.ok){
            return {successful : true, url: response.json()}
        }
        else{
            return {successful : false, error: response.statusText}
        }
    }
    catch(err){
        console.log(err)
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
            console.log(await response.text())
            return {isValidLabel : false, errors : [`Request to server failed: ${response.statusText}`]}
        }
    }
    catch(err){
        return {isValidLabel: false, errors: [err]}
    }
}
