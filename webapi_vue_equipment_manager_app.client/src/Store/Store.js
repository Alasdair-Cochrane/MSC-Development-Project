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

export const store = reactive({
    Units : ["Admin",],
    ModelCategories: ["Centrifuge", "Pipette"],
    Statuses: ['Active'],
    Models: [],
})


export async function UpdateUnits(){
    const response = await fetch('/api/units')
    store.Units = response.json();
}
    
export async function UpdateCategories(){
    
}
export async function UpdateStatuses(){
    
}
export async function UpdateModels(){
    store.Models = await GetAllModels()
}

export const UploadUrls = ref({
    ImageUpload: ''
})


