using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
	public Vector2 gridPos;
	public int type; // 0 = default ; 1 = root ; 2 = stanza-livello ; 3 = stanza-loot
	public Room parentRoom;
    public int distance;
    public string nomeStanza;
	public List<Room> childrenRooms = new List<Room> ();
    public List<Oggetto> oggetti = new List<Oggetto> ();

	public bool doorTop = false, doorBot = false, doorLeft = false, doorRight = false;

    bool isLocked = false;

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

    public bool IsLocked
    {
        get
        {
            return isLocked;
        }

        set
        {
            isLocked = value;
        }
    }

    public Room GetParentRoom(){
		return this.parentRoom;
	}

	public void SetParentRoom(Room parentRoom) {
		this.parentRoom = parentRoom;
	}

}
