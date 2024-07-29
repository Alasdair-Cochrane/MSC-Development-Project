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

const GetFromAPI = async (route, errorMessage) => {
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


//GET SET FOR LIST OF UNITS
var units;
const GetUnits = async () => {
    if(units){
        return units;
    }
    else{
        let result = UpdateUnits()
        units = result
        return result
    }
}

var structure;
export async function GetStructure(){
    if(structure){
        return structure
    }
    else{
        structure = await GetOrgStructure()
        return structure
    }
}
export const UpdateStructure = async () =>{   
        UpdateUnits()
        structure = await GetOrgStructure();
        return structure;
    }


export async function UpdateUnits(){
    let result = await GetFromAPI('/api/units', "Could not retrieve Units")
    return result;
}

//GET SET FOR LIST OF PUBLIC USERS
var users;
const GetPublicUsers = async () =>
{
    if(!users){
        users = await GetUsers();
        return users
    }
    else{
        return users;
    }
}

//GET SET FOR LIST OF MODEL CATEGORIES
var categories;
async function GetCategories(){
    if(categories){
        return categories
    }
    else{
        categories = await UpdateCategories()
        return categories
    }
}

export async function UpdateCategories(){
    let result = await GetFromAPI('api/models/categories',"Could not retrieve categories")
    categories = result
    return result;
}

var statuses;
//GET LIST OF STATUSES
export async function GetStatuses(){
    if(statuses){
        return statuses
    }
    else{
        let result = await GetFromAPI('api/items/statuses/names', "Could not retrieve status names")
        statuses = result
        return result
    }    
}
const statusQuantities = ref()
async function UpdateStatusQuantities(){
    let result = await GetFromAPI("api/items/statuses", "Could not retrieve status quantities")
    statusQuantities.value = result
    console.log(result)
    return result
}
async function GetStatusQuantities(){
    if(statusQuantities.value){
        return statusQuantities
    }
    else{
        statusQuantities.value = await UpdateStatusQuantities()
        return statusQuantities
    }
}


//GET SET FOR LIST OF MODELS
var models 
async function GetModels(){
    if(models){
        return models
    }
    else{
        models = await UpdateModels()
        return models
    }
}

export async function UpdateModels(){
    let result = await GetFromAPI("/api/Models", "Could not retrieve models")
    return result
}

//GET SET FOR LIST OF USER ROLES
var roles;
const GetRoles = async () =>
{
    if(roles){
        return roles;
    }
    else{
        let result = await GetFromAPI('api/users/roles', "Could not retrieve user roles")
        roles = result;
        return result;
    }
}

//GET FOR USER DETAILS
var user;
const GetUserDetails = async () => {
    if(user){
        return user;
    }
    else{
        let result = await GetFromAPI('api/users/user', "Could not retrieve user details")
        user = result
        return result
    }
}

//GET FOR MAINTENANCE CATEGORIES
var maintenanceCategories
async function GetMaintenanceCategories(){
    if(maintenanceCategories){
        return maintenanceCategories
    }
    else{
        maintenanceCategories = await GetFromAPI('api/items/maintenance/names', "Could not retrieve maintenance categories")
        return maintenanceCategories
    }
}



export async function PopulateStartingData() {
    await Promise.all([GetUnits(), GetUserDetails(),GetStructure(),GetPublicUsers(),GetRoles(), GetCategories(), GetStatuses(), GetStatusQuantities()])
}

export async function UpdateItemData(){
    UpdateStatusQuantities()
    UpdateModels()
    UpdateCategories()
}


export const store = reactive({
    Units : await GetUnits(),
    UserDetails : await GetUserDetails(),
    ModelCategories: await GetCategories(),
    Statuses: await GetStatuses(),
    Models: await GetModels(),
    OrgStructure: await GetStructure(),
    PublicUsers: await GetPublicUsers(),
    Roles: await GetRoles(),
    StatusQuantities: await GetStatusQuantities(),
    MaintenanceCategories : await GetMaintenanceCategories()
})


