using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class KeyNpc : MonoBehaviour {

    GameObject playerGO;
    Oggetto keyNPC;
    GameObject gameManager;
    Room lootRoom;
    bool roomLocked = false;

    // Use this for initialization
    void Start () {
        playerGO = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");

        Vector3 oggettoPosition = new Vector3();
        Oggetto chiave = new Oggetto(gameManager.GetComponent<LevelGeneration>().RandomRoomNoLevelOrRoot(), "chiave");
        chiave.IsMovable = true;
        chiave.CurrentRoom.oggetti.Add(chiave);
        GameObject selectedPrefab = gameManager.GetComponent<ObjectPrefabSelector>().PickObjectPrefab(Regex.Replace(chiave.nomeOggetto, "[0-9]", ""));

        GameObject oggettoIstanziato = Instantiate(selectedPrefab) as GameObject;

        oggettoPosition.y = oggettoIstanziato.transform.position.y;
        oggettoPosition.x = oggettoIstanziato.transform.position.x + (chiave.CurrentRoom.gridPos.x * 24);
        oggettoPosition.z = oggettoIstanziato.transform.position.z + (chiave.CurrentRoom.gridPos.y * 24);

        oggettoIstanziato.transform.position = oggettoPosition;

        oggettoIstanziato.name = chiave.nomeOggetto;
        oggettoIstanziato.transform.parent = GameObject.Find("/" + chiave.CurrentRoom.nomeStanza).transform;
    }
	
	// Update is called once per frame
	void Update () {

		if(playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == "keyNPC") && !roomLocked)
        {
            keyNPC = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == "keyNPC");
            foreach(Room room in keyNPC.CurrentRoom.childrenRooms)
            {
                if(room.oggetti.Find(x => x.nomeOggetto == "frammentoPergamena")!= null)
                {
                    room.IsLocked = true;
                    roomLocked = true;
                    lootRoom = room;
                }
            }
        }
        else if (roomLocked)
        {
            if(keyNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto.Contains("chiave"))!=null)
            {
                Oggetto chiave = keyNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto.Contains("chiave"));
                GameObject chiaveObj = GameObject.Find("/" + keyNPC.CurrentRoom.nomeStanza +"/"+chiave.nomeOggetto);
                Destroy(chiaveObj);
                keyNPC.CurrentRoom.oggetti.Remove(chiave);
                keyNPC.TestoTxT = "Prima non c' era nulla e poi..\n..Puff..\nLa chiave è comparsa proprio davanti a me!!\nLa stanchezza fa brutti scherzi....";

                lootRoom.IsLocked = false;
                Destroy(this);
            }
        }
	}
}
