using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour {

    GameObject playerGO;

    void Start()
    {
        playerGO = GameObject.Find("Player");
    }

    public Vector2 RoomDirection(Room currentRoom, Room roomToGo)
    {
        Vector2 resultVector = new Vector2();

        resultVector.x = roomToGo.gridPos.x - currentRoom.gridPos.x;
        resultVector.y = roomToGo.gridPos.y - currentRoom.gridPos.y;

        Debug.Log(resultVector.x + ", " + resultVector.y);
        return resultVector;
    }

    public void GoToDoor(Vector2 roomDirection)
    {
        if(roomDirection == Vector2.up)
        {
            playerGO.GetComponent<PlayerMovement>().TargetPosition = new Vector3(-2, 0.5f, 11);
        }
        else if (roomDirection == Vector2.down)
        {
            playerGO.GetComponent<PlayerMovement>().TargetPosition = new Vector3(-2, 0.5f, -11);
        }
        else if (roomDirection == Vector2.left)
        {
            playerGO.GetComponent<PlayerMovement>().TargetPosition = new Vector3(-13, 0.5f, 0);
        }
        else if (roomDirection == Vector2.right)
        {
            playerGO.GetComponent<PlayerMovement>().TargetPosition = new Vector3(9, 0.5f, 0);
        }
        else
        {
            playerGO.GetComponent<PlayerMovement>().TargetPosition = new Vector3(9, 0.5f, 0);
        }

    }

    public void MoveBeforeChangeRoom(Room roomToGo)
    {
        playerGO.GetComponent<PlayerMovement>().WantToChangeRoom = true;
        Vector2 roomDirection = new Vector2();
        playerGO.GetComponent<PlayerMovement>().TarghetRoom = roomToGo;
        roomDirection = RoomDirection(playerGO.GetComponent<PlayerMovement>().currentRoom, roomToGo);
        GoToDoor(roomDirection);

    }

    public void ChangeRoom(Room targhetRoom)
    {
        playerGO.transform.parent = GameObject.Find("/" + targhetRoom.nomeStanza).transform;
        GoToDoor(RoomDirection(targhetRoom, playerGO.GetComponent<PlayerMovement>().currentRoom));
        playerGO.GetComponent<PlayerMovement>().currentRoom = targhetRoom;
        Camera.main.transform.parent = GameObject.Find("/" + targhetRoom.nomeStanza).transform;
    }

}
