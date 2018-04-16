using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{

    GameObject playerGO;
    GameObject clickedObject;
    GameObject foundWithGrepGO;
    bool isMouseOverObj;

    public GameObject pergamenaPanel;
    public GameObject pergamenaText;

    public GameObject dialoguePanel;
    public GameObject dialogueText;

    public GameObject tutorialPanel;

    public GameObject MenuUI;
    public GameObject MenuPanel;
    public GameObject questsPanel;
    public GameObject foundPanel;
    public GameObject listOfFoundTxtUI;
    public GameObject listOfQuests;

    public List<GameObject> addedQuests;
    private bool fineGioco = false;

    LogWriter logWriter;

    public bool FineGioco
    {
        get
        {
            return fineGioco;
        }

        set
        {
            fineGioco = value;
        }
    }

    public GameObject ClickedObject
    {
        get
        {
            return clickedObject;
        }

        set
        {
            if (!playerGO.GetComponent<PlayerMovement>().BlockedMovement)
            {
                if (clickedObject != null)
                {
                    if(clickedObject.transform.childCount == 0)
                    {
                        clickedObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * 0f);
                    }
                    else
                    {
                        clickedObject.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.white * 0f);
                    }
                    
                }
                if (value != null)
                {
                    playerGO.GetComponent<NavMeshAgent>().destination = value.transform.position;
                }
                clickedObject = value;
                if (clickedObject != null)
                {
                   if (clickedObject.GetComponent<Renderer>() != null)
                    {
                        clickedObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * 0.3f);
                     }
                    else
                    {
                        clickedObject.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.white * 0.3f);
                    }
                    
                }
                else
                {
                    FoundWithGrepGO = null;
                }
            }
        }
    }


    public bool IsMouseOverObj
    {
        get
        {
            return isMouseOverObj;
        }

        set
        {
            isMouseOverObj = value;
        }
    }

    public GameObject FoundWithGrepGO
    {
        get
        {
            return foundWithGrepGO;
        }

        set
        {

            foundWithGrepGO = value;
            if (foundWithGrepGO != null)
            {
                ClickedObject = FoundWithGrepGO;
            }
        }
    }

    void Start()
    {
        playerGO = GameObject.Find("Player");
        addedQuests = new List<GameObject>();
        logWriter = GetComponent<LogWriter>();
    }

    private void Update()
    {

        if (ClickedObject != null && Vector2.Distance(new Vector2(playerGO.transform.position.x, playerGO.transform.position.z), new Vector2(ClickedObject.transform.position.x, ClickedObject.transform.position.z)) <= 2.5f && !pergamenaPanel.activeSelf && !dialoguePanel.activeSelf)
        {
            if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == ClickedObject.name).IsTxt)
            {
                if (!clickedObject.name.Contains("pergamenaCentrale") && !clickedObject.name.Contains("pergamenaDestra") && !clickedObject.name.Contains("pergamenaSinistra"))
                {
                    Oggetto oggettoPergamena = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == ClickedObject.transform.name);
                    string testoPergamena = oggettoPergamena.TestoTxT;

                    //User is reading this scroll for the first time
                    if (!oggettoPergamena.HasBeenRead)
                    {
                        oggettoPergamena.HasBeenRead = true;
                        GameObject newPergamenaFound = Instantiate(gameObject.GetComponent<ObjectPrefabSelector>().PergamenaFoundUI) as GameObject;
                        newPergamenaFound.transform.SetParent(listOfFoundTxtUI.transform, false);
                        //newPergamenaFound.transform.localScale = new Vector3(1, 1, 1);
                        newPergamenaFound.GetComponentInChildren<Text>().text = oggettoPergamena.TestoTxT;

                        logWriter.ScrollReadToLog(oggettoPergamena.TestoTxT.Substring(0, 20));
                    }

                    pergamenaText.GetComponent<Text>().text = testoPergamena;

                    pergamenaPanel.SetActive(true);
                    playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
                    playerGO.transform.LookAt(new Vector3(ClickedObject.transform.position.x, playerGO.transform.position.y, ClickedObject.transform.position.z));
                    playerGO.GetComponent<NavMeshAgent>().ResetPath();
                }
                else
                {
                    dialoguePanel.SetActive(true);
                    dialogueText.GetComponent<DialogueController>().SetText("Leggendola sarebbe troppo facile...");
                }
            }
            else if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == ClickedObject.name).IsNPC)
            {

                string testoDialogo = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == ClickedObject.transform.name).TestoTxT;

                dialogueText.GetComponent<DialogueController>().SetText(testoDialogo);

                dialoguePanel.SetActive(true);
                playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
                playerGO.transform.LookAt(new Vector3(ClickedObject.transform.position.x, playerGO.transform.position.y, ClickedObject.transform.position.z));
                playerGO.GetComponent<NavMeshAgent>().ResetPath();

            }
        }
        else if (ClickedObject != null && Vector2.Distance(new Vector2(playerGO.transform.position.x, playerGO.transform.position.z), new Vector2(ClickedObject.transform.position.x, ClickedObject.transform.position.z)) > 3.5f && !playerGO.GetComponent<NavMeshAgent>().hasPath && !playerGO.GetComponent<NavMeshAgent>().pathPending)
        {
            dialoguePanel.SetActive(true);
            dialogueText.GetComponent<DialogueController>().SetText("Che nervoso, non riesco a raggiungerlo !");
        }
        else if (FoundWithGrepGO != null && Vector2.Distance(new Vector2(playerGO.transform.position.x, playerGO.transform.position.z), new Vector2(FoundWithGrepGO.transform.position.x, FoundWithGrepGO.transform.position.z)) <= 2.5f)
        {
            pergamenaPanel.SetActive(false);
            dialoguePanel.SetActive(true);
            dialogueText.GetComponent<DialogueController>().SetText("Ecco la pergamena che cercavo !");
        }
    }

    public void SetMenuUIActive()
    {
        MenuUI.SetActive(true);
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
    }

    public void SetMenuUIOff()
    {
        MenuUI.SetActive(false);
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = false;
    }

    public void SetMenuPanelActive()
    {
        MenuPanel.SetActive(true);
        SetFoundPanelOff();
        SetQuestPanelOff();
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
    }

    public void SetQuestPanelActive()
    {
        questsPanel.SetActive(true);
        SetFoundPanelOff();
        SetMenuPanelOff();
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
    }

    public void SetFoundPanelActive()
    {
        foundPanel.SetActive(true);
        SetMenuPanelOff();
        SetQuestPanelOff();
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
    }

    public void SetMenuPanelOff()
    {
        MenuPanel.SetActive(false);
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = false;
    }

    public void SetQuestPanelOff()
    {
        questsPanel.SetActive(false);
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = false;
    }

    public void SetFoundPanelOff()
    {
        foundPanel.SetActive(false);
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = false;
    }

    public void OnCloseDialogues()
    {
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = false;
        ClickedObject = null;
        dialoguePanel.SetActive(false);
        pergamenaPanel.SetActive(false);
    }

    public Vector2 RoomDirection(Room currentRoom, Room roomToGo)
    {
        Vector2 resultVector = new Vector2();

        resultVector.x = roomToGo.gridPos.x - currentRoom.gridPos.x;
        resultVector.y = roomToGo.gridPos.y - currentRoom.gridPos.y;

        return resultVector;
    }

    public void GoToDoor(Vector2 roomDirection)
    {


        Vector3 positionToGo = new Vector3();
        if (roomDirection == Vector2.up)
        {
            //playerGO.GetComponent<NavMeshAgent>().enabled = true;
            positionToGo.x = -2 + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.x * 24);
            positionToGo.z = 10.5f + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.y * 24);

            positionToGo.y = 0.5f;
            playerGO.GetComponent<NavMeshAgent>().destination = positionToGo;
            playerGO.GetComponent<PlayerMovement>().TargetPosition = positionToGo;
        }
        else if (roomDirection == Vector2.down)
        {
            //playerGO.GetComponent<NavMeshAgent>().enabled = true;
            positionToGo.x = -2 + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.x * 24);

            positionToGo.z = -10.5f + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.y * 24);

            positionToGo.y = 0.5f;
            playerGO.GetComponent<NavMeshAgent>().destination = positionToGo;
            playerGO.GetComponent<PlayerMovement>().TargetPosition = positionToGo;
        }
        else if (roomDirection == Vector2.left)
        {
            //playerGO.GetComponent<NavMeshAgent>().enabled = true;
            positionToGo.x = -12.5f + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.x * 24);

            positionToGo.z = 0 + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.y * 24);

            positionToGo.y = 0.5f;
            playerGO.GetComponent<NavMeshAgent>().destination = positionToGo;
            playerGO.GetComponent<PlayerMovement>().TargetPosition = positionToGo;
        }
        else if (roomDirection == Vector2.right)
        {
            //playerGO.GetComponent<NavMeshAgent>().enabled = true;
            positionToGo.x = 8.5f + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.x * 24);

            positionToGo.z = 0 + (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.y * 24);

            positionToGo.y = 0.5f;
            playerGO.GetComponent<NavMeshAgent>().destination = positionToGo;
            playerGO.GetComponent<PlayerMovement>().TargetPosition = positionToGo;
        }
        else
        {
            if (!playerGO.GetComponent<PlayerMovement>().TarghetRoom.IsLocked)
            {
                playerGO.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                StartCoroutine(ChangeRoomWithCooldown(3));
            }
            else
            {
                //playerGO.GetComponent<NavMeshAgent>().enabled = true;
                dialoguePanel.SetActive(true);
                dialogueText.GetComponent<DialogueController>().SetText("Non riesco a raggiungere la stanza, qualcosa blocca il mio teletrasporto..");
                playerGO.GetComponent<PlayerMovement>().WantToChangeRoom = false;
                playerGO.GetComponent<PlayerMovement>().TarghetRoom = null;
            }

        }

    }

    public void MoveBeforeChangeRoom(Room roomToGo)
    {
        playerGO.GetComponent<PlayerMovement>().WantToChangeRoom = true;
        Vector2 roomDirection = new Vector2();
        playerGO.GetComponent<PlayerMovement>().TarghetRoom = roomToGo;
        roomDirection = RoomDirection(playerGO.GetComponent<PlayerMovement>().currentRoom, roomToGo);
        GoToDoor(roomDirection);

    }

    public void ChangeRoom(Room targhetRoom)
    {
        if (!targhetRoom.IsLocked)
        {
            Room oldRoom = playerGO.GetComponent<PlayerMovement>().currentRoom;
            playerGO.transform.parent = GameObject.Find("/" + targhetRoom.nomeStanza).transform;
            playerGO.GetComponent<PlayerMovement>().currentRoom = targhetRoom;
            GoToDoor(RoomDirection(playerGO.GetComponent<PlayerMovement>().currentRoom, oldRoom));

            Camera.main.transform.parent = GameObject.Find("/" + targhetRoom.nomeStanza).transform;
        }
        else
        {
            //playerGO.GetComponent<NavMeshAgent>().enabled = true;
            dialoguePanel.SetActive(true);
            dialogueText.GetComponent<DialogueController>().SetText("Non riesco ad entrare..");
            playerGO.GetComponent<PlayerMovement>().WantToChangeRoom = false;
            playerGO.GetComponent<PlayerMovement>().TarghetRoom = null;
        }
    }

    public void SkipTutorial()
    {
        GameObject sirGiorgio = GameObject.Find("//SirGiorgioNPC");
        sirGiorgio.GetComponent<SirGiorgio>().SetTutorialBools();
        pergamenaPanel.SetActive(false);
        dialoguePanel.SetActive(false);
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = false;
        ClickedObject = null;
        SetTutorialPanelOff();
    }

    public void SetTutorialPanelOff()
    {
        tutorialPanel.SetActive(false);
    }

    IEnumerator ChangeRoomWithCooldown(int sec)
    {
        Vector3 oldLocalPosition = playerGO.transform.position;

        oldLocalPosition.x = oldLocalPosition.x - (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.x * 24);
        oldLocalPosition.z = oldLocalPosition.z - (playerGO.GetComponent<PlayerMovement>().currentRoom.gridPos.y * 24);
        if (playerGO.GetComponent<PlayerMovement>().TarghetRoom.oggetti.Exists(x => x.nomeOggetto == "postazioneTeletrasporto"))
        {
            oldLocalPosition.x = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().TarghetRoom.nomeStanza + "/" + "postazioneTeletrasporto").transform.position.x;
            oldLocalPosition.z = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().TarghetRoom.nomeStanza + "/" + "postazioneTeletrasporto").transform.position.z;
        }
        else
        {
            oldLocalPosition.x = oldLocalPosition.x + (playerGO.GetComponent<PlayerMovement>().TarghetRoom.gridPos.x * 24);
            oldLocalPosition.z = oldLocalPosition.z + (playerGO.GetComponent<PlayerMovement>().TarghetRoom.gridPos.y * 24);
        }
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
        playerGO.GetComponent<PlayerMovement>().WantToChangeRoom = false;
        yield return new WaitForSeconds(sec);


        playerGO.GetComponent<NavMeshAgent>().Warp(oldLocalPosition);
        playerGO.transform.parent = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().TarghetRoom.nomeStanza).transform;
        playerGO.GetComponent<NavMeshAgent>().enabled = true;
        playerGO.transform.position = oldLocalPosition;

        playerGO.GetComponent<PlayerMovement>().currentRoom = playerGO.GetComponent<PlayerMovement>().TarghetRoom;
        //playerGO.GetComponent<NavMeshAgent>().destination = playerGO.transform.position;
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = false;

        Camera.main.transform.parent = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().TarghetRoom.nomeStanza).transform;
        Camera.main.transform.localPosition = new Vector3(-2, 31.87f, 0);
        yield return new WaitForSeconds(1);
        playerGO.transform.GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

    }

    public Vector3 CenterOfVectors(List<Vector3> vectors)
    {
        Vector3 sum = Vector3.zero;
        if (vectors == null || vectors.Count == 0)
        {
            return sum;
        }

        foreach (Vector3 vec in vectors)
        {
            sum += vec;
        }
        return sum / vectors.Count;
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public string GetPath(Room Stanza)
    {
        string pathCalcolato = "";
        Room stanzaCorrente = Stanza;

        while (stanzaCorrente.nomeStanza != "/")
        {
            pathCalcolato = "/" + stanzaCorrente.nomeStanza + pathCalcolato;
            stanzaCorrente = stanzaCorrente.parentRoom;
        }

        if (pathCalcolato == "")
        {
            pathCalcolato = "/";
        }

        return pathCalcolato;
    }

    public void AddQuest(string testo)
    {
        
            GameObject newQuest = Instantiate(gameObject.GetComponent<ObjectPrefabSelector>().setQuest) as GameObject;
            newQuest.transform.SetParent(listOfQuests.transform, false);
            newQuest.GetComponentInChildren<Text>().text = testo;
            addedQuests.Add(newQuest);
            logWriter.QuestAddedToLog(testo.Substring(0, 20));
        
    }

    public void RemoveQuest(string questText)
    {
        if(addedQuests.Exists(x => x.GetComponentInChildren<Text>().text.Contains(questText)))
        {
            GameObject questToRemove = addedQuests.Find(x => x.GetComponentInChildren<Text>().text.Contains(questText));
            //addedQuests.Remove(questToRemove);
            questToRemove.SetActive(false);
            logWriter.QuestRemovedToLog(questText.Substring(0, 20));
        }
    }
}
