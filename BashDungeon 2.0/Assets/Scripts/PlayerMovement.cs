using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Vector3 targetPosition;
    float speed = 10f;


    // Use this for initialization
    void Start () {

        targetPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && !(hit.transform.tag=="BlockMovement"))
            {
                targetPosition = hit.point;
                targetPosition.y = 0.5f;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
    }
}
