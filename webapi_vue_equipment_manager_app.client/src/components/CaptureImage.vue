<script setup>
//https://developer.mozilla.org/en-US/docs/Web/API/Media_Capture_and_Streams_API/Taking_still_photos
//https://stackoverflow.com/questions/11642926/stop-close-webcam-stream-which-is-opened-by-navigator-mediadevices-getusermedia
//https://stackoverflow.com/questions/51755989/how-i-display-as-image-a-blob-object-with-js
import {ref, onMounted} from 'vue'
const width = 320
var height = 320

const video = ref(null);
const canvas = ref(null)
const imageElement = ref(null)
var streaming = false;

const emits = defineEmits(['cancelled', 'imageConfirmed'])


const imageCaptured = ref(false)
var capturedImage = ref(null)

function imageSelected(event){

  const data = event.target.files[0]
  capturedImage.value = data

  const imageURL = URL.createObjectURL(data)
  imageElement.value.src = imageURL;

  window.localStream.getTracks().forEach((track) => track.stop())
  imageCaptured.value = true;
}

function initCamera(){
  navigator.mediaDevices.
    getUserMedia({video:true, audio:false}).
    then((stream) => {
        window.localStream = stream
        video.value.srcObject = stream;
        video.value.play()}).
        catch((err) => console.log(err));
}

onMounted( () =>{
    initCamera()
    video.value.addEventListener(
        "canplay",
     (ev) => {
    if (!streaming) {
      height = (video.value.videoHeight / video.value.videoWidth) * width;

      video.value.setAttribute("width", width);
      video.value.setAttribute("height", height);
      canvas.value.setAttribute("width", width);
      canvas.value.setAttribute("height", height);
      streaming = true;
    }
  },
  false,
    )
    clearphoto();
    }
)

function takePhoto() {
  streaming = false;
  const context = canvas.value.getContext("2d");
  if (width) {

    canvas.value.width = width;
    canvas.value.height = height;

    context.drawImage(video.value, 0, 0, width, height);
    const data = canvas.value.toDataURL("image/png");
  
    //set value of file to be returned to the parent 
    canvas.value.toBlob((blob) => {capturedImage.value = blob}, "image/png" )
    //set value of image element within component
    imageElement.value.setAttribute("src", data);
    //close the video stream
    window.localStream.getTracks().forEach((track) => track.stop())
    imageCaptured.value = true;
  } else {
    console.log("width and height are not set")
    clearphoto();
  }
}

function clearphoto() {
  const context = canvas.value.getContext("2d");
  context.fillStyle = "#AAA";
  context.fillRect(0, 0, canvas.value.width, canvas.value.height);

  const data = canvas.value.toDataURL("image/png");
  imageElement.value.setAttribute("src", data);
}
function cancel(){
    streaming = false;
    window.localStream.getTracks().forEach(track => track.stop())
    emits('cancelled')
}

function reTake(){
  clearphoto()
  imageCaptured.value = false;
  capturedImage.value = null;
  initCamera();
}
function submit(){
  emits('imageConfirmed', capturedImage.value)
}

</script>
<template>
<div class="container">
    <div class="viewport">
        <video v-show="!imageCaptured" ref="video"></video>
        <img  v-show="imageCaptured" ref="imageElement" width="320" height="240">
        <canvas ref="canvas" style="display:none;"></canvas>
        <div class="bttn-group">
          <Button v-if="!imageCaptured" id="btn-capture" @click="takePhoto" label="Capture"></Button>
          <input type="file" accept="image/*" @change="imageSelected"/>
          <Button v-if="!imageCaptured" id="btn-cancel" @click="cancel" label="Cancel" severity="danger"></Button>
        </div>
        <div class="bttn-group">
          <Button v-if="imageCaptured" id="btn-retake" @click="reTake" label="Re-Take"></Button>
          <Button v-if="imageCaptured" id="btn-submit" @click="submit" label="Submit"></Button>
        </div>

    </div>



</div>
</template>
<style scoped>

.bttn-group{
  display:  flex;
  justify-content: space-between;
}

img{
  max-width: 320;
  max-height: 320;
}
</style>