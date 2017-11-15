using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

    NavMeshAgent m_Agent;
    Vector3 targetPosition;
    float speed = 10f;
    public Room currentRoom;
    Room targhetRoom;
    bool wantToChangeRoom = false;
    GameObject gameManager;
    bool blockedMovement = false;
 

    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }

        set
        {
            targetPosition = value;
        }
    }

    public bool WantToChangeRoom
    {
        get
        {
            return wantToChangeRoom;
        }

        set
        {
            wantToChangeRoom = value;
        }
    }

    public Room TarghetRoom
    {
        get
        {
            return targhetRoom;
        }

        set
        {
            targhetRoom = value;
        }
    }

    public bool BlockedMovement
    {
        get
        {
            return blockedMovement;
        }

        set
        {
            blockedMovement = value;
        }
    }


    // Use this for initialization
    void Start () {

        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        TargetPosition = transform.position;
        gameManager = GameObject.Find("GameManager");
        m_Agent = GetComponent<NavMeshAgent>();

    } 
	
	// Update is called once per frame
	void Update () {
        if((transform.localPosition == TargetPosition) && WantToChangeRoom && targhetRoom!=null && targhetRoom!=currentRoom)
        {
            
            gameManager.GetComponent<PlayManager>().ChangeRoom(targhetRoom);
            WantToChangeRoom = false;
        }

        if (!WantToChangeRoom)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    m_Agent.destination = hit.point;
                }
            }
        }

        if (!BlockedMovement)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetPosition, Time.deltaTime * speed);
        }
    }
}