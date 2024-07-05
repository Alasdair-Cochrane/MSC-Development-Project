<script setup>
import FileDisplay from '@/components/FileDisplay.vue';
import ItemDetail from '@/components/ItemDetail.vue';
import ItemSearch from '@/components/ItemSearch.vue'
import {ref} from 'vue'

const editMode = ref(false)
function toggleExtra() {editMode.value = !editMode.value
    console.log("emit recieved")
}

</script>
<template>
<div class="page">
    <div class="search">
        <ItemSearch></ItemSearch>
    </div>
    <div class="display">
        <ItemDetail @editTrue="toggleExtra"></ItemDetail>
    </div>
    <div v-if="!editMode" class="extra">
        <FileDisplay title="Maintenance Records"></FileDisplay>
    </div>
</div>
</template>

<style scoped>
div{
    /* border: black 2px solid; */
}
.page{
    display: flex;
    flex-wrap: wrap;
    flex: 1;
    padding: 1rem;
}
.search{


}
.display{
    flex: 1;

}
.extra{
    max-width: 20%;
    min-width: 300px;


}

</style>