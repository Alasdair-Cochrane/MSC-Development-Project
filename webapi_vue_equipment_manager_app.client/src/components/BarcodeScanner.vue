
<script setup>
//https://scanapp.org/html5-qrcode-docs/docs/intro

import {Html5Qrcode, Html5QrcodeScanner} from "html5-qrcode";
import {onMounted, ref} from 'vue'

const scanValue = ref("")
const scanResult = ref("")
const scanner = ref()
const config = { fps: 10, qrbox: {width: 200, height: 200} }

function onScanSuccess(decodedText, decodedResult) {
    scanValue.value = decodedText
    scanResult.value = decodedResult
    confirm()
}


function onScanFailure(error) {

  console.warn(`Code scan error = ${error}`);
}

onMounted(() =>
  {
        let html5Qrcode= new Html5Qrcode(
      "reader")
      html5Qrcode.start({facingMode:"environment"}, config, onScanSuccess,onScanFailure)
      
    scanner.value = html5Qrcode;
  })

const emit = defineEmits(['confirmed', 'cancelled'])


const close = () =>{
  scanner.value.stop().catch((err) => console.warn(err.message)) 
  emit('cancelled')
}

const confirm = () => {
  scanner.value.stop().catch((err) => console.warn(err.message)) 
  emit('confirmed', scanValue.value)

}
</script>

<template>
<div id="reader"></div>
<div class="scan-success" v-if="scanValue"><label>{{scanValue}}</label></div>
<div class="btns">
    <Button label="Cancel" severity="danger" @click="close()"></Button>
    </div>  
</template>
<style>
#reader{
    width: 300px;
}
.scan-success{
  border: solid black 1px;
  display: flex;
  justify-content: center;
  background-color: lightgreen;
  padding: 1rem;
}
.scan-success label{
  font-weight: bold;
  font-size: large;
}
.btns{
  display: flex;
  justify-content: space-between;
  padding:10px
}
</style>