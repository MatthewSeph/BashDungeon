using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        playerGO.GetComponent<NavMeshAgent>().enabled = true;
        
        Vector3 positionToGo = new Vector3();
        if (roomDirection == Vector2.up)
        {
            positionToGo.x = -2 + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.x * 24);
            Debug.Log(positionToGo.x);
            positionToGo.z = 10.5f + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.y * 24);
            Debug.Log(positionToGo.z);
            positionToGo.y = 0.5f;
            playerGO.GetComponent<NavMeshAgent>().destination = positionToGo;
            playerGO.GetComponent<PlayerMovement>().TargetPosition = positionToGo;
        }
        else if (roomDirection == Vector2.down)
        {
            positionToGo.x = -2 + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.x * 24);
            Debug.Log(positionToGo.x);
            positionToGo.z = -10.5f + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.y * 24);
            Debug.Log(positionToGo.z);
            positionToGo.y = 0.5f;
            playerGO.GetComponent<NavMeshAgent>().destination = positionToGo;
            playerGO.GetComponent<PlayerMovement>().TargetPosition = positionToGo;
        }
        else if (roomDirection == Vector2.left)
        {
            positionToGo.x = -12.5f + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.x * 24);
            Debug.Log(positionToGo.x);
            positionToGo.z = 0 + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.y * 24);
            Debug.Log(positionToGo.z);
            positionToGo.y = 0.5f;
            playerGO.GetComponent<NavMeshAgent>().destination = positionToGo;
            playerGO.GetComponent<PlayerMovement>().TargetPosition = positionToGo;
        }
        else if (roomDirection == Vector2.right)
        {
            positionToGo.x = 8.5f + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.x * 24);
            Debug.Log(positionToGo.x);
            positionToGo.z = 0 + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.y * 24);
            Debug.Log(positionToGo.z);
            positionToGo.y = 0.5f;
            playerGO.GetComponent<NavMeshAgent>().destination = positionToGo;
            playerGO.GetComponent<PlayerMovement>().TargetPosition = positionToGo;
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
        Room oldRoom = playerGO.GetComponent<PlayerMovement>().currentRoom;
        playerGO.transform.parent = GameObject.Find("/" + targhetRoom.nomeStanza).transform;
        playerGO.GetComponent<PlayerMovement>().currentRoom = targhetRoom;
        GoToDoor(RoomDirection(playerGO.GetComponent<PlayerMovement>().currentRoom, oldRoom));
        
        Camera.main.transform.parent = GameObject.Find("/" + targhetRoom.nomeStanza).transform;
    }

    IEnumerator ChangeRoomWithCooldown(int sec)
    {
        Vector3 oldLocalPosition = playerGO.transform.localPosition;
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
        playerGO.GetComponent<PlayerMovement>().WantToChangeRoom = false;
        yield return new WaitForSeconds(sec);
        Debug.Log(":)");
        playerGO.transform.parent = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().TarghetRoom.nomeStanza).transform;

        playerGO.transform.localPosition = oldLocalPosition;
        playerGO.GetComponent<PlayerMovement>().TargetPosition = oldLocalPosition;
        playerGO.GetComponent<PlayerMovement>().currentRoom = playerGO.GetComponent<PlayerMovement>().TarghetRoom;
        playerGO.GetComponent<NavMeshAgent>().destination = playerGO.transform.position;
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = false;
        Debug.Log("SONONELMEZZO");
        Camera.main.transform.parent = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().TarghetRoom.nomeStanza).transform;
        Camera.main.transform.localPosition = new Vector3(-2, 31.87f, 0);
        yield return new WaitForSeconds(1);
        playerGO.transform.GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        Debug.Log("FINE");
    }
}
