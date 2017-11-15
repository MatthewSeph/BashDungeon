using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

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
            if (Input.GetMouseButtonDown(0) && (!EventSystem.current.IsPointerOverGameObject()))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.transform.position.y;
                Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(mousePos);
                Debug.Log(clickedPosition.x + " " + clickedPosition.y + " " + " " + clickedPosition.z);

                if ((clickedPosition.x < 9) && (clickedPosition.x > -13) && (clickedPosition.z > -11) && (clickedPosition.z < 11))
                {
                    
                    TargetPosition = clickedPosition;
                    targetPosition.y = 0.5f;
                    
                }


            }
        }

        if (!BlockedMovement)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetPosition, Time.deltaTime * speed);
        }
    }
}