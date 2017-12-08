using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrudeliaNPC : MonoBehaviour {

    GameObject playerGO;
    Oggetto crudelioNPC;
    GameObject gameManager;
    Room lootRoom;
    bool roomLocked = false;
    int cuccioliTrovati = 0;

    // Use this for initialization
    void Start()
    {
        playerGO = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");
       
                    for (int i = 0; i < 3; i++)
                    {
                        Vector3 oggettoPosition = new Vector3();
                        Oggetto cuccioloNascosto = new Oggetto(gameManager.GetComponent<LevelGeneration>().RandomRoomNoLevelOrRoot(), "cuccioloNascosto");
                        cuccioloNascosto.IsMovable = true;
                        cuccioloNascosto.IsInvisible = true;
                        cuccioloNascosto.CurrentRoom.oggetti.Add(cuccioloNascosto);

                        GameObject selectedPrefab = gameManager.GetComponent<ObjectPrefabSelector>().PickObjectPrefab("cuccioloNascosto");

                        GameObject oggettoIstanziato = Instantiate(selectedPrefab) as GameObject;

                        oggettoPosition.y = oggettoIstanziato.transform.position.y;
                        oggettoPosition.x = Random.Range(-8, 5) + (cuccioloNascosto.CurrentRoom.gridPos.x * 24);
                        oggettoPosition.z = Random.Range(-6, 6) + (cuccioloNascosto.CurrentRoom.gridPos.y * 24);

                        oggettoIstanziato.transform.position = oggettoPosition;

                        oggettoIstanziato.name = cuccioloNascosto.nomeOggetto;
                        oggettoIstanziato.transform.parent = GameObject.Find("/" + cuccioloNascosto.CurrentRoom.nomeStanza).transform;
                    }
    }

    // Update is called once per frame
    void Update()
    {

        if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == "CrudelioDeMonNPC") && !roomLocked)
        {
            crudelioNPC = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == "CrudelioDeMonNPC");
            foreach (Room room in crudelioNPC.CurrentRoom.childrenRooms)
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
            if (crudelioNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto.Contains("cuccioloNascosto")) != null)
            {
                Oggetto cucciolo = crudelioNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto.Contains("cuccioloNascosto"));
                GameObject cuccioloObj = GameObject.Find("/" + crudelioNPC.CurrentRoom.nomeStanza + "/" + cucciolo.nomeOggetto);
                Destroy(cuccioloObj);
                crudelioNPC.CurrentRoom.oggetti.Remove(cucciolo);
                
                cuccioliTrovati++;
            }
        }
        if (cuccioliTrovati == 3)
        {
            gameManager.GetComponent<PlayManager>().ClickedObject = gameObject;
            playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
            crudelioNPC.TestoTxT = "Ho finalmente acchiappato quelle bestiacce, TUTTE e 100 !!!\n..Erano 100, giusto?..\nAddio, \"buonuomo\"... MUAHAHAHAHA";
            lootRoom.IsLocked = false;
        }

        else if (gameManager.GetComponent<PlayManager>().ClickedObject != null && gameManager.GetComponent<PlayManager>().ClickedObject.name != "CrudelioDeMonNPC" && cuccioliTrovati == 3 && !lootRoom.IsLocked)
        {
            crudelioNPC.CurrentRoom.oggetti.Remove(crudelioNPC);
            Destroy(gameObject);
        }
    }
}
