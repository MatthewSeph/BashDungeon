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
            
            playerGO.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            StartCoroutine(ChangeRoomWithCooldown(3));
            

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

    IEnumerator ChangeRoomWithCooldown(int sec)
    {
        Vector3 oldLocalPosition = playerGO.transform.localPosition;
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
        playerGO.GetComponent<PlayerMovement>().WantToChangeRoom = false;
        yield return new WaitForSeconds(sec);

        playerGO.transform.parent = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().TarghetRoom.nomeStanza).transform;

        playerGO.transform.localPosition = oldLocalPosition;
        playerGO.GetComponent<PlayerMovement>().TargetPosition = oldLocalPosition;
        playerGO.GetComponent<PlayerMovement>().currentRoom = playerGO.GetComponent<PlayerMovement>().TarghetRoom;
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = false;
        Camera.main.transform.parent = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().TarghetRoom.nomeStanza).transform;
        Camera.main.transform.localPosition = new Vector3(-2, 31.87f, 0);
        yield return new WaitForSeconds(1);
        playerGO.transform.GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        
    }
}
