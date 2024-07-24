<script setup>
import { Unit } from '@/Models/Unit';
import { addUnit } from '@/Services/UnitService';
import { store, UpdateUnits } from '@/Store/Store';
import {onMounted, ref} from 'vue'

    const newUnit = ref(new Unit())
    const currentUnits = ref(store.Units)
    const street = ref()
    const city = ref()
    const postcode = ref()
    const selectedParent = ref()
    const noName = ref(false)
    const loading = ref(false)
    const successfullyAdded = ref(false)
    const errorOccured = ref(false)


    const props = defineProps({delay:{
        type : Boolean,
        default: false
        },
        small:{
            type:Boolean,
            default: true,
        },
        parent:{
            type : Unit,
            default : null
        }
        })
    const emit = defineEmits(['confirmed', "cancelled"])

onMounted(() => {
    selectedParent.value = props.parent
}
)


const buildAddress = () =>
   {
    let string = ""
    if(street.value) {
        string = string + street.value + ",";
    }
    if(city.value) {
        string = string + city.value + ","
    }
    if(postcode.value) {
       string = string + postcode.value + ","
    }
    if(string === "") return null
    else return string
   }

async function addSimilarUnit(){
    successfullyAdded.value = false
    errorOccured.value = false;
    noName.value = false;
    loading.value = true;

    if(!newUnit.value.name){
        noName.value = true
        console.log("Name field required")
        loading.value = false;
        return;
    }
    
    noName.value = false;
    
    newUnit.value.address = buildAddress()

    if(selectedParent.value){
        newUnit.value.parentId = selectedParent.value.id
    }
    else {
        newUnit.value.parentId = null;
    }

    if(props.delay){
        emit('confirmed', newUnit.value)
        loading.value = false;
        return
    }

    var response = await addUnit(newUnit.value)
    if(response.successful){
        newUnit.value.name = null
        emit('confirmed', newUnit.value)
        loading.value = false;
        successfullyAdded.value = true
        currentUnits.value = await UpdateUnits()
        return true
    }
    else{
        console.log(response.error + " " + response?.message)
        loading.value = false;
        errorOccured.value = true;
        return false
    }
}

function clear(){
    successfullyAdded.value = false;
    errorOccured.value = false;
    newUnit.value = new Unit()
    street.value = null
    city.value = null
    postcode.value = null
}

async function addNew(){
    if(await addSimilarUnit()){
        clear()
        successfullyAdded.value= true;
    }
}

</script>
<template>
<div class="container">
    <div class="fields">
        <div class="input-field">
            <label for="name">Name *</label>
            <InputText id="name" size="small" v-model="newUnit.name" :invalid="noName"/>
            <small v-show="noName">Name Field Required</small>
        </div>
        <div class="input-field">
            <label for="name">Room</label>
            <InputText id="room" size="small" v-model="newUnit.room"/>
        </div>
        <div class="input-field">
            <label for="building">Building</label>
            <InputText id="building" size="small" v-model="newUnit.building"/>
        </div>
        <div class="input-field">
            <label for="street">Street</label>
            <InputText id="street" size="small" v-model="street"/>
        </div>
        <div class="input-field">
            <label for="city">City</label>
            <InputText id="city" size="small" v-model="city"/>
        </div>
        <div class="input-field">
            <label for="postcode">Postcode</label>
            <InputText id="postcode" size="small" v-model="postcode"/>
        </div>
        <div class="input-field" v-if="!delay" >
            <label for="parent">Parent Unit</label>
            <Select id="parent" :options="currentUnits" optionLabel="name" v-model="selectedParent" placeholder="Select Unit" />
        </div>
        <div id="public-check">
            <Checkbox id="public" v-model="newUnit.isPublic" :binary="true"></Checkbox>
            <label for="public">Make Public</label>
        </div>
    </div>
    <div class="bttns">
        <div class="submit-btns" >
            <Button icon="pi pi-save" label="Save" class="s-btn" @click="addNew" :loading="loading"></Button>
            <Button icon="pi pi-plus" label="Save & New" class="s-btn" @click="addSimilarUnit" v-if="!small" :loading="loading "></Button>
            <Button icon="pi pi-eraser" label="Clear" severity="danger" class="s-btn" @click="clear" v-if="!small" v-show="!loading "></Button>
            <Button v-if="small" label="Cancel" @click="$emit('cancelled')" severity="danger" class="s-btn" v-show="!loading"></Button>
        </div>
    </div>
    <label v-show="!delay && successfullyAdded">Successfully Added</label>
    <label v-show="!delay && errorOccured">Failed to Add : </label>
</div>
</template>
<style scoped>
.wrapper{
    max-width: 350px;
}
.fields{
    display: flex;
    flex: 1;
    flex-direction: column;
    align-items: center;
}
small{
    color: red;
    font-size: x-small
}
.input-field{
    display: grid;
    grid-template-columns: 1fr;
    grid-template-rows: 1fr, 1fr;
    width: fit-content;
    margin-right: 0.5rem;
    margin-top: 0.5rem;
}
.submit-btns{
    display: flex;
    justify-content: space-around;
    flex-wrap: wrap;
    padding: 0.5rem;
    gap:0.5rem;
    z-index: 98;
    margin-top: 1rem;
}
.submit-btns Button{
    height: 3rem;
    flex: 1;
}

.s-btn :deep(.p-button-label){
    font-size: smaller;
    @container(width < 30px){
        display: none;
    }
}

.s-btn :deep(.p-button){
    padding: 0;
}

.p-select{
    min-width: 180px;
}

#public-check{
    display: flex;
    justify-content: space-around;
    margin: 1rem;
    gap: 0.5rem;
    align-items: center;
}
</style>


