
<script setup>
//https://scanapp.org/html5-qrcode-docs/docs/intro

import {Html5QrcodeScanner} from "html5-qrcode";
import {onMounted, ref} from 'vue'

function onScanSuccess(decodedText, decodedResult) {
    console.log(`Code matched = ${decodedText}`, decodedResult);
    scanValue.value = decodedText
}

const scanValue = ref("")

function onScanFailure(error) {

  console.warn(`Code scan error = ${error}`);
}

onMounted(() =>
{
    let html5QrcodeScanner = new Html5QrcodeScanner(
  "reader",
  { fps: 10, qrbox: {width: 250, height: 250} },
  /* verbose= */ false);
html5QrcodeScanner.render(onScanSuccess, onScanFailure);
})

const emit = defineEmits(['scanConfirmed'])



</script>

<template>
<div id="reader"></div>
<label>{{scanValue}}</label>
<div class="btns">
    <Button label="Confirm" @click="confirmed"></Button>
    <Button label="Cancel"></Button>
    </div>  
</template>
<style>
#reader{
    width: 600px;
}
</style>