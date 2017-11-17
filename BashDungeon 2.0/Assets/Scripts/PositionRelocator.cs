using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRelocator : MonoBehaviour {
    /*
    private bool hasHitSomething;

    public bool HasHitSomething
    {
        get
        {
            return hasHitSomething;
        }

        set
        {
            hasHitSomething = value;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        HasHitSomething = true;
    }

    void RandomRelocate()
    {
        Vector3 newLocalRandomPosition = new Vector3();
        newLocalRandomPosition.x = Random.Range(-10.5f, 7f);
        newLocalRandomPosition.z = Random.Range(-9, 9);
        newLocalRandomPosition.y = transform.localPosition.y;

    }

    */

    void Update()
    {
        if(Physics.CheckBox(this.transform.position, this.GetComponent<Renderer>().bounds.extents, transform.rotation, 9)) // Dove 9 è il layer per gli oggetti che devono collidere tra loro
        {
            Debug.Log("entrato in collisione.");
        }
    }



}
