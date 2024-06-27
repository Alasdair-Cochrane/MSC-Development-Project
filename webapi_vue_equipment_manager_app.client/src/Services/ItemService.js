const route = "api/Items"

export async function addItem(item) {
   
    const response = await fetch(route, {
        method: 'POST',
        body: JSON.stringify(item),
    
        headers: {
            "Content-Type" : "application/json"
        }
    }).catch(err => console.warn(err))
    let added = response.json();
    return added
}

export  async function getAllItems() {
    const response = await fetch(route,
        {
           method : "GET",
            headers: {
                "Content-Type" : "application/json"
            }
        })
    let list = response.json()
    return list;
}

export async function GetItem(id) {
    const response = await fetch(route + "/" + id,
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        })
    let list = response.json()

    return list;
}
