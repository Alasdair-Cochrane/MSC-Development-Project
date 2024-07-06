const route = "api/Items"

export async function addItem(item, image) {

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

