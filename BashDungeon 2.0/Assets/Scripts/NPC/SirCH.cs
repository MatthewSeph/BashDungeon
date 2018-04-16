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
    bool primoIncontro = false;
    string questText = "Risolvi l'indovinello di SirCH nella stanza ";

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
            int randomSearch = Random.Range(0, 4);

            switch (randomSearch)
            {
                case 0:
                    sirCHNPC.TestoTxT += "\nQuale tra queste tre pergamene inizia con \"Inizialmente\" ?";
                    chosenPergamena = "pergamenaSinistra";
                    break;
                case 1:
                    sirCHNPC.TestoTxT += "\nQuale tra queste tre pergamene finisce con \"Fine\" ?";
                    chosenPergamena = "pergamenaDestra";
                    break;
                case 2:
                    sirCHNPC.TestoTxT += "\nQuale tra queste tre pergamene finisce con \"tratto\"?";
                    chosenPergamena = "pergamenaCentrale";
                    break;

                case 3:
                    sirCHNPC.TestoTxT += "\nQuale tra queste tre pergamene contiene la parola \"Inizialmente\" ma non inizia ne finisce con essa ?";
                    chosenPergamena = "pergamenaDestra";
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
                gameManager.GetComponent<PlayManager>().RemoveQuest(questText);
                lootRoom.parentRoom.oggetti.Remove(sirCHNPC);
                Destroy(gameObject);
            }
        }

        if ((!primoIncontro) && (gameManager.GetComponent<PlayManager>().ClickedObject == gameObject) && (playerGO.GetComponent<PlayerMovement>().BlockedMovement))
        {
            gameManager.GetComponent<PlayManager>().AddQuest(questText + gameManager.GetComponent<PlayManager>().GetPath(sirCHNPC.CurrentRoom));
            primoIncontro = true;
        }

    }
}
