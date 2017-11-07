using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
	public Vector2 gridPos;
	public int type;
	public Room parentRoom;
    public int distance;
    public string nomeStanza;
	public List<Room> childrenRooms = new List<Room> ();

	public bool doorTop = false, doorBot = false, doorLeft = false, doorRight = false;

	public Room(Vector2 _gridPos, int _type){
		gridPos = _gridPos;
		type = _type;
        
        if (type == 1)
        {
            nomeStanza = "/";
        }
        else
        {
            nomeStanza = GameObject.Find("GameManager").GetComponent<RandomNamesGenerator>().GenerateName();
        }
	}

	public Room GetParentRoom(){
		return this.parentRoom;
	}

	public void SetParentRoom(Room parentRoom) {
		this.parentRoom = parentRoom;
	}


}
