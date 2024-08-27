 export class Unit{
    constructor(id, name, room, building,address, parentId){
        this.id = id
        this.name = name,
        this.room = room,
        this.building = building,
        this.address = address
        this.parentId = parentId
        this.isPublic = false
    }
 }