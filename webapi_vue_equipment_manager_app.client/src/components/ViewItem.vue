<script>
    import { ref, onMounted } from 'vue'
    import {GetItem } from '@/Services/ItemService'
    import { Item } from '@/Models/Item';

    export default {
        setup() {
            var item = ref(new Item())
            var loaded = ref(false)
            const text = ref("")

            onMounted(async () => {
                item.value = await GetItem(13);
                console.log(JSON.stringify(item.value));
                loaded.value = true;
            })

            async function Load() {
                item.value = await GetItem(12);
                console.log(JSON.stringify(item.value));
                text.value = JSON.stringify(item.value);
            }
            return {
                item,
                loaded,
                Load,
                text,
            }

        }
    }

</script>
<template>


</template>