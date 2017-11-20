using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour {

    public bool isMadeVisible;
    bool isVisible;
    Color myColorAlphaZero;
    Color myColorFullAlpha;

    void Start()
    {
        myColorAlphaZero = transform.GetComponent<MeshRenderer>().material.color;
        myColorAlphaZero.a = 0;
        myColorFullAlpha = transform.GetComponent<MeshRenderer>().material.color;
        myColorFullAlpha.a = 100;
    }

    private void Update()
    {
        if (!isVisible)
        {
            if (transform.name.Contains(".") && !isMadeVisible)
            {

                transform.GetComponent<MeshRenderer>().material.color = myColorAlphaZero;
            }
            else if (transform.name.Contains(".") && isMadeVisible)
            {
                transform.GetComponent<MeshRenderer>().material.color = myColorFullAlpha;
            }

            if (transform.GetComponent<MeshRenderer>().material.color.a == 100)
            {
                isVisible = true;
            }
        }
    }

}
