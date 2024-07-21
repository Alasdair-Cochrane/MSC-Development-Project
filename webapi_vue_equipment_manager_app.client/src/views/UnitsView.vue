<script setup>
import OrgChart from '@/components/OrgChart.vue'

</script>
<template>
<div class="wrapper">
    <div id="org"><OrgChart></OrgChart></div>
</div>
</template>
<style scoped>
.wrapper{
    display: flex;
    flex: 1;
    border: 1px solid black;
    height: 100%;
    padding: 2rem;
}

#org{
    box-shadow: 0 4px 8px 0 rgba(57, 40, 40, 0.2);
    height: fit-content;
    padding: 2rem;
    border-radius: 5px;
}
</style>