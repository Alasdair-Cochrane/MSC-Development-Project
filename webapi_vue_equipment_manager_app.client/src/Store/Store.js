import { GetAllModels } from '@/Services/EquipmentModelService';
import {reactive, ref, computed} from 'vue'
import { getAccessToken } from '@/Services/UserService';
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

export const UploadUrls = ref({
    ImageUpload: ''
})

export const store = reactive({
    Units : await GetUnits(),
    ModelCategories: ["Centrifuge", "Pipette"],
    Statuses: ['Active'],
    Models: [],
    OrgStructure: await GetStructure(),
})


