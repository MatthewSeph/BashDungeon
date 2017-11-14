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


    // Use this for initialization
    void Start () {

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

                if ((clickedPosition.x % 24 < 9) && (clickedPosition.x % 24 > -13) && (clickedPosition.z % 24 > -11) && (clickedPosition.z % 24 < 11))
                {
                    TargetPosition = clickedPosition;
                    targetPosition.y = 0.5f;
                    Debug.Log(TargetPosition.x + " " + TargetPosition.y + " " + TargetPosition.z);
                    Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z);
                    Debug.Log(Input.mousePosition.x + " " + Input.mousePosition.y + " " + Input.mousePosition.z);
                }


            }
        }


        transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetPosition, Time.deltaTime * speed);
        
    }
}