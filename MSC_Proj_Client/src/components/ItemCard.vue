<script setup>
import { DeleteItem } from '@/Services/ItemService';
import { onMounted, ref } from 'vue';

const deleteLoading = ref(false)
const emit = defineEmits(['clicked', 'deleted'])
const shownItem = ref()
const showDetails = ref(false)

onMounted(() => shownItem.value = props.item)

const props = defineProps({
    showButtons:{
        type : Boolean,
        default : true,
    },
    item:{
    },
    clickable: {
        type : Boolean,
        default : false,
    }
})

const cardClicked = () => {
    if(props.clickable){
        emit('clicked', props.item)
    }
}

const deleteItem = async () =>{
    deleteLoading.value = true;
    let result = await DeleteItem(props.item.id)
    if(result.successfull){
        emit('deleted', props.item)
    }
    deleteLoading.value = false
}
</script>
<template>
    <div class="container "  >
        <div :class="{ canClick : clickable }" @click="cardClicked" style="flex: 1; display: flex;">
        <div id="thumbnail" v-if="shownItem && shownItem.imageUrl">
            <img :src=shownItem?.imageUrl height="80px" width="100%" v-if="shownItem && shownItem.imageUrl">
        </div>
        <div id="item-info">
            <div class="info-labels">
                <span>S/n: </span>
                <span>Model: </span>
                <span>Category: </span>
            </div>
            <div>
                <span>{{shownItem?.serialNumber ?? " "}}</span>
                <span>{{shownItem?.model.modelNumber ?? " "}}</span>
                <span>{{shownItem?.model.category ?? " "}}</span>
            </div>
            
        </div>
        <div id="btns" v-if="showButtons">
            <Button rounded icon="pi pi-search" @click="showDetails= true"></Button>
            <Button rounded severity="danger" icon="pi pi-trash" @click="deleteItem()"></Button>
            <Dialog v-model:visible="showDetails"><ItemDetail :selectedItem="shownItem"></ItemDetail></Dialog>
        </div>
    </div>

</div>
</template>
<style scoped>
.container{

    flex: 1;
    height: 100%;
    min-height: 80px;
    max-width: 350px;
    min-width: 250px;
    border-radius: 10px;
    display: flex;
    padding: 3px;
    border: solid black 1px;
    box-shadow:  0 2px 2px 0 rgba(28, 25, 25, 0.2);
    overflow: hidden;
    background-color: var(--p-surface-0);

}
img{
    border-radius: 10px;
}
#thumbnail{
    display: flex;
    align-items: center;
    margin: 5px;
    margin: 5px;
    border-radius: 10px;    
    min-width:80px;
    max-width: 80px;
}
#item-info{
    flex: 1;
    display: flex;
    justify-content: space-evenly;

}
#item-info div{
    display: flex;
    flex-direction: column;
    gap: 5px;
}

#btns{
    display: flex;
    flex-direction: column;
    justify-content: space-evenly;
    justify-self: flex-end;
    max-width: fit-content;
}
Button{
    max-width: 30px;
    max-height: 30px;
}
.canClick :hover{
    background-color: var(--p-surface-200);
}
.canClick{
    display: flex;
    
}
span{
    font-size: small;
}
.info-labels span{
    font-weight: 600;
}

</style>