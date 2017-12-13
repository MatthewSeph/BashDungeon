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
    public GameObject cuccioloNascosto;

    public GameObject pergamenaFoundUI;

    public GameObject defaultLevel;

    public List<GameObject> standardRoomPrefab;

    public List<GameObject> level1Prefab;
    public List<GameObject> level2Prefab;
    public List<GameObject> level3Prefab;
    public List<GameObject> level4Prefab;

    public GameObject PergamenaFoundUI
    {
        get
        {
            return pergamenaFoundUI;
        }
    }

    public GameObject PickObjectPrefab(string nomeOggetto)
    {
        if (nomeOggetto.Contains("cuccioloNascosto"))
        {
            return cuccioloNascosto;
        }
        if (nomeOggetto.Contains("pezzoChiave0"))
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

        if (nomeOggetto.Contains("chiave"))
        {
            return chiave;
        }

        if (nomeOggetto.EndsWith(".tar") || nomeOggetto.EndsWith(".tar.gz"))
        {
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

        else
        {
            return pergamena;
        }
    }

    public GameObject PickStandardRoomPrefab()
    {
        return standardRoomPrefab[Random.Range(0, standardRoomPrefab.Count)];
    }

    public GameObject PickLevelPrefab(int level)
    {
        GameObject chosenPrefab;

        if (level == 1 && level1Prefab != null)
        {
            chosenPrefab = level1Prefab[Random.Range(0, level1Prefab.Count)];
        }
        else if (level == 2 && level2Prefab != null)
        {
            chosenPrefab = level2Prefab[Random.Range(0, level2Prefab.Count)];
        }
        else if (level == 3 && level3Prefab != null)
        {
            chosenPrefab = level3Prefab[Random.Range(0, level3Prefab.Count)];
        }
        else if (level == 4 && level4Prefab != null)
        {
            chosenPrefab = level4Prefab[Random.Range(0, level4Prefab.Count)];
        }
        else
        {
            chosenPrefab = defaultLevel;
        }
        return chosenPrefab;
    }
}
