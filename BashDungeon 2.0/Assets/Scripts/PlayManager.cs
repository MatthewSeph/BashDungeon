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
        
        
        Vector3 positionToGo = new Vector3();
        if (roomDirection == Vector2.up)
        {
            playerGO.GetComponent<NavMeshAgent>().enabled = true;
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
            playerGO.GetComponent<NavMeshAgent>().enabled = true;
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
            playerGO.GetComponent<NavMeshAgent>().enabled = true;
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
            playerGO.GetComponent<NavMeshAgent>().enabled = true;
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
        Vector3 oldLocalPosition = playerGO.transform.position;

        oldLocalPosition.x = oldLocalPosition.x - (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.x * 24);
        oldLocalPosition.z = oldLocalPosition.z - (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.y * 24);

        oldLocalPosition.x = oldLocalPosition.x + (playerGO.GetComponent<PlayerMovement>().TarghetRoom.gridPos.x * 24);
        oldLocalPosition.z = oldLocalPosition.z + (playerGO.GetComponent<PlayerMovement>().TarghetRoom.gridPos.y * 24);

        playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
        playerGO.GetComponent<PlayerMovement>().WantToChangeRoom = false;
        yield return new WaitForSeconds(sec);

        playerGO.GetComponent<NavMeshAgent>().Warp(oldLocalPosition);
        playerGO.transform.parent = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().TarghetRoom.nomeStanza).transform;
        playerGO.GetComponent<NavMeshAgent>().enabled = true;
        playerGO.transform.position = oldLocalPosition;

        playerGO.GetComponent<PlayerMovement>().currentRoom = playerGO.GetComponent<PlayerMovement>().TarghetRoom;
        //playerGO.GetComponent<NavMeshAgent>().destination = playerGO.transform.position;
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = false;

        Camera.main.transform.parent = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().TarghetRoom.nomeStanza).transform;
        Camera.main.transform.localPosition = new Vector3(-2, 31.87f, 0);
        yield return new WaitForSeconds(1);
        playerGO.transform.GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

    }

    public Vector3 CenterOfVectors(List<Vector3> vectors)
    {
        Vector3 sum = Vector3.zero;
        if (vectors == null || vectors.Count == 0)
        {
            return sum;
        }

        foreach (Vector3 vec in vectors)
        {
            sum += vec;
        }
        return sum / vectors.Count;
    }

}
