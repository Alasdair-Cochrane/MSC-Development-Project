<script setup>
import ItemSearch from '@/components/ItemSearch.vue'
</script>
<template>
<div class="grid-nogutter page">
    <div class="col-12 md:col-2">
        <ItemSearch></ItemSearch>
    </div>
    <div class="col-12 md:col-4"></div>
    <div class="col-12 md:col-3"></div>
    <div class="col-12 md:col-3"></div>
</div>
</template>
<style scoped>
div{
    border: black 2px solid;
}
.page{
    display: flex;
    flex: 1;
    height: 100%;
}
</style>