using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyguardNPC : MonoBehaviour {

    GameObject playerGO;
    Oggetto bodyguardNPC;
    GameObject gameManager;
    Room lootRoom;
    int numberOfTraps = 3;
    bool roomLocked = false;
    bool primoIncontro = false;
    string questText = "Elimina le trappone nascoste nella stanza ";

    // Use this for initialization
    void Start () {
        playerGO = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");

    }
	
	// Update is called once per frame
	void Update () {

        if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == "bodyguardNPC") && !roomLocked)
        {
            bodyguardNPC = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == "bodyguardNPC");
            foreach (Room room in bodyguardNPC.CurrentRoom.childrenRooms)
            {
                if (room.oggetti.Find(x => x.nomeOggetto == "frammentoPergamena") != null)
                {
                    room.IsLocked = true;
                    roomLocked = true;
                    lootRoom = room;
                }
            }
        }
        else if (roomLocked)
        {
            if (bodyguardNPC.CurrentRoom.oggetti.Exists(x => x.nomeOggetto.Contains(".trappolaEliminabileNascosta")) && bodyguardNPC.CurrentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(".trappolaEliminabileNascosta")).Count < numberOfTraps)
            {
                bodyguardNPC.TestoTxT = "Ci sai fare.. Continua così!";
            }
            else if(!bodyguardNPC.CurrentRoom.oggetti.Exists(x => x.nomeOggetto.Contains(".trappolaEliminabileNascosta")))
            {
                gameManager.GetComponent<PlayManager>().RemoveQuest(questText);
                lootRoom.IsLocked = false;
                bodyguardNPC.TestoTxT = "La via è libera";
                Destroy(this);
            }
        }
        if ((!primoIncontro) && (gameManager.GetComponent<PlayManager>().ClickedObject == gameObject) && (playerGO.GetComponent<PlayerMovement>().BlockedMovement))
        {
            gameManager.GetComponent<PlayManager>().AddQuest(questText + gameManager.GetComponent<PlayManager>().GetPath(bodyguardNPC.CurrentRoom));
            primoIncontro = true;
        }
    }
}
