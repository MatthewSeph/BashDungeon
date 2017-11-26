using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrefabSelector : MonoBehaviour {

    public GameObject cassa;
    public GameObject archivio;
    public GameObject pergamena;

    public GameObject PickObjectPrefab(string nomeOggetto)
    {

        if (nomeOggetto.EndsWith(".tar") || nomeOggetto.EndsWith(".tar.gz"))
        {
            return archivio;
        }

        if (nomeOggetto.Contains("cassa"))
        {
            return cassa;
        }

        if (nomeOggetto.Contains("pergamena"))
        {
            return pergamena;
        }

       
        return null;
    }

}
