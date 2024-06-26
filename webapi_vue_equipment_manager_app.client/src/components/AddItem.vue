<script>
    import { reactive, ref } from "vue"
    import { Item } from '@/Models/Item';
    import {EquipmentModel } from '@/Models/EquipmentModel'
    import { addItem, getAllItems } from '@/Services/ItemService'

import { store } from "@/Store/UserStore";

    export default {
        setup() {
            const item = reactive(new Item());
            const model = reactive(new EquipmentModel());
            const added = ref("response");
            const items = ref([""])

            async function Add() {
                item.Model = model;
                added.value = await addItem(item);
            }
            async function Get() {
                items.value = await getAllItems();
            }
            return {
                items,
                Add,
                added,
                model,
                store,
                Get,
                item
            }
        }
    }
</script>
<template>
    <v-form>
        <v-text-field v-model="item.SerialNumber" label="serial number" />
        <v-text-field v-model="model.model_Number" label="Model Number" />
        <v-text-field v-model="model.model_Name" label="Model Name" />
        <v-text-field v-model="model.Manufacturer" label="Manufacturer" />
        <v-text-field v-model="model.category" label="Category" />
        <v-text-field v-model="model.Description" label="Description" />

        <v-checkbox v-model="item.New_On_Reciept" label="New on Reciept?" />
        <v-text-field v-model="item.CurrentStatus" label="Current Status" />
        <v-autocomplete :items="store.Units" v-model="item.UnitName" label="Unit" />
    </v-form>
    <button type="submit" @click="Add">Submit</button>

    <p>{{ added }}</p>
    <p>{{items}}</p>
    <button @click="Get">Get Items</button>

</template>