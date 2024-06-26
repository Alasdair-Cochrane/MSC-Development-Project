<script>
    import AddNew from '@/Services/EquipmentModelService';
    import {reactive, ref} from 'vue'
    import { EquipmentModel } from '@/Models/EquipmentModel';

    export default {
        setup(){
        const model = reactive(new EquipmentModel())
        var post = ref("model")
        
        
        async function add(){
            post.value = await AddNew(model);
            }
          
        return {
            model,
            add,
            post
        }
}
};   
</script>

<template>
    <form>
        <input v-model="model.model_Name" placeholder="Model Name"/>
        <input v-model="model.model_Number" placeholder="Model Number"/>
        <input v-model="model.description" placeholder="Description"/>
        <input v-model="model.manufacturer" placeholder="Manufacturer"/>
        <input v-model="model.category" placeholder="Category"/>
        <input v-model="model.weight" placeholder="Weight"/>
        <input v-model="model.height" placeholder="Height (kg)"/>
        <input v-model="model.length" placeholder="Length"/>
        <input v-model="model.depth" placeholder="Depth"/>
    </form>

    <p>{{post}}</p>
   

    <button type="submit" @click="add">Submit</button>
</template>