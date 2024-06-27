
const apiRoute = "/api/Models"

export default async function AddNew(model) {
    console.log(JSON.stringify(model));
    const response = await fetch(apiRoute, {
        method : "POST",
        body : JSON.stringify(model),
        headers: {
            "Content-Type": "application/json"
        }
    })
    return response.json();
}

export async function GetAllModels() {
    const response = await fetch(apiRoute,
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
   

