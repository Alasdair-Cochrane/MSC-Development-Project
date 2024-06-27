<script>
    import { reactive, ref, onMounted } from "vue"
    import { Item } from '@/Models/Item';
    import {EquipmentModel } from '@/Models/EquipmentModel'
    import { addItem, getAllItems } from '@/Services/ItemService'
    import {GetAllModels} from '@/Services/EquipmentModelService'


import { store } from "@/Store/UserStore";

    export default {
        setup() {
            const item = reactive(new Item());
            const model = ref(new EquipmentModel());
            const added = ref();

            const models = ref()
            const filteredModels = ref()



            onMounted(() => {
                GetAllModels().then((data) => (models.value = data));
                added.value = models.value;
            })

                async function Add() {
                    item.Model = model.value;
                    added.value = await addItem(item);
                }

                //https://primevue.org/autocomplete/
                async function searchModelName(event) {
                    setTimeout(() => {
                        if (!event.query.trim().length) {
                            filteredModels.value = [...models.value];
                        } else {
                            filteredModels.value = models.value.filter((modelX) => {
                                return modelX.model_Name.toLowerCase().startsWith(event.query.toLowerCase());
                            });
                        }
                    }, 250);
            }
            async function searchModelNumber(event) {
                setTimeout(() => {
                    if (!event.query.trim().length) {
                        filteredModels.value = [...models.value];
                    } else {
                        filteredModels.value = models.value.filter((modelY) => {
                            return modelY.model_Number.toLowerCase().startsWith(event.query.toLowerCase());
                        });
                    }
                }, 250);

            }

                return {

                    Add,
                    added,
                    model,
                    store,
                    searchModelName,
                    searchModelNumber,
                    item,
                    filteredModels,
                }
            }
            }
    
   
   
</script>
<template>
    <div>
        <form>
            <FloatLabel>
                <InputText id="serial" v-model="item.SerialNumber" />
                <label for="serial">Serial Number</label>
            </FloatLabel>

            <FloatLabel>
                <InputText id="local" v-model="item.LocalName" />
                <label for="local">Local Identifier</label>
            </FloatLabel>

            <FloatLabel>
                <AutoComplete v-model="model" dropdown inputId="autoMNum" optionLabel="model_Number" :suggestions="filteredModels" @complete="searchModelNumber">
                    <template #option="slotProps">
                        <div class="flex items-center">
                            <div>{{slotProps.option.model_Number}}</div>
                            <div>{{slotProps.option.model_Name}}</div>
                            <div>{{slotProps.option.manufacturer}}</div>
                            </div>
                    </template>
                </AutoComplete>
                <label for="autoMNum">Model Num Auto</label>
            </FloatLabel>

            <FloatLabel>
                <InputText id="mName" v-model="model.model_Name" />
                <label for="mName">Model Name</label>
            </FloatLabel>

            <FloatLabel>
                <InputText id="manufacturer" v-model="model.manufacturer" />
                <label for="manufacturer">Manufacturer</label>
            </FloatLabel>

            <FloatLabel>
                <InputText id="cat" v-model="model.category" />
                <label for="cat">Category</label>
            </FloatLabel>

            <FloatLabel>
                <DatePicker v-model="item.Date_Of_Reciept" inputId="reciept" />
                <label for="reciept">Date Of Reciept</label>
            </FloatLabel>

            <FloatLabel>
                <DatePicker v-model="item.Date_Of_Commissioning" inputId="commission" />
                <label for="commission">Date Of Commissioning</label>
            </FloatLabel>

            <FloatLabel>
        <AutoComplete v-model="item.CurrentStatus" inputId="status" :suggestions="items" @complete="search" />
        <label for="status">Current Satus</label>
    </FloatLabel>

    <FloatLabel>
        <AutoComplete v-model="item.UnitName" inputId="unit" :suggestions="store.Units" @complete="search" />
        <label for="unit">Unit</label>
    </FloatLabel>

            <div class="checkbox">
                <Checkbox v-model="item.New_On_Reciept" name="new" inputID="new" :binary="true" />
                <label for="new">New On Reciept?</label>
            </div>

            <FloatLabel>
                <InputText id="status" v-model="item.CurrentStatus" />
                <label for="status">Current Status</label>
            </FloatLabel>
            <FloatLabel class="inputfield">
                <InputText id="unit" v-model="item.UnitName" />
                <label for="unit">Unit</label>
            </FloatLabel>

        </form>
        <div class="choices">
            <Button label="Save" icon="pi pi-save" @click="Add"></Button>
            <Button label="Clear" icon="pi pi-eraser"></Button>
            <Button label="Add Same" icon="pi pi-plus-circle"></Button>
        </div>

        <p>{{ added }}</p>
    </div>
</template>
<style scoped>
    .p-floatlabel{
        margin-top : 2rem;
    }
    .checkbox{
        margin-top:2rem;
        display: flex;
        justify-content : space-evenly;
    }
    .choices {
        margin-top: 2rem;
        display: flex;
        justify-content: space-evenly;
    }
</style>