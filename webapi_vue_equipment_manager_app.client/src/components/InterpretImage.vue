<script setup>
import CaptureImage from "@/components/CaptureImage.vue";
import { queryLabelImage } from "@/Services/ItemService";
import {ref} from 'vue'

const emit = defineEmits(['cancelled'])
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

async function imageSubmitted(image){
    imageConfirmed.value = true;
    if(image){
        var response = await queryLabelImage(image)

        console.log(response)
        awaitingResponse.value = false;
        if(response.isValidLabel){
            serial.value = response.item.serialNumber
            modelNumber.value = response.item.modelNumber,
            modelName.value = response.item.modelName,
            manufacturer.value = response.item.manufacturer,
            weight.value = response.item.weight,
            height.value = response.item.height,
            length.value = response.item.length,
            width.value = response.item.width,
            category.value = response.item.category
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
            <span><InputText v-model="serial"></InputText></span>
        </div>
        <div v-if="modelName">
            <label>Model Name</label>
            <span><InputText v-model="modelName"></InputText></span>
        </div>
        <div v-if="modelNumber">
            <label>Model Number</label>
            <span><InputText v-model="modelNumber"></InputText></span>
        </div>
        <div v-if="manufacturer">
            <label>Manufacturer</label>
            <span><InputText v-model="manufacturer"></InputText></span>
        </div>
        <div v-if="category">
            <label>Category</label>
            <span><InputText v-model="category"></InputText></span>
        </div>
        <div v-if="weight">
            <label>Weight</label>
            <span><InputText v-model="weight"></InputText></span>
        </div>
        <div v-if="height">
            <label>Height</label>
            <span><InputText v-model="height"></InputText></span>
        </div>
        <div v-if="length">
            <label>Length</label>
            <span><InputText v-model="length"></InputText></span>
        </div>
        <div v-if="width">
            <label>Width</label>
            <span><InputText v-model="width"></InputText></span>
        </div>
        <ul id="errorList" v-if="errors.length > 0">
            <li v-for="x in errors">{{ x }}</li>
        </ul>

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