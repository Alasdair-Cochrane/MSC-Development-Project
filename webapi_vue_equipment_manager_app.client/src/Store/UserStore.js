import {reactive} from 'vue'

export const store = reactive({
    Units : ["Admin",],
    async UpdateUnits(){
        const response = await fetch('/api/units')
        this.Units = response.json();
    }
})

