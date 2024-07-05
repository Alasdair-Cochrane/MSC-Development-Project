import { GetAllModels } from '@/Services/EquipmentModelService';
import {reactive, ref} from 'vue'

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
