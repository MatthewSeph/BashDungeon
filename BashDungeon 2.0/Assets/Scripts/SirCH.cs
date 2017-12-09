using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirCH : MonoBehaviour {

    GameObject playerGO;
    Room lootRoom;
    GameObject gameManager;
    Oggetto sirCHNPC;
    bool roomLocked = false;
    bool endLevel = false;

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
                    chosenPergamena = "pergamenaDestra";
                    break;
                case 2:
                    sirCHNPC.TestoTxT += "\nQuale tra queste tre pergamene finisce con \"tratto\" e inizia con \"Inizialmente\" ?";
                    chosenPergamena = "pergamenaCentrale";
                    break;
            }
        }

        if (roomLocked)
        {

            if(gameManager.GetComponent<PlayManager>().FoundWithGrepGO != null && gameManager.GetComponent<PlayManager>().FoundWithGrepGO.name.Contains(chosenPergamena) && !endLevel)
            {

                    playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
                    endLevel = true;   
            }
            if (!playerGO.GetComponent<PlayerMovement>().BlockedMovement && endLevel && lootRoom.IsLocked)
            {
                

                sirCHNPC.TestoTxT = "Wow, che velocità! Sei troppo bravo, non c' è gusto..\nVado a cercare altre persone da importunare :P";
                gameManager.GetComponent<PlayManager>().ClickedObject = GameObject.Find("/" + sirCHNPC.CurrentRoom.nomeStanza + "/" + sirCHNPC.nomeOggetto);
                lootRoom.IsLocked = false;
                playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
            }

            if (!lootRoom.IsLocked && !playerGO.GetComponent<PlayerMovement>().BlockedMovement && endLevel)
            {

                lootRoom.parentRoom.oggetti.Remove(sirCHNPC);
                Destroy(gameObject);
            }
        }

    }
}
