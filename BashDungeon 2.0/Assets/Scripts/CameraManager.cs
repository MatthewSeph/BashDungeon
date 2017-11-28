using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.localPosition = new Vector3(-2, 31.87f, 0.06f);

    }
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(-2, 31.87f, 0), Time.deltaTime * 20);
    }
}
