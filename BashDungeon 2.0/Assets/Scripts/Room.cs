using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
	public Vector2 gridPos;
	public int type;
	public Room parentRoom;

	public bool doorTop = false, doorBot = false, doorLeft = false, doorRight = false;

	public Room(Vector2 _gridPos, int _type){
		gridPos = _gridPos;
		type = _type;
	}

	public Room GetParentRoom(){
		return this.parentRoom;
	}

	public void SetParentRoom(Room parentRoom) {
		this.parentRoom = parentRoom;
	}
}
