<script setup>
import {ref} from 'vue';

const email = ref(null)


</script>
<template>
<div class="wrapper">
    <div id="login" class="inputForm">
        <h2>Reset Password</h2>
        <FloatLabel>
            <InputText id="email" v-model="email" :invalid="accessFailed"/>
            <label for="email">Email</label>
        </FloatLabel>
        <Button label="Login"></Button>
        <div class="bottom-links">
            <Button label="Back to Login" icon="pi pi-arrow-circle-left" severity="secondary" @click="$emit('login')"></Button>

        </div>
    </div>
    
</div>
</template>
<style scoped>


.wrapper{
    display: flex;
    flex: 1;
    justify-content: center;
    align-items: center;
    height: 100%;
}

.inputForm{
    display: flex;
    flex-direction: column;
    padding: 1rem;
    gap: 2rem;
    padding-block: 1rem;
    align-items: center;
    flex: 1;
}

.bottom-links{
    display: flex;
    justify-content: space-between;
    flex: 1;
    width: 100%;
}
.bottom-links Button{
    font-size: small;
    min-width: 120px;
}

#email{
    min-width: 234px;
}
</style>