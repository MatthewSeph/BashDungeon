using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirGiorgio : MonoBehaviour {

    GameObject gameManager;
    GameObject playerGO;
    GameObject consoleView;
   // public GameObject dialogueView;
    Oggetto sirGiorgio;

    bool hasPlayerClickedLs = false;
    bool hasPlayerClickedCd = false;

    bool hasPlayerEndedSecondText = false;

    bool endTutorial = false;
    string secondoTesto = "Queste sono solo alcune pergamene di questo labirinto, ti insegnano dei comandi utili che potrai usare nella console affianco.\nCi sono anche delle pergamene che ti danno dei consigli utili per risolvere degli enigmi, tienile a mente.\nBuona fortuna!";
    string defaultText;
    bool lastQuest = false;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<PlayManager>().ClickedObject = gameObject;
        playerGO = GameObject.Find("Player");
        consoleView = GameObject.Find("ConsoleView");
        playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;

        consoleView.transform.GetChild(0).gameObject.SetActive(false);
        sirGiorgio = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == "SirGiorgioNPC");
    }
	
	// Update is called once per frame
	void Update () {



        if (hasPlayerClickedLs && hasPlayerClickedCd && !endTutorial)
        {
            if (hasPlayerEndedSecondText && gameManager.GetComponent<PlayManager>().ClickedObject != gameObject && !playerGO.GetComponent<PlayerMovement>().BlockedMovement)
            {
                endTutorial = true;
            }
            if (!hasPlayerEndedSecondText && !endTutorial && !playerGO.GetComponent<PlayerMovement>().BlockedMovement)
            {
                gameManager.GetComponent<PlayManager>().ClickedObject = gameObject;
                playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
                sirGiorgio.TestoTxT = secondoTesto;
                consoleView.transform.GetChild(0).gameObject.SetActive(true);
                hasPlayerEndedSecondText = true;
            }


        }

        if (hasPlayerEndedSecondText && endTutorial)
        {
            gameManager.GetComponent<PlayManager>().SetTutorialPanelOff();
            sirGiorgio.TestoTxT = "Come faccio a non annoiarmi qui dentro?\nVa bene te lo dirò, ma promettimi che non riderai!\n..Come hobby aggiusto la carta..\n...\nNon stai ridendo?! Sei il primo! Portami qualche foglio rotto, te li aggiusterò GRATIS, sconto amico!";
            if(sirGiorgio.CurrentRoom.oggetti.Find(x => x.nomeOggetto.Contains("frammentoPergamena")) != null )
            {
                sirGiorgio.TestoTxT = "Vedo che ti stai dando da fare!\nPorta i restanti pezzi e la tua pergamena tornerà come nuova!";

                if (sirGiorgio.CurrentRoom.nomeStanza == "/" && sirGiorgio.CurrentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains("frammentoPergamena")).Count == gameManager.GetComponent<LevelGeneration>().LootPrefabs.Count)
                {
                    if (lastQuest && gameManager.GetComponent<PlayManager>().ClickedObject != gameObject)
                    {
                        foreach(Oggetto o in sirGiorgio.CurrentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains("frammentoPergamena")))
                        {
                            GameObject.Find("//"+ o.nomeOggetto);
                            sirGiorgio.CurrentRoom.oggetti.Remove(o);
                        }          
                    }
                    sirGiorgio.TestoTxT = "Bene bene! Sembra che tutti i pezzi siano qui\nTi auguro una buona lettura, ma sopratutto...\nAMMIRA LA MIA BRAVURA!!";
                    gameManager.GetComponent<PlayManager>().ClickedObject = gameObject;
                    lastQuest = true;
                }

                
            }
        }
		if(gameManager.GetComponent<PlayManager>().ClickedObject!= null && gameManager.GetComponent<PlayManager>().ClickedObject.name == "pergamenaLs" && !hasPlayerClickedLs)
        {
            playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
            hasPlayerClickedLs = true;
        }
        else if (gameManager.GetComponent<PlayManager>().ClickedObject != null && gameManager.GetComponent<PlayManager>().ClickedObject.name == "pergamenaCd" && !hasPlayerClickedCd)
        {
            playerGO.GetComponent<PlayerMovement>().BlockedMovement = true;
            hasPlayerClickedCd = true;
        }
        else if(gameManager.GetComponent<PlayManager>().ClickedObject == null || gameManager.GetComponent<PlayManager>().ClickedObject.name != "SirGiorgioNPC" && (!hasPlayerClickedLs || !hasPlayerClickedCd))
        {
            sirGiorgio.TestoTxT = "Su su, Sgranchisciti le gambe andando a leggere quelle pergamene!";
        }

        
    }

    public void SetTutorialBools()
    {
        hasPlayerClickedCd = true;
        hasPlayerClickedLs = true;
        hasPlayerEndedSecondText = true;
        endTutorial = true;
        consoleView.transform.GetChild(0).gameObject.SetActive(true);
    }
}
