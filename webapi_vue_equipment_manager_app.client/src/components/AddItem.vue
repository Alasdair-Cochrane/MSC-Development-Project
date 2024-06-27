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
  
    <v-app>
        <v-form>
            <v-container>
                <v-row>
                    <v-col>
                        <v-text-field class="input-field" v-model="item.SerialNumber" label="Serial Number" />
                        <v-text-field v-model="item.LocalName" label="Local Identifier" />
                    </v-col>
                </v-row>
                <v-row>
                    <v-col>
                        <v-text-field v-model="model.model_Number" label="Model Number" />
                        <v-text-field v-model="model.model_Name" label="Model Name" />
                        <v-text-field v-model="model.Manufacturer" label="Manufacturer" />
                        <v-text-field v-model="model.category" label="Category" />
                    </v-col>
                </v-row>
                <v-row>
                    <v-col>
                        <v-text-field  type="date" v-model="item.Date_Of_Reciept" label="Date of Reciept" />
                        <v-text-field  type="date" v-model="item.Date_Of_Commissioning" label="Date of Commissioning" />
                        <v-checkbox  v-model="item.New_On_Reciept" label="New?" />
                        <v-text-field v-model="item.CurrentStatus" label="Current Status" />
                        <v-autocomplete  :items="store.Units" v-model="item.UnitName" label="Unit" />
                    </v-col>
                </v-row>
            </v-container>
        </v-form>
        <v-btn variant="outlined" prepend-icon="mdi-content-save" type="submit" @click="Add">Save</v-btn>

        <p>{{ added }}</p>
    </v-app>

</template>