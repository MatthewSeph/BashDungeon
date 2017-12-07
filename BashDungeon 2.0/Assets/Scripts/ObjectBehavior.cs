using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour {

    public bool isMadeVisible;
    bool isBeingCompressed;
    bool isVisible;

    GameObject playerGO;
   
    Color myColorAlphaZero;
    Color myColorFullAlpha;
    public List<GameObject> oggettiGOArchiviati;
    public List<Oggetto> oggettiArchiviati;

    GameObject gameManager;

    public bool IsBeingCompressed
    {
        get
        {
            return isBeingCompressed;
        }

        set
        {
            isBeingCompressed = value;
        }
    }

    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        transform.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        myColorAlphaZero = transform.GetComponent<MeshRenderer>().material.color;
        myColorAlphaZero.a = 0;
        myColorFullAlpha = transform.GetComponent<MeshRenderer>().material.color;
        myColorFullAlpha.a = 75;
        gameManager = GameObject.Find("GameManager");

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

        if(this.gameObject.name.EndsWith(".tar.gz") && !IsBeingCompressed)
        {
            Vector3 scaledVector = this.gameObject.transform.localScale;
            scaledVector.x /= 2;
            scaledVector.z /= 2;
            scaledVector.y /= 2;
            this.gameObject.transform.localScale = scaledVector;

            IsBeingCompressed = true;
        }

        if(oggettiGOArchiviati.Count == 1)
        {
            if(oggettiGOArchiviati[0].transform.name.Contains("Gigante") && IsBeingCompressed)
            {
                Oggetto thisOggetto = gameManager.GetComponent<LevelGeneration>().GetRoomByName(gameObject.transform.parent.name).oggetti.Find(x => x.nomeOggetto.Contains(gameObject.transform.name) && x.IsActive);
                thisOggetto.nomeOggetto = thisOggetto.nomeOggetto.Replace(".tar.gz", "");
                thisOggetto.IsMovable = true;
                thisOggetto.IsTar = false;
                gameObject.transform.name = gameObject.transform.name.Replace(".tar.gz", "");
                oggettiGOArchiviati.Remove(oggettiGOArchiviati[0]);
            }
        }
        else if(oggettiGOArchiviati.Count == 3 && oggettiGOArchiviati.FindAll(x => x.transform.name.Contains("pezzoChiave")).Count == 3)
        {
            foreach(GameObject go in oggettiGOArchiviati)
            {
                gameManager.GetComponent<LevelGeneration>().GetRoomByName(gameObject.transform.parent.name).oggetti.Find(x => x.nomeOggetto == go.name).IsActive = false;
            }
            Oggetto thisOggetto = gameManager.GetComponent<LevelGeneration>().GetRoomByName(gameObject.transform.parent.name).oggetti.Find(x => x.nomeOggetto.Contains(gameObject.transform.name) && x.IsActive);
            thisOggetto.nomeOggetto = "chiave";
            thisOggetto.IsMovable = true;
            thisOggetto.IsTar = false;
            gameObject.transform.name = "chiave";

            oggettiGOArchiviati.Clear();
        }
    }

    public void SettaOff()
    {
        this.enabled = false;
    }

    private void OnMouseDown()
    {
        gameManager.GetComponent<PlayManager>().ClickedObject = gameObject;
    }

    private void OnMouseEnter()
    {
        gameManager.GetComponent<PlayManager>().IsMouseOverObj = true;
    }
    private void OnMouseExit()
    {
        gameManager.GetComponent<PlayManager>().IsMouseOverObj = false;
    }


}
