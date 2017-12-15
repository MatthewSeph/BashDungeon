using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarileNPC : MonoBehaviour {

    Oggetto barileEliminabile;
    bool primoIncontro = false;
    GameObject gameManager;
    GameObject playerGO;
    string questText = "Elimina il barile che blocca il passaggio nella stanza ";

    public string QuestText
    {
        get
        {
            return questText;
        }

        set
        {
            questText = value;
        }
    }

    // Use this for initialization
    void Start () {
        playerGO = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");

        barileEliminabile = gameManager.GetComponent<LevelGeneration>().GetRoomByName(gameObject.transform.parent.name).oggetti.Find(x => x.nomeOggetto == "barileEliminabile");
    }
	
	// Update is called once per frame
	void Update () {

        if ((!primoIncontro) && (playerGO.GetComponent<PlayerMovement>().currentRoom == barileEliminabile.CurrentRoom))
        {
            gameManager.GetComponent<PlayManager>().AddQuest(QuestText + gameManager.GetComponent<PlayManager>().GetPath(barileEliminabile.CurrentRoom));
            primoIncontro = true;
        }

    }
}
