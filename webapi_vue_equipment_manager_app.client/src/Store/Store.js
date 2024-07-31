import {reactive, ref, computed} from 'vue'
import { getAccessToken, GetUsers } from '@/Services/UserService';
import { GetOrgStructure } from '@/Services/UnitService';
//Reactive Local store using computed getter/setter

const l = ref(localStorage.getItem("loggedIn") === "true")
export const loggedIn = computed({ 
    get:() => {return l.value}, 
    set: (value) => 
    {l.value = value;
    localStorage.setItem('loggedIn',value.toString())}
})
export const toggleLogIn = () =>  loggedIn.value = true
export const toggleLogOut = () =>  loggedIn.value = false

export const GetFromAPI = async (route, errorMessage) => {
    try{
        let response = await fetch(route, {
            method :"GET",
            headers : {
                "Authorization" : await getAccessToken()
            }
        })
        return await response.json()
        }
        catch(ex){
            console.warn(errorMessage + " : " + ex.message)
        }    
}

export const UpdateStructure = async () =>{   
        UpdateUnits()
        store.OrgStructure = await GetOrgStructure();
        return store.OrgStructure
    }


export async function UpdateUnits(){
    store.Units = await GetFromAPI('/api/units', "Could not retrieve Units")
    return store.Units
}

const GetPublicUsers = async () =>{
    store.PublicUsers = await GetUsers()
    return store.Units
}


export async function UpdateCategories(){
    store.ModelCategories = await GetFromAPI('api/models/categories',"Could not retrieve categories")
    return store.ModelCategories
}

export async function UpdateStatuses(){
        store.Statuses = await GetFromAPI('api/items/statuses/names', "Could not retrieve status names")
        return store.statuses
    }    

async function UpdateStatusQuantities(){
    store.StatusQuantities = await GetFromAPI("api/items/statuses", "Could not retrieve status quantities")
    return store.StatusQuantities
}


export async function UpdateModels(){
    store.Models = await GetFromAPI("/api/Models", "Could not retrieve models")
    return store.Models
}

const UpdateRoles = async () =>{
       store.Roles = await GetFromAPI('api/users/roles', "Could not retrieve user roles")
        return store.Roles
    }

const UpdateUserDetails = async () => {
        store.UserDetails = await GetFromAPI('api/users/user', "Could not retrieve user details")
        return store.UserDetails
    }


const  UpdateMaintenanceCategories = async () => {
        store.MaintenanceCategories = await GetFromAPI('api/items/maintenance/names', "Could not retrieve maintenance categories")
        return store.MaintenanceCategories
    }



export async function PopulateStartingData() {
    await Promise.all([UpdateUserDetails(),UpdateStructure(),GetPublicUsers(),
        UpdateRoles(), UpdateCategories(), UpdateStatuses(), UpdateStatusQuantities(),
        UpdateMaintenanceCategories()])
}

export async function UpdateItemData(){
    UpdateStatusQuantities()
    UpdateModels()
    UpdateCategories()
}


export const store = reactive({
    Units : ref([]),
    UserDetails : ref([]),
    ModelCategories: ref([]),
    Statuses: ref([]),
    Models: ref([]),
    OrgStructure: ref([]),
    PublicUsers: ref([]),
    Roles: ref([]),
    StatusQuantities: ref([]),
    MaintenanceCategories : ref([]),
})


