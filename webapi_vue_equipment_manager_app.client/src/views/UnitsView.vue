<script setup>
import OrgChart from '@/components/OrgChart.vue'
import OrgTable from '@/components/OrgTable.vue'

</script>
<template>
<div class="wrapper">
        <div class="org" id="org-table"><OrgTable></OrgTable></div>
        <div class="org" id="org-chart"><OrgChart></OrgChart></div>

</div>
</template>
<style scoped>
.wrapper{
    display: flex;
    flex: 1;
    border: 1px solid black;
    height: 100%;
    padding: 1rem;
    flex-wrap: wrap;
    gap: 2rem;
}

.org{
    box-shadow: 0 4px 8px 0 rgba(57, 40, 40, 0.2);
    height: fit-content;
    padding: 2rem;
    border-radius: 5px;
}
#org-table{
    min-height: 50%;
}
</style>