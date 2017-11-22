using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrefabSelector : MonoBehaviour {

    public GameObject cassa;
    public GameObject archivio;

    public GameObject PickObjectPrefab(string nomeOggetto)
    {
        if (nomeOggetto.Contains("cassa"))
        {
            return cassa;
        }

        if (nomeOggetto.EndsWith(".tar"))
        {
            return archivio;
        }
        return null;
    }

}
