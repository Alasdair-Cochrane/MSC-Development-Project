
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
   

