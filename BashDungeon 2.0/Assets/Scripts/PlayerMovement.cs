using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

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
    void Start () {
        anim = transform.GetComponent<Animator>();
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        TargetPosition = transform.position;
        gameManager = GameObject.Find("GameManager");
        m_Agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(m_Agent.destination, m_Agent.transform.position) <= m_Agent.stoppingDistance)
        {
            if (!m_Agent.hasPath || m_Agent.velocity.sqrMagnitude == 0f)
            {
                anim.SetFloat("MoveSpeed", 0f);

            }

        }
        else if ((transform.position.x != TargetPosition.x) || (transform.position.z != TargetPosition.z))
        {
            anim.SetFloat("MoveSpeed", 0.5f);
        }

        if ((transform.position.x == TargetPosition.x) && (transform.position.z == TargetPosition.z) && WantToChangeRoom )
        {
            m_Agent.enabled = false;
            gameManager.GetComponent<PlayManager>().ChangeRoom(targhetRoom);
            WantToChangeRoom = false;
        }

        if (!WantToChangeRoom && !blockedMovement)
        {
            if (Input.GetMouseButtonDown(0) && (!EventSystem.current.IsPointerOverGameObject()) && (Camera.main.ScreenToViewportPoint(Input.mousePosition).x > 0.1f) 
                && (Camera.main.ScreenToViewportPoint(Input.mousePosition).x < 0.9f) && (Camera.main.ScreenToViewportPoint(Input.mousePosition).y > 0.1f) 
                && (Camera.main.ScreenToViewportPoint(Input.mousePosition).y < 0.9f))
            {
                Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition).x + " " + Camera.main.ScreenToViewportPoint(Input.mousePosition).y + " " + Camera.main.ScreenToViewportPoint(Input.mousePosition).z);

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.transform.position.y;
                Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(mousePos);

                    GetComponent<NavMeshAgent>().destination = clickedPosition;

            }
        }
    }
}