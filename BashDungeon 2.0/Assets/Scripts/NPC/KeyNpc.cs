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
    bool primoIncontro = false;
    string questText = "Porta una chiave nella stanza ";

    // Use this for initialization
    void Start () {
        playerGO = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");
        int chiaveRandom = Random.Range(0, 3);

        switch (chiaveRandom)
        {
            case 0:
                {
                    Vector3 oggettoPosition = new Vector3();
                    Oggetto chiave = new Oggetto(gameManager.GetComponent<LevelGeneration>().RandomRoomNoLevelOrRoot(), "chiave");
                    chiave.IsMovable = true;
                    chiave.CurrentRoom.oggetti.Add(chiave);
                    chiave.nomeOggetto += "Gigante";
                    chiave.CanZCF = true;
                    GameObject selectedPrefab = gameManager.GetComponent<ObjectPrefabSelector>().PickObjectPrefab(Regex.Replace(chiave.nomeOggetto, "[0-9]", ""));

                    GameObject oggettoIstanziato = Instantiate(selectedPrefab) as GameObject;

                    oggettoPosition.y = oggettoIstanziato.transform.position.y;
                    oggettoPosition.x = oggettoIstanziato.transform.position.x + (chiave.CurrentRoom.gridPos.x * 24);
                    oggettoPosition.z = oggettoIstanziato.transform.position.z + (chiave.CurrentRoom.gridPos.y * 24);

                    oggettoIstanziato.transform.position = oggettoPosition;
                    oggettoIstanziato.transform.localScale *= 2;
                    oggettoIstanziato.name = chiave.nomeOggetto;
                    oggettoIstanziato.transform.parent = GameObject.Find("/" + chiave.CurrentRoom.nomeStanza).transform;
                    break;
                }
            case 1:
                {
                    Vector3 oggettoPosition = new Vector3();
                    Room randomRoom = gameManager.GetComponent<LevelGeneration>().RandomRoomNoLevelOrRoot();
                    Oggetto chiave = new Oggetto(randomRoom, "chiave");
                    chiave.IsActive = false;
                    chiave.IsMovable = true;
                    chiave.CurrentRoom.oggetti.Add(chiave);

                    Oggetto chiaveMinuscola = new Oggetto(randomRoom, "chiave");
                    chiaveMinuscola.IsMovable = true;
                    chiaveMinuscola.CurrentRoom.oggetti.Add(chiaveMinuscola);
                    chiaveMinuscola.nomeOggetto += "Minuscola.tar.gz";
                    chiaveMinuscola.IsTar = true;
                    chiaveMinuscola.IsZip = true;
                    chiaveMinuscola.CanZXF = true;

                    GameObject selectedPrefab = gameManager.GetComponent<ObjectPrefabSelector>().PickObjectPrefab(Regex.Replace(chiaveMinuscola.nomeOggetto, "[0-9]", ""));

                    GameObject chiaveGO = Instantiate(selectedPrefab) as GameObject;
                    GameObject chiaveMinuscolaGO = Instantiate(selectedPrefab) as GameObject;

                    oggettoPosition.y = chiaveMinuscolaGO.transform.position.y;
                    oggettoPosition.x = chiaveMinuscolaGO.transform.position.x + (chiaveMinuscola.CurrentRoom.gridPos.x * 24);
                    oggettoPosition.z = chiaveMinuscolaGO.transform.position.z + (chiaveMinuscola.CurrentRoom.gridPos.y * 24);

                    chiaveMinuscolaGO.transform.position = oggettoPosition;
                    chiaveGO.transform.position = oggettoPosition;

                    chiaveGO.name = chiave.nomeOggetto;
                    chiaveMinuscolaGO.name = chiaveMinuscola.nomeOggetto;

                    chiaveGO.transform.parent = GameObject.Find("/" + chiaveMinuscola.CurrentRoom.nomeStanza).transform;
                    chiaveMinuscolaGO.transform.parent = GameObject.Find("/" + chiaveMinuscola.CurrentRoom.nomeStanza).transform;

                    chiaveGO.SetActive(false);
                    chiaveMinuscolaGO.transform.localScale = chiaveMinuscolaGO.transform.localScale/2;
                    chiaveMinuscolaGO.GetComponent<ObjectBehavior>().oggettiGOArchiviati.Add(chiaveGO);
                    break;
                }
            case 2:
                {

                    for (int i = 0; i < 3; i++)
                    {
                        bool isRoomFine = false;
                        Room randomRoom = null;
                        while (!isRoomFine)
                        {
                            randomRoom = gameManager.GetComponent<LevelGeneration>().RandomRoomNoLevelOrRoot();
                            if (!randomRoom.oggetti.Exists(x => x.nomeOggetto.Contains("pezzoChiave")))
                            {
                                isRoomFine = true;
                            }
                        }

                        Vector3 oggettoPosition = new Vector3();
                        Oggetto chiavePezzo = new Oggetto(randomRoom, "pezzoChiave");
                        chiavePezzo.IsMovable = true;
                        chiavePezzo.CurrentRoom.oggetti.Add(chiavePezzo);
                        chiavePezzo.CanCF = true;

                        GameObject selectedPrefab = gameManager.GetComponent<ObjectPrefabSelector>().PickObjectPrefab(chiavePezzo.nomeOggetto + i);

                        GameObject oggettoIstanziato = Instantiate(selectedPrefab) as GameObject;

                        oggettoPosition.y = oggettoIstanziato.transform.position.y;
                        oggettoPosition.x = oggettoIstanziato.transform.position.x + (chiavePezzo.CurrentRoom.gridPos.x * 24);
                        oggettoPosition.z = oggettoIstanziato.transform.position.z + (chiavePezzo.CurrentRoom.gridPos.y * 24);

                        oggettoIstanziato.transform.position = oggettoPosition;
 
                        oggettoIstanziato.name = chiavePezzo.nomeOggetto;
                        oggettoIstanziato.transform.parent = GameObject.Find("/" + chiavePezzo.CurrentRoom.nomeStanza).transform;
                        
                    }
                    break;
                }

        }

       
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
            if(keyNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto == "chiave")!=null)
            {
                Oggetto chiave = keyNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto == "chiave");
                GameObject chiaveObj = GameObject.Find("/" + keyNPC.CurrentRoom.nomeStanza +"/"+chiave.nomeOggetto);
                Destroy(chiaveObj);
                keyNPC.CurrentRoom.oggetti.Remove(chiave);
                keyNPC.TestoTxT = "Prima non c' era nulla e poi..\n..Puff..\nLa chiave è comparsa proprio davanti a me!!\nLa stanchezza fa brutti scherzi....";
                gameManager.GetComponent<PlayManager>().RemoveQuest(questText);
                lootRoom.IsLocked = false;
                Destroy(this);
            }
        }

        if ((!primoIncontro) && (gameManager.GetComponent<PlayManager>().ClickedObject == gameObject) && (playerGO.GetComponent<PlayerMovement>().BlockedMovement))
        {
            gameManager.GetComponent<PlayManager>().AddQuest(questText + gameManager.GetComponent<PlayManager>().GetPath(keyNPC.CurrentRoom));
            primoIncontro = true;
        }

	}
}
