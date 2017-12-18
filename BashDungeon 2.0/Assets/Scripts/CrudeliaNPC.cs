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
    bool primoIncontro = false;
    string questText = "Porta i tre cuccioli nascosti nella stanza ";
    // Use this for initialization
    void Start()
    {
        playerGO = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager");
       
                    for (int i = 0; i < 3; i++)
                    {
                        bool isRoomFine = false;
                        Room randomRoom = null;
                        while (!isRoomFine)
                        {
                            randomRoom = gameManager.GetComponent<LevelGeneration>().RandomRoomNoLevelOrRoot();
                            if (!randomRoom.oggetti.Exists(x => x.nomeOggetto.Contains("cuccioloNascosto")))
                            {
                                isRoomFine = true;
                            }
                        }
                        Vector3 oggettoPosition = new Vector3();
                        Oggetto cuccioloNascosto = new Oggetto(randomRoom, "cuccioloNascosto");
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
            if (crudelioNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto.Contains("cuccioloNascosto")) != null && primoIncontro)
            {
                Oggetto cucciolo = crudelioNPC.CurrentRoom.oggetti.Find(x => x.nomeOggetto.Contains("cuccioloNascosto"));
                GameObject cuccioloObj = GameObject.Find("/" + crudelioNPC.CurrentRoom.nomeStanza + "/" + cucciolo.nomeOggetto);
                Destroy(cuccioloObj);
                crudelioNPC.CurrentRoom.oggetti.Remove(cucciolo);
                
                cuccioliTrovati++;
            }
        }
        if(primoIncontro && cuccioliTrovati > 0 && cuccioliTrovati < 3)
        {
            crudelioNPC.TestoTxT = "Che ci fai ancora qui? Mi hai portato solo " + cuccioliTrovati + "/3 cuccioli.";
        }
        if (cuccioliTrovati == 3 && lootRoom.IsLocked && playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Contains(crudelioNPC))
        {
            gameManager.GetComponent<PlayManager>().ClickedObject = gameObject;
            playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
            crudelioNPC.TestoTxT = "Ho finalmente acchiappato quelle bestiacce, TUTTE e 100 !!!\n..Erano 100, giusto?..\nAddio, \"buonuomo\"... MUAHAHAHAHA";
            lootRoom.IsLocked = false;
        }
        else if (gameManager.GetComponent<PlayManager>().ClickedObject == null && cuccioliTrovati == 3 && !lootRoom.IsLocked)
        {
            gameManager.GetComponent<PlayManager>().RemoveQuest(questText);
            crudelioNPC.CurrentRoom.oggetti.Remove(crudelioNPC);
            Destroy(gameObject);
        }

        if ((!primoIncontro) && (gameManager.GetComponent<PlayManager>().ClickedObject == gameObject) && (playerGO.GetComponent<PlayerMovement>().BlockedMovement))
        {
            gameManager.GetComponent<PlayManager>().AddQuest(questText + gameManager.GetComponent<PlayManager>().GetPath(crudelioNPC.CurrentRoom));
            primoIncontro = true;
        }
    }

}
