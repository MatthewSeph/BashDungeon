using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour {

    public bool isMadeVisible;
    bool isBeingCompressed;
    bool isVisible;
    Color myColorAlphaZero;
    Color myColorFullAlpha;
    public List<GameObject> oggettiArchiviati;

    void Start()
    {
        myColorAlphaZero = transform.GetComponent<MeshRenderer>().material.color;
        myColorAlphaZero.a = 0;
        myColorFullAlpha = transform.GetComponent<MeshRenderer>().material.color;
        myColorFullAlpha.a = 75;
        
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

            if (transform.GetComponent<MeshRenderer>().material.color.a == 75)
            {
                isVisible = true;
            }
        }

        if(this.gameObject.name.EndsWith(".tar.gz") && !isBeingCompressed)
        {
            Vector3 scaledVector = this.gameObject.transform.localScale;
            scaledVector.x /= 2;
            scaledVector.z /= 2;
            this.gameObject.transform.localScale = scaledVector;

            isBeingCompressed = true;
        }
    }

    public void SettaOff()
    {
        this.enabled = false;
    }



}
