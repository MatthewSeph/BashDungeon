using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovableObject : MonoBehaviour {

    GameObject gameManager;
    Room lootRoom;
    Room myRoom;

    // Use this for initialization
    void Start () {
		gameManager = GameObject.Find("GameManager");

        myRoom = gameManager.GetComponent<LevelGeneration>().GetRoomByName(gameObject.transform.parent.name);
            
        foreach(Room childRoom in myRoom.childrenRooms)
        {
            if(childRoom.type == 3)
            {
                lootRoom = childRoom;
                break;
            }
        }
        
        lootRoom.IsLocked = true;
	}

    private void Update()
    {
        lootRoom.IsLocked = true;
    }

    private void OnDestroy()
    {
        lootRoom.IsLocked = false;
    }
}
