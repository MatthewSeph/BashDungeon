using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarileNPC : MonoBehaviour {

    Oggetto barileEliminabile;
    bool primoIncontro = false;
    GameObject gameManager;
    GameObject playerGO;

	// Use this for initialization
	void Start () {
        playerGO = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");

        barileEliminabile = gameManager.GetComponent<LevelGeneration>().GetRoomByName(gameObject.transform.parent.name).oggetti.Find(x => x.nomeOggetto == "barileEliminabile");
    }
	
	// Update is called once per frame
	void Update () {

        if ((!primoIncontro) && (gameManager.GetComponent<PlayManager>().ClickedObject == gameObject) && (playerGO.GetComponent<PlayerMovement>().BlockedMovement))
        {
            gameManager.GetComponent<PlayManager>().AddQuest("Elimina il barile che blocca il passaggio nella stanza " + gameManager.GetComponent<PlayManager>().GetPath(barileEliminabile.CurrentRoom));
            primoIncontro = true;
        }

    }
}
