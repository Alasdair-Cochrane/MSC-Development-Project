import { GetAllModels } from '@/Services/EquipmentModelService';
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


//GET SET FOR LIST OF UNITS
var units;
const GetUnits = async () => {
    if(!units){
        try{
            await UpdateUnits()
            return units
        }
        catch(ex){
            console.log(ex.message)
            return []
        }
        
    }
    else{
        return units
    }
}

var structure;
const GetStructure = async () =>
    {if(!structure){
        structure = await GetOrgStructure()
        return structure
    }
    else{
        return structure
    }
}
export const UpdateStructure = async () =>
    {structure = await GetOrgStructure();
        return structure;}

export async function UpdateUnits(){
    const response = await fetch('/api/units',
        {
            method: "GET",
            headers:{
                "Authorization" : await getAccessToken()
            }
        }
        
    )
    units = await response.json();
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

//GET SET FOR LIST OF CATEGORIES
export async function UpdateCategories(){
    
}

//GET SET FOR LIST OF STATUSES
export async function UpdateStatuses(){
    
}

//GET SET FOR LIST OF MODELS

export async function UpdateModels(){
    store.Models = await GetAllModels()
}

//GET SET FOR LIST OF USER ROLES
var roles;
const GetRoles = async () =>
{
    if(roles){
        return roles;
    }
    else{
        try{
        let response = await fetch('api/users/roles', {
            method :"GET",
            headers : {
                "Authorization" : await getAccessToken()
            }
        })
        return await response.json()
    }
    catch(ex){
        console.log("Could not retrieve user roles : " + ex.message)
    }
    }
}

//GET FOR USER DETAILS
var user;
const GetUserDetails = async () => {
    if(user){
        return user;
    }
    else{
        try{
        let response = await fetch('api/users/user', {
            method :"GET",
            headers : {
                "Authorization" : await getAccessToken()
            }
        })
        return await response.json()
    }
    catch(ex){
        console.log("Could not retrieve user details : " + ex.message)
    }
    }
}


export const store = reactive({
    Units : await GetUnits(),
    UserDetails : await GetUserDetails(),
    ModelCategories: ["Centrifuge", "Pipette"],
    Statuses: ['Active'],
    Models: [],
    OrgStructure: await GetStructure(),
    PublicUsers: await GetPublicUsers(),
    Roles: await GetRoles(),
})


