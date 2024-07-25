<script setup>
import CaptureImage from "@/components/CaptureImage.vue";
import { EquipmentModel } from "@/Models/EquipmentModel";
import { Item } from "@/Models/Item";
import { queryLabelImage } from "@/Services/ItemService";
import {ref} from 'vue'

const emit = defineEmits(['cancelled', 'confirmed'])
const imageConfirmed = ref(false)
const awaitingResponse = ref(true)


const serial = ref(null)
const modelNumber = ref(null)
const modelName = ref(null)
const manufacturer = ref(null)
const category = ref(null)
const weight = ref(null)
const height = ref(null)
const length = ref(null)
const width = ref(null)

const errors = ref([])


function cancel(){
    emit('cancelled')
}

function confirm(){
    const item = new Item(new EquipmentModel(null,modelNumber.value,
    modelName.value,null,
    manufacturer.value,
    weight.value,
    height.value,
    length.value,
    width.value,
    category.value),
    null,
    serial.value)
    emit('confirmed', item)
}

async function imageSubmitted(image){
    imageConfirmed.value = true;
    if(image){
        var response = await queryLabelImage(image)

        console.log(response)
        awaitingResponse.value = false;
        if(response.isValidLabel){
            serial.value = response.newItem.serialNumber
            modelNumber.value = response.newItem.modelNumber,
            modelName.value = response.newItem.modelName,
            manufacturer.value = response.newItem.manufacturer,
            weight.value = response.newItem.weight,
            height.value = response.newItem.height,
            length.value = response.newItem.length,
            width.value = response.newItem.width,
            category.value = response.newItem.category
            errors.value = response.errors
        }
        else{
            errors.value = response.errors
        }
    }
    
}


</script>
<template>
    <CaptureImage v-if="!imageConfirmed" @cancelled="cancel" @imageConfirmed="imageSubmitted"></CaptureImage>
    <div v-if="imageConfirmed && !awaitingResponse">
        <div v-if="serial">
            <label>Serial Number</label>
            <InputGroup>
                <InputText v-model="serial"></InputText>
                <Button icon="pi pi-times" @click="serial = ''" severity="danger"/>
            </InputGroup>
        </div>
        <div v-if="modelName">
            <label>Model Name</label>
            <InputGroup>
                <InputText v-model="modelName"></InputText>
                <Button icon="pi pi-times" @click="modelName = ''" severity="danger"/>
            </InputGroup>
            
        </div>
        <div v-if="modelNumber">
            <label>Model Number</label>
            <InputGroup>
                <InputText v-model="modelNumber"></InputText>
                <Button icon="pi pi-times" @click="modelNumber = ''" severity="danger"/>
            </InputGroup>
        </div>
        <div v-if="manufacturer">
            <label>Manufacturer</label>
            <InputGroup>
                <InputText v-model="manufacturer"></InputText>
                <Button icon="pi pi-times" @click="manufacturer = ''" severity="danger"/>
            </InputGroup>
        </div>
        <div v-if="category">
            <label>Category</label>
            <InputGroup>
                <InputText v-model="category"></InputText>
                <Button icon="pi pi-times" @click="category = ''" severity="danger"/>
            </InputGroup>
        </div>
        <div v-if="weight">
            <label>Weight</label>
            <InputGroup>
                <InputText v-model="weight"></InputText>
                <Button icon="pi pi-times" @click="weight = ''" severity="danger"/>
            </InputGroup>
        </div>
        <div v-if="height">
            <label>Height</label>
            <InputGroup>
                <InputText v-model="height"></InputText>
                <Button icon="pi pi-times" @click="height = ''" severity="danger"/>
            </InputGroup>
        </div>
        <div v-if="length">
            <label>Length</label>
            <InputGroup>
                <InputText v-model="length"></InputText>
                <Button icon="pi pi-times" @click="length= ''" severity="danger"/>
            </InputGroup>
        </div>
        <div v-if="width">
            <label>Width</label>
            <InputGroup>
                <InputText v-model="width"></InputText>
                <Button icon="pi pi-times" @click="width = ''" severity="danger"/>
            </InputGroup>
        </div>
        <div v-if="errors.length > 0">
            <h3>Errors</h3>
            <ul id="errorList" >
                <li v-for="x in errors">{{ x }}</li>
            </ul>
        </div>

        <div class="bttns">
            <Button label="Confirm" @click="confirm"></Button>
            <Button label="Cancel" @click="cancel"></Button>

        </div>
    </div>
    <div v-if="imageConfirmed && awaitingResponse">
        <Skeleton class="mb-2"></Skeleton>
        <Skeleton class="mb-2"></Skeleton>
        <Skeleton class="mb-2"></Skeleton>
        <Skeleton class="mb-2"></Skeleton>
        <Skeleton class="mb-2"></Skeleton>        
    </div>
</template>

<style>
</style>