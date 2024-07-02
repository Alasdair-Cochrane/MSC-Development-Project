<script>
    import { reactive, ref, onMounted } from "vue"
    import { Item } from '@/Models/Item';
    import {EquipmentModel } from '@/Models/EquipmentModel'
    import { addItem, getAllItems } from '@/Services/ItemService'
    import {GetAllModels} from '@/Services/EquipmentModelService'
    import { store } from "@/Store/UserStore";
    import { IsMobile } from "@/Services/DeviceService";
    import {useToast} from 'primevue/usetoast'



    export default {
        setup() {
            const value = ref(null)
            const item = ref(new Item())
            const model = ref(new EquipmentModel())
            var mobile = ref(false);
            const condOptions = ["New", "Used"]
            const ownerOptions = ["Admin"]
            const toast = useToast();
            
            onMounted(() => {
                mobile.value = IsMobile();
            })

            const onAdvancedUpload = () => {
            toast.add({ severity: 'info', summary: 'Success', detail: 'File Uploaded', life: 3000 });
};

            return{
                value,
                item,
                model,
                condOptions,
                ownerOptions,
                mobile,
                onAdvancedUpload,
            }

        }
    }
            
</script>
<template>
<div class="grid-nogutter container">
<div class="col-12 md:col-7">
    
    <form >
        <div class="input-group">
            <div class="input-field">
                <label for="serial">Serial Number</label>
                <InputText id="serial" size="small" v-model="item.SerialNumber"/>
            </div>
            <div class="input-field">
                    <label for="serial">Barcode</label>
                    <InputGroup>
                        <InputText id="serial" size="small" v-model="item.Barcode"/>
                        <Button icon="pi pi-barcode"> </Button>
                    </InputGroup>
                </div>
        </div>   
            <div class="input-group">
                <div class="input-field">
                    <label for="serial">Local Name</label>
                    <InputText id="serial" size="small" v-model="item.LocalName"/>
                </div>
                
                <div class="input-field">
            <label for="owner">Owner</label>
            <Select id="owner" size="small" v-model="item.UnitName" showClear :options="ownerOptions"/>
        </div>
        </div>
        
        <div class="input-group">
        <div class="input-field">
            <label for="modelName">Model Name</label>
            <InputText id="modelName" size="small" v-model="model.model_Name"/>
        </div>
        <div class="input-field">
            <label for="modelNum">Model Number</label>
            <InputText id="modelNum" size="small" v-model="model.model_Number"/>
        </div>
        </div>  
        <div class="input-group">
        <div class="input-field">
            <label for="Manufacturer">Manufacturer</label>
            <InputText id="manufacturer" size="small" v-model="model.manufacturer"/>
        </div>
        <div class="input-field ">
            <label for="category">Category</label>
            <InputText id="category" size="small" v-model="model.category"/>
        </div>
        </div>


            <div class="input-field">
                <label for="weight">Weight</label>
                <InputGroup class="num-input">
                    <InputGroupAddon>kg</InputGroupAddon>
                    <InputNumber id="weight" size="small" v-model="model.weight" :minFractionDigits="2"/>             
                </InputGroup>
            </div>
            <div class="input-group">
            <div class="input-field">
                <label for="length">Length</label>
                <InputGroup class="num-input">
                    <InputGroupAddon>mm</InputGroupAddon>
                    <InputNumber id="length" size="small" v-model="model.length"/>             
                </InputGroup>
            </div>  
            <div class="input-field">
                <label for="depth">Depth</label>
                <InputGroup class="num-input">
                    <InputGroupAddon>mm</InputGroupAddon>
                    <InputNumber id="depth" size="small" v-model="model.depth"/>             
                </InputGroup>
            </div>

            <div class="input-field">
                <label for="height">Height</label>
                <InputGroup class="num-input">
                    <InputGroupAddon>mm</InputGroupAddon>
                    <InputNumber id="height" size="small" v-model="model.height"/>             
                </InputGroup>
            </div>
        </div>
 
        <div class="input-group">
            <div class="input-field">
                <label for="recieptDate">Date of Reciept</label>
                <DatePicker showIcon icon-display="input" v-model="item.Date_Of_Reciept"/>
            </div>
            <div class="input-field">
            <label for="condition">Condition on Reciept</label>
            <SelectButton id="condition" size="small" v-model="item.Condition" :options="condOptions"/>
            </div>
        </div>
        <div class="input-field">
        <label for="commisionDate">Date of Commissioning</label>
        <DatePicker showIcon icon-display="input" v-model="item.Date_Of_Commissioning"/>
        </div>
        <div class="input-field">
            <label for="status">Current Status</label>
            <InputText id="status" size="small" v-model="item.CurrentStatus"/>
        </div>
        <div class="submit-btns mobile" >
            <Button icon="pi pi-save" label="Save" class="s-btn"></Button>
            <Button icon="pi pi-plus" label="New" class="s-btn"></Button>
            <Button icon="pi pi-eraser" label="Clear" severity="danger" class="s-btn"></Button>
        </div>
    </form>
    </div>

    <div  class="col-12 md:col-5 right">
        
        <div class="card ">
            <label>Image</label>
                <Toast />
                <FileUpload  class="image-upload" name="demo[]" url="/api/upload" @upload="onAdvancedUpload($event)" :multiple="false" accept="image/*" :maxFileSize="1000000"
                :pt="{}">
                    <template #empty>
                        <span>Drag and drop files to here to upload.</span>
                    </template>
                </FileUpload>
        </div>
        <div class="card document-upload">
            <label>Upload Documents</label>
                <Toast />
                <FileUpload name="demo[]" url="/api/upload" @upload="onAdvancedUpload($event)" :multiple="true" accept=".pdf" :maxFileSize="1000000">
                    <template #empty>
                        <span>Drag and drop files to here to upload.</span>
                    </template>
                </FileUpload>
        </div>
    </div>
    
</div>

</template>
<style scoped>


.input-field{
    display: grid;
    grid-template-columns: 1fr;
    grid-template-rows: 1fr, 1fr;
    width: fit-content;
    margin-right: 0.5rem;

    label{
        color: black;
        margin-block: 0.2rem;
    }
}

.input-group{
    /* border: black 2px solid; */
    display: flex;
    flex-wrap: wrap;
    justify-content: flex-start;
    gap:1rem;
}
form{
    display: flex;
    flex-direction: column;
}

.num-input{
    max-width: 128px;
}
.submit-btns{
    display: flex;
    justify-content: space-around;
    flex-wrap: wrap;
    position: sticky;
    top:0;
    padding: 0.5rem;
    gap:0.5rem;
    z-index: 98;
    
    Button{
        height: 3rem;
        flex: 1;
    }
}

.container{
    flex: 1;
    display: flex;
    flex-wrap: wrap;
    padding: 1rem;
    
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

.p-fileupload :deep(.p-button){
    font-size: smaller;
    flex: 1;
    container-type: inline-size;
 
    }    

.p-fileupload :deep(.p-button-label){
    @container(width < 50px){
        display: none;
}
}

Button {
    container-type: inline-size;
}
.p-fileupload :deep(.p-fileupload-header){
    padding: 0.4rem;
    display: flex;
    justify-content: space-evenly
}

.mobile{
    position: sticky;
    top:0;
}
</style>