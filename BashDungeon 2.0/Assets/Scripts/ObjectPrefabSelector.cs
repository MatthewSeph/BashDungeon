using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrefabSelector : MonoBehaviour {

    public GameObject cassa;

    public GameObject PickObjectPrefab(string nomeOggetto)
    {
        if (nomeOggetto.Contains("cassa"))
        {
            return cassa;
        }
        return null;
    }

}
