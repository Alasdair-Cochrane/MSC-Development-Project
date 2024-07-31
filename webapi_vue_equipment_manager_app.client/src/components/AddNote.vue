<script setup>
import { AddNote } from '@/Services/NotesService';
import { onMounted, ref } from 'vue';

const notes = defineModel('notes')
const props = defineProps({item:{}})
const newNote = ref({})
const emit = defineEmits(["added"])
const loading = ref(false)

onMounted(()=> newNote.value.itemId = props.item.id)

const validateNote = ()=> {
    if(!newNote.value.title){
        return false
    }
    if(!newNote.value.text){
        return false
    }
    return true
}
const addNote = async() =>{
    loading.value = true
    if(!validateNote()){
        console.warn("Required Note fields are empty")
    }
    let result = await AddNote(newNote.value)
    if(result.successfull){
        notes.value.push(result.payload)
        console.log(result)
        emit("added", newNote.value)
    }
    loading.value = false;
}
</script>
<template>
<div class="wrapper">
    <h3>S/N : {{ item.serialNumber }}</h3>
    <div>
        <div class="field">
            <label for="noteTitle">Title</label>
            <InputText id="noteTitle" v-model="newNote.title"></InputText>
        </div>
        <div class="field">
            <label for="noteText">Note</label>
            <Textarea :rows="5" :cols="30" v-model="newNote.text"></Textarea>
            <small v-if="newNote.text">{{ newNote.text.length }}/1000 characters</small>
        </div>
    </div>
    <span style="display: flex; justify-content: center;">
        <Button label="Submit" icon="pi pi-save" style="max-width: 250px;" :loading="loading" @click="addNote"
        :disabled="!newNote.text && !newNote.title" ></Button>     
    </span>
</div>
</template>
<style scoped>
.wrapper{
    display: flex;
    flex-direction: column;
}
.field{
    display: flex;
    flex-direction: column;
}
.field label{
    font-weight: bold;
}
h3{
    font-weight: bold;
    margin-bottom: 2rem;
}
</style>