<script setup>
import { DeleteNote, GetNotesForItem } from '@/Services/NotesService';
import { onMounted, ref, watch } from 'vue';
import { FormatDate } from '@/Services/FormatService';

const item = defineModel()
const notes = ref([])
const showAdd = ref(false)
const expandedRows = ref([])
const deleteLoading = ref(false)

onMounted(async () => {if(item.value) notes.value = await GetNotesForItem(item.value.id)})

watch(item, async() => {
    let list = await GetNotesForItem(item.value.id)
    notes.value = list
})

const deleteNote = async(note) =>{
    deleteLoading.value = true
    let response = await DeleteNote(note.id)
    if(response.successfull){
        notes.value = notes.value.filter(x => x.id !== note.id)
    }
    deleteLoading.value = false;
}

</script>
<template>
    <div class="container">
        <div class="notes-header">
            <h3>Notes</h3>
            <Button icon="pi pi-plus" @click="showAdd = true"></Button>
        </div>
        <div class="notes-table">
            <DataTable :value="notes" scrollable scrollHeight="300px" 
              size="small" v-model:expandedRows="expandedRows">
                <Column expander></Column>
                <Column field="title" header="Title"></Column>
                <Column field="datePosted" header="Date" sortable>
                    <template #body="{data}">
                            {{ FormatDate(data.datePosted) }}
                    </template>
                </Column>
                <template #expansion="slotProps">
                    <div class="text-display">
                        <h5 style="align-self: flex-start; font-weight: bold;">{{slotProps.data.userName}}</h5>
                        <p>{{ slotProps.data.text }}</p>
                        <Button icon="pi pi-trash" severity="danger" 
                        v-if="slotProps.data.userCanDelete"
                        style="max-width: 30px; max-height: 30px; justify-self: flex-end;"
                        @click="deleteNote(slotProps.data)"></Button>
                    </div>
                </template>
            </DataTable>
        </div>

    </div>
    <Dialog v-model:visible="showAdd"><AddNote v-model:notes="notes" :item="item" @confirmed="showAdd = false"></AddNote></Dialog>
</template>
<style scoped>
.notes-header{
    display: flex;
    justify-content: space-between;
}
h3{
    font-weight: bold;
    font-size: medium;
}
.container{
    padding: 10px;
    flex: 1;
    max-height: 400px;
    min-width: 280px;
}
.text-display{
    padding:2px;
    display: flex;
    flex-direction: column;
    align-items: flex-end;
}
p{
    font-size: small;
    width: 100%;
    max-width: 400px;
    max-height: 400px;
    overflow: auto;
}
*{
    font-size: small;
}

</style>