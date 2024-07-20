import { GetAllModels } from '@/Services/EquipmentModelService';
import {reactive, ref, computed} from 'vue'

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
export async function UpdateUnits(){
    const response = await fetch('/api/units')
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
})


