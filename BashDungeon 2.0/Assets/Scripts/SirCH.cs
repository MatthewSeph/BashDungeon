using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirCH : MonoBehaviour {

    GameObject playerGO;
    Room lootRoom;
    GameObject gameManager;
    Oggetto sirCHNPC;
    bool roomLocked = false;

    string chosenPergamena = "";



    // Use this for initialization
    void Start () {
        playerGO = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");
    }
	
	// Update is called once per frame
	void Update () {

        if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == "SirC.H.NPC") && !roomLocked)
        {
            sirCHNPC = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == "SirC.H.NPC");
            foreach (Room room in sirCHNPC.CurrentRoom.childrenRooms)
            {
                if (room.oggetti.Find(x => x.nomeOggetto == "frammentoPergamena") != null)
                {
                    room.IsLocked = true;
                    roomLocked = true;
                    lootRoom = room;
                }
            }
            int randomSearch = Random.Range(0, 3);
            switch (randomSearch)
            {
                case 0:
                    sirCHNPC.TestoTxT += "\nQuale tra queste tre pergamene inizia con \"Inizialmente\" e finisce con \"Fine\" ?";
                    chosenPergamena = "pergamenaSinistra";
                    break;
                case 1:
                    sirCHNPC.TestoTxT += "\nQuale tra queste tre pergamene finisce con \"Fine\" ma non inizia con \"Inizialmente\" ?";
                    chosenPergamena = "pergamenaCentrale";
                    break;
                case 2:
                    sirCHNPC.TestoTxT += "\nQuale tra queste tre pergamene contiene, ma non inizia ne finisce, con \"ad un tratto\" ?";
                    chosenPergamena = "pergamenaDestra";
                    break;
            }
        }
        else if(roomLocked)
        {
            if(gameManager.GetComponent<PlayManager>().FoundWithGrepGO != null && gameManager.GetComponent<PlayManager>().FoundWithGrepGO.name.Contains(chosenPergamena))
            {
                if(!playerGO.GetComponent<PlayerMovement>().BlockedMovement)
                {
                    sirCHNPC.TestoTxT = "Wow, che velocità! Sei troppo bravo, non c' è gusto..\nVado a cercare altre persone da importunare :P";
                    gameManager.GetComponent<PlayManager>().ClickedObject = GameObject.Find("/" + sirCHNPC.CurrentRoom.nomeStanza + "/" + sirCHNPC.nomeOggetto);
                    lootRoom.IsLocked = false;
                }

                if(!lootRoom.IsLocked && !gameManager.GetComponent<PlayManager>().ClickedObject.name.Contains("SirC.H.NPC") )
                {
                    lootRoom.parentRoom.oggetti.Remove(sirCHNPC);
                    Destroy(gameObject);
                }
            }
        }

    }
}
