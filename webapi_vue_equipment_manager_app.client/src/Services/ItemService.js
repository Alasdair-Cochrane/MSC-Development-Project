const route = "api/Items"
import { Item } from "@/Models/Item";
import { FormatDate } from "./FormatService";

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

export async function getAllItems() {
    let list = []
    await fetch(route,
        {
           method : "GET",
            headers: {
                "Content-Type" : "application/json"
            }
        }).then((response) => response.json()).then((data)=>
        list.push(...data))
    console.log(list)
    return list;
}

export async function GetItem() {
    console.log(route)

    const response = await fetch(route + "/" + 11,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
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
             "accept" :"application/json"
        },

    })
    if(response.status == 404){
        return [""]
    }
    let list = response.json()
    console.log(list)

    return list;
}

export async function UploadImage(image){
    try{
    const response = await fetch(route +"/image",{
        method: 'POST',
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
