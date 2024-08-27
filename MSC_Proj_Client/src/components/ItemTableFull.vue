<script setup> 
import { ref, onMounted } from 'vue';
import { FilterMatchMode, FilterOperator } from '@primevue/core/api';
import { getAllItems } from '@/Services/ItemService';
import {store} from '@/Store/Store';
import { FormatDate } from '@/Services/FormatService';
import AddItem from './AddItem.vue';

const items = ref([])
const selectedItems = ref()
const loading = ref(true)
const editedRows = ref([])
const addNew = ref(false)
const dt = ref()

const filters = ref({
        global: {value:null, matchMode: FilterMatchMode.CONTAINS},
        'model.modelNumber':{operator: FilterOperator.AND, constraints: [{value:null, matchMode: FilterMatchMode.STARTS_WITH}]},
        'model.modelName':{operator: FilterOperator.AND, constraints: [{value:null, matchMode: FilterMatchMode.STARTS_WITH}]},
        'model.manufacturer':{operator: FilterOperator.AND, constraints: [{value:null, matchMode: FilterMatchMode.STARTS_WITH}]},
        'model.category':{value:null, matchMode:FilterMatchMode.IN},
        currentStatus: {value:null, matchMode:FilterMatchMode.IN},
        unit:{value:null, matchMode:FilterMatchMode.IN},

    })
const clearFilters = () => {
    filters.value ={
        global: {value:null, matchMode: FilterMatchMode.CONTAINS},
        'model.modelNumber':{operator: FilterOperator.AND, constraints: [{value:null, matchMode: FilterMatchMode.STARTS_WITH}]},
        'model.modelName':{operator: FilterOperator.AND, constraints: [{value:null, matchMode: FilterMatchMode.STARTS_WITH}]},
        'model.manufacturer':{operator: FilterOperator.AND, constraints: [{value:null, matchMode: FilterMatchMode.STARTS_WITH}]},
        'model.category':{value:null, matchMode:FilterMatchMode.IN},
        currentStatus: {value:null, matchMode:FilterMatchMode.IN},
       
    }
}

onMounted( async () => {
    items.value = await getAllItems().catch(() => loading.value = false)
    loading.value = false
    shownColumns.value = columns.value;   
})

const columns = ref([
    {label: "Serial Number" , show: true},
    {label: "Local Identifier" , show: true},
    {label: "Barcode" , show: false},
    {label: "Model Number" , show: true},
    {label: "Model Name" , show: true},
    {label: "Manufacturer" , show: true},
    {label: "Category" , show: true},
    {label: "Weight" , show: false},
    {label: "Length" , show: false},
    {label: "Width " , show: false},
    {label: "Height" , show: false},
    {label: "Reciept Date" , show: false},
    {label: "Commission Date" , show: false},
    {label: "Owner" , show: true},
    {label: "Status" , show: true},
])
const shownColumns = ref()

const onToggle = (val) =>
{   
    columns.value.filter(col => val.includes(col)).forEach(col => {col.show = true})
    columns.value.filter(col => !val.includes(col)).forEach(col => {col.show = false})   
    shownColumns.value = val;
}

const onRowEditSave = () => {}

const exportCSV = () => {
    dt.value.exportCSV()
    }
</script>

<template>
    <div class="wrapper">
    <DataTable :value="items" 
    tableStyle="min-width: 50rem" 
    v-model:selection="selectedItems"
    stripedRows showGridlines 
    removablSort 
    :reorderableColumns="true" 
    v-model:filters="filters" 
    filterDisplay="menu"
    :loading="loading"
    :size="'small'" 
    :globalFilterFields="['serialNumber','localName','barcode','model.modelNumber','model.modelName','model.category', 'model.manufacturer']"
    editMode="row"
    v-model:editingRows="editedRows"
    @row-edit-save="onRowEditSave"
    paginator
    :rows="25" 
    :rowsPerPageOptions="[25, 50, 75, 100]"
    scrollable
    scroll-height="80vh"
    ref="dt"
    >
    
<!-- HEADER -->
        <template #header>
            <div class="header-bar">
                <Button label="Add" icon="pi pi-plus" @click="addNew = true"></Button>
                <div style="display: flex; gap:10px">
                    <Button type="button" icon="pi pi-filter-slash" label="Clear" severity="warn" @click="clearFilters()" />

                    <IconField>
                        <InputIcon>
                            <i class="pi pi-search" />
                        </InputIcon>
                        <InputText v-model="filters['global'].value" placeholder="Search" />
                    </IconField>
                </div>
                <Button label="Export" icon="pi pi-external-link" @click="exportCSV($event)"></Button>
                <MultiSelect :modelValue="shownColumns" :options="columns" optionLabel="label" placeholder="Show Columns"
                @update:modelValue="onToggle" display="chip" :maxSelectedLabels=2></MultiSelect>
            </div>
        </template>

    <template #empty> No Items Found. </template>
    <template #loading> Loading data. Please wait. </template>


<!-- SERIAL NUMBER -->
    <Column field="serialNumber" header="Serial Number" sortable v-if="columns[0].show">
        <template #body="{data}">
            {{ data.serialNumber }}
        </template>
    </Column>
<!-- LOCAL NAME -->
    <Column field="localName" header="Local Identifier" sortable filterField="localName" v-if="columns[1].show">
        <template #body="{data}">
            {{ data.localName }}
        </template>
    </Column>

<!-- BARCODE -->
    <Column field="barcode" header="Barcode" sortable filterField="barcode" v-if="columns[2].show">
        <template #body="{data}">
            {{ data.barcode }}
        </template>
    </Column>

<!-- MODEL NUMBER-->
    <Column field="model.modelNumber" header="Model Number" sortable filterField="model.modelNumber" v-if="columns[3].show">
        <template #body="{data}">
            {{ data.model.modelNumber }}
        </template>
        <template #filter="{filterModel}">
            <InputText v-model="filterModel.value" type="text" placeholder="search"></InputText>
        </template>
    </Column>

<!-- MODEL NAME -->
    <Column field="model.modelName" header="Model Name" sortable filterField="model.modelName" v-if="columns[4].show">
        <template #body="{data}">
            {{ data.model.modelName }}
        </template>
        <template #filter="{filterModel}">
            <InputText v-model="filterModel.value" type="text" placeholder="search"></InputText>
        </template>
    </Column>

<!-- MANUFACTURER -->
    <Column field="model.manufacturer" header="Manufacturer" sortable filterField="model.manufacturer" v-if="columns[5].show">
        <template #body="{data}">
            {{ data.model.manufacturer }}
        </template>
        <template #filter="{filterModel}">
            <InputText v-model="filterModel.value" type="text" placeholder="search"></InputText>
        </template>
    </Column>

<!-- CATEGORY -->
    <Column field="model.category" header="Category" sortable :showFilterMatchModes="false" filterField="model.category" v-if="columns[6].show">
        <template #body="{data}">
            {{ data.model.category }}
        </template>
        <template #filter="{filterModel}">
            <MultiSelect v-model="filterModel.value" :options="store.ModelCategories" placeholder="select"></MultiSelect>
        </template>
    </Column>

<!-- WEIGHT -->
    <Column field="model.weight" header="Weight (kg)" sortable filterField="weight" v-if="columns[7].show" dataType="numeric"> 
        <template #body="{data}">
            {{ data.model.weight }}
        </template>
    </Column> 

<!-- LENGTH -->
    <Column field="model.length" header="Length (mm)" sortable filterField="length" v-if="columns[8].show" dataType="numeric">
        <template #body="{data}">
            {{ data.model.length }}
        </template>
    </Column> 

<!-- WIDTH -->
    <Column field="model.width" header="Width (mm)" sortable filterField="width" v-if="columns[9].show" dataType="numeric">
        <template #body="{data}">
            {{ data.model.width }}
        </template>
    </Column> 

<!-- Height -->
    <Column field="model.height" header="Height (mm)" sortable filterField="height" v-if="columns[10].show" dataType="numeric">
        <template #body="{data}">
            {{ data.model.height }}
        </template>
    </Column> 

<!-- DATE OF RECIEPT -->
<Column field="date_of_reciept" header="Reciept Date" sortable v-if="columns[11].show"
    dataType="date">
    <template #body="{data}">
        {{ FormatDate(data.date_of_reciept) }}
    </template>

</Column>

<!-- DATE OF COMMISSIONING -->
<Column field="date_of_commissioning" header="Commission Date" sortable v-if="columns[12].show"
    dataType="date">
    <template #body="{data}" >
        {{ FormatDate(data.date_of_commissioning) }}
    </template>

</Column>
<!-- Unit -->
<Column field="unit" header="Owner" sortable :showFilterMatchModes="false" filterField="unit" v-if="columns[13].show">
        <template #body="{data}">
            {{ data.unitName }}
        </template>
        <template #filter="{filterModel}">
            <MultiSelect v-model="filterModel.value" :options="store.UnitsAuthorised" optionLabel="name" placeholder="select"></MultiSelect>
        </template>
    </Column>

<!-- STATUS -->
    <Column field="currentStatus" header="Status" sortable 
    :showFilterMatchModes="false" filterField="currentStatus" v-if="columns[14].show" >
        <template #body="{data}">
            {{ data.currentStatus }}
        </template>
        <template #filter="{filterModel}">
            <MultiSelect v-model="filterModel.value" :options="store.Statuses" placeholder="select"></MultiSelect>
        </template>
    </Column>
<!-- EDITOR
<Column :rowEditor="true" style="width: fit-content;flex:1; min-width: 4rem" bodyStyle="text-align:center"></Column> -->

</DataTable>

<Dialog v-model:visible="addNew" modal header="Add New Item">
    <AddItem @itemSaved="(i) => {items.push(i); addNew = false}"></AddItem>
</Dialog>
</div>
</template>
<style scoped>
.header-bar{
    display: flex;
    justify-content: space-between
}



</style>