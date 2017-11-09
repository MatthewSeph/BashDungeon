using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Vector3 targetPosition;
    float speed = 10f;
    public Room currentRoom;


    // Use this for initialization
    void Start () {

        targetPosition = transform.position;
    } 
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.y;
            Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(mousePos);

            if ((clickedPosition.x < 9) && (clickedPosition.x > -13) && (clickedPosition.z > -11) && (clickedPosition.z < 11))
            {
                targetPosition = clickedPosition;
                targetPosition.y = 0.5f;
                Debug.Log(targetPosition.x + " " + targetPosition.y + " " + targetPosition.z);
                Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z);
                Debug.Log(Input.mousePosition.x + " " + Input.mousePosition.y + " " + Input.mousePosition.z);
            }


        }

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, Time.deltaTime * speed);
    }
}