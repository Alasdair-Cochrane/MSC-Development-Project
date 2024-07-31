<script setup>
import { Item } from '@/Models/Item';

const emit = defineEmits(['clicked'])
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

const cardClikced = () => {
    if(props.clickable){
        emit('clicked', props.item)
    }
}
</script>
<template>
    <div :class="{ canClick : clickable }" @click="cardClikced" style="flex: 1;">
    <div class="container "  >
        <div id="thumbnail" v-if="item && item.imageUrl">
            <img :src=item?.imageUrl height="80px" width="100%" v-if="item && item.imageUrl">
        </div>
        <div id="item-info">
            <div class="info-labels">
                <span>S/n: </span>
                <span>Model: </span>
                <span>Category: </span>
            </div>
            <div>
                <span>{{item?.serialNumber ?? " "}}</span>
                <span>{{item?.model.modelNumber ?? " "}}</span>
                <span>{{item?.model.category ?? " "}}</span>
            </div>
            
        </div>
        <div id="btns" v-if="showButtons">
            <Button rounded icon="pi pi-search" @click="$emit('clicked', item)"></Button>
            <Button rounded severity="danger" icon="pi pi-trash" @click="$emit('delete')"></Button>
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
    border-radius: 10px;
    display: flex;
    padding: 3px;
    border: solid var(--p-primary-600) 2px;
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
}
Button{
    max-width: 30px;
    max-height: 30px;
}
.canClick :hover{
    background-color: var(--p-surface-100);
}
span{
    font-size: small;
}
.info-labels span{
    font-weight: 600;
}

</style>