import { getAccessToken } from "./UserService";

const apiRoute = "/api/Models"

export default async function AddNew(model) {
    console.log(JSON.stringify(model));
    const response = await fetch(apiRoute, {
        method : "POST",
        body : JSON.stringify(model),
        headers: {
            "Content-Type": "application/json",
            "Authorization" : await getAccessToken()

        }
    })
    return response.json();
}

export async function GetAllModels() {
    const response = await fetch(apiRoute,
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
   

