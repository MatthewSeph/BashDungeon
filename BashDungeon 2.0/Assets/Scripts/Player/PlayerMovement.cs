using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{

    NavMeshAgent m_Agent;
    Vector3 targetPosition;
    public Room currentRoom;
    Room targhetRoom;
    bool wantToChangeRoom = false;
    GameObject gameManager;
    bool blockedMovement = false;
    Animator anim;


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
    void Start()
    {

        anim = transform.GetComponent<Animator>();
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        TargetPosition = transform.position;
        gameManager = GameObject.Find("GameManager");
        m_Agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if the Player should move
        if (Vector3.Distance(m_Agent.destination, m_Agent.transform.position) <= m_Agent.stoppingDistance && m_Agent.velocity.sqrMagnitude <= 0.1f && !m_Agent.pathPending && !m_Agent.hasPath)
        {
            anim.SetFloat("MoveSpeed", 0f);
            m_Agent.ResetPath();
            //TargetPosition = transform.position;
        }
        else if ((transform.position.x != m_Agent.destination.x) || (transform.position.z != m_Agent.destination.z) && !m_Agent.pathPending && m_Agent.hasPath) //Can move and is moving
        {
            anim.SetFloat("MoveSpeed", 0.5f);

        }


        //The player is in front of a door and want to change room
        if ((Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(TargetPosition.x, TargetPosition.z)) < 0.01 ) && WantToChangeRoom)
        {
            //m_Agent.enabled = false;
            gameManager.GetComponent<PlayManager>().ChangeRoom(targhetRoom);
            WantToChangeRoom = false;
        }

        //Can move inside a room
        if (!WantToChangeRoom && !blockedMovement)
        {
            if (Input.GetMouseButtonDown(0) && (!EventSystem.current.IsPointerOverGameObject()) && (Camera.main.ScreenToViewportPoint(Input.mousePosition).x > 0.1f) //Are my clicks on the Left Camera and not on a UI Element?
                && (Camera.main.ScreenToViewportPoint(Input.mousePosition).x < 0.9f) && (Camera.main.ScreenToViewportPoint(Input.mousePosition).y > 0.1f)
                && (Camera.main.ScreenToViewportPoint(Input.mousePosition).y < 0.9f))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.transform.position.y;
                Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(mousePos);

                

                if (!gameManager.GetComponent<PlayManager>().IsMouseOverObj)
                {
                    gameManager.GetComponent<PlayManager>().ClickedObject = null;
                    m_Agent.destination = clickedPosition;
                }
            }
        }
    }
}