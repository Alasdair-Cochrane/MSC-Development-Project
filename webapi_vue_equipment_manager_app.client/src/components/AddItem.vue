<script>
    import { reactive, ref } from "vue"
    import { Item } from '@/Models/Item';
    import {EquipmentModel } from '@/Models/EquipmentModel'
    import AddItem  from '@/Services/ItemService'

    export default {
        setup() {
            const item = reactive(new Item());
            const model = reactive(new EquipmentModel());
            var added = ref("");

            async function Add() {
                item.Model = model;
                added.value = await AddItem(item);
            }

            return {
                item,
                Add,
                added,
                model,
            }

        }
    }
</script>
<template>
    <form>
        <input v-model="item.SerialNumber" placeholder="serial number" />
        <input  v-model="model.model_Number" placeholder="Model Number" />
        <input  v-model="model.model_Name" placeholder="Model Name" />
        <input  v-model="model.Manufacturer" placeholder="Manufacturer" />
        <input  v-model="model.category" placeholder="Category" />
        <input  v-model="model.Description" placeholder="Description" />
        <input  v-model="model.Weight" placeholder="Weight" />
        <input  v-model="model.Height" placeholder="Height" />
        <input  v-model="model.Length" placeholder="Length" />
        <input  v-model="model.Depth" placeholder="Depth" />
        <input type="datetime" v-model="item.Date_of_Reciept" placeholder="Date of Reciept" />
        <input type="datetime" v-model="item.Date_Acceptance_Test" placeholder="Date of Acceptance Test" />
        <input type="datetime" v-model="item.Date_of_Activation" placeholder="Date of Commissioning" />
        <input type="checkbox" v-model="item.New_On_Reciept" placeholder="serial number" />
        <input type="checkbox" v-model="item.New_On_Reciept" placeholder="New on Reciept?" />
        <input  v-model="item.CurrentStatus" placeholder="Current Status" />
        <input  v-model="item.UnitName" placeholder="Unit" />       
    </form>
    <p>{{ added }}</p>
    <button type="submit" @click="Add">Submit</button>
</template>