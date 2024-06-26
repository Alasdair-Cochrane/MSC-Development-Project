const route = "api/Items"

export default async function addItem(item) {
    console.log(JSON.stringify(item));
    const response = await fetch(route, {
        method: 'POST',
        body: JSON.stringify(item),
    
        headers: {
            "Content-Type" : "application/json"
        }
    })
    
    return response.json();
}