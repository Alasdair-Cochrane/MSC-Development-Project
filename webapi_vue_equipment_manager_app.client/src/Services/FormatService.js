export function FormatDate(date){
    if(date == null || date == undefined){return null}
   return new Date(date).toLocaleDateString('en-GB')
}