using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrefabSelector : MonoBehaviour {

    public GameObject cassa;
    public GameObject archivio;
    public GameObject pergamena;
    public GameObject npc;
    public GameObject tappeto;
    public GameObject barile;
    public GameObject tavolino;
    public GameObject libreria;
    public GameObject pozione;
    public GameObject chiave;
    public GameObject pezzoChiave0;
    public GameObject pezzoChiave1;
    public GameObject pezzoChiave2;

    public GameObject defaultLevel;

    public List<GameObject> level1Prefab;

    public GameObject PickObjectPrefab(string nomeOggetto)
    {
        if(nomeOggetto.Contains("pezzoChiave0"))
        {
            return pezzoChiave0;
        }
        if (nomeOggetto.Contains("pezzoChiave1"))
        {
            return pezzoChiave1;
        }
        if (nomeOggetto.Contains("pezzoChiave2"))
        {
            return pezzoChiave2;
        }

        if (nomeOggetto.EndsWith(".tar") || nomeOggetto.EndsWith(".tar.gz"))
        {
            if(nomeOggetto.Contains("chiave"))
            {
                return chiave;
            }
            return archivio;
        }

        if (nomeOggetto.Contains("cassa"))
        {
            return cassa;
        }

        if (nomeOggetto.Contains("tappeto"))
        {
            return tappeto;
        }
        if (nomeOggetto.Contains("barile"))
        {
            return barile;
        }
        if (nomeOggetto.Contains("tavolino"))
        {
            return tavolino;
        }

        if (nomeOggetto.Contains("libreria"))
        {
            return libreria;
        }

        if (nomeOggetto.Contains("pozione"))
        {
            return pozione;
        }

        if (nomeOggetto.Contains("pergamena"))
        {
            return pergamena;
        }

        if (nomeOggetto.Contains("NPC"))
        {
            return npc;
        }
        if (nomeOggetto.Contains("chiave"))
        {
            return chiave;
        }
        else
        {
            return pergamena;
        }
    }

    public GameObject PickLevelPrefab(int level)
    {
        GameObject chosenPrefab;

        if (level == 1 && level1Prefab != null)
        {
            chosenPrefab = level1Prefab[Random.Range(0, level1Prefab.Count - 1)];
        }
        else
        {
            chosenPrefab = defaultLevel;
        }
        return chosenPrefab;
    }
}
