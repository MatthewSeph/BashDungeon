using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ConsoleScript : MonoBehaviour {

    public string messaggio;
    public Text textObj;
    public GameObject playerGO;
    public GameObject gameManager;
    List<String> oldMessages = new List<string>();
    int contatoreOldMessaggi = 0;
    GameObject consoleText;
    //GameObject consoleCanvas;

    int righeMax = 14;

    void Start()
    {
        consoleText = GameObject.Find("ConsoleText");
        playerGO = GameObject.Find("Player");
        //consoleCanvas = GameObject.Find("ConsoleCanvas");
        gameManager = GameObject.Find("GameManager");

        oldMessages.Insert(0, "");

        textObj = consoleText.GetComponent<Text>();
    }

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                
                if (messaggio.Length != 0)
                {
                    messaggio = messaggio.Substring(0, messaggio.Length - 1);

                    if (textObj.text.Length != 0)
                    {
                        textObj.text = textObj.text.Substring(0, textObj.text.Length - 1);

                    }
                }
            }
            else if ((c == '\n') || (c == '\r'))
            {
                textObj.text += "\n";
                
				SplitMessage(messaggio);
                oldMessages.Insert(1, messaggio);
                messaggio = "";
                textObj.text += "User@linux:~$ ";

            }

            else
            {
                textObj.text += c;
                messaggio += c;
            }

            CancellatoreDiRiga();
        }

        //Se premo freccia su voglio usare l ultimo commando inserito
        if (Input.GetKeyUp(KeyCode.UpArrow) && oldMessages.Count > contatoreOldMessaggi + 1 )
        {
            textObj.text = textObj.text.Substring(0, textObj.text.Length - messaggio.Length);
            contatoreOldMessaggi += 1;
            messaggio = oldMessages[contatoreOldMessaggi];
            textObj.text += messaggio;
        }

        //torno indietro di una posizione nella lista dei commandi
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            textObj.text = textObj.text.Substring(0, textObj.text.Length - messaggio.Length);
            if (contatoreOldMessaggi > 0)
            {
                contatoreOldMessaggi -= 1;
            }
            messaggio = oldMessages[contatoreOldMessaggi];
            textObj.text += messaggio;
        }

        //tengo la lista dei comandi a max 25 messaggi
        if(oldMessages.Count > 25)
        {
            oldMessages.RemoveAt(oldMessages.Count - 1);
        }

        CancellatoreDiRiga();
    }

    void CancellatoreDiRiga()
    {
        if (textObj.cachedTextGenerator.lineCount > righeMax)
        {
            int valore;

            valore = textObj.cachedTextGenerator.lines[1].startCharIdx;
            textObj.text = textObj.text.Substring(valore, textObj.text.Length - valore);


        }
    }


    void SplitMessage (string message)
    {
        string[] splittedMessage;

        splittedMessage = message.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        if (splittedMessage.Length != 0)
        {
            ControlloMessaggio(splittedMessage);
        }
        
    }


    void ControlloMessaggio(string[] splittedMessage)
    {

        string comando = splittedMessage[0];

        switch (comando)
        {
			case "pwd":
				Pwd (splittedMessage);
                break;


			case "cd":
				Cd (splittedMessage);
                break;

            case "ls":
                Ls(splittedMessage);
                break;

            case "mv":
                Mv(splittedMessage);
                break;

            default:
				textObj.text += (splittedMessage[0] + " non e' un comando riconosciuto." + "\n");
				break;
        }

    }

	bool CheckPath(string pathToCheck) 
	{
		bool isPathCorrect = true;
        if (pathToCheck.ToCharArray().First() == '/')
        {


            string[] path = pathToCheck.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (path.Length >= 2)
            {
                for (int i = 0; i <= path.Length - 1; i++)
                {

                    if (i == path.Length - 1)
                    {
                        break;
                    }
                    else if (!(gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[i]).childrenRooms.Exists(x => x.nomeStanza == path[i + 1])))
                    {
                        isPathCorrect = false;
                        break;
                    }

                }
            }
            else if (path.Length == 1)
            {
                if (!(gameManager.GetComponent<LevelGeneration>().GetRoomByName("/").childrenRooms.Exists(x => x.nomeStanza == path[0])))
                {
                    isPathCorrect = false;

                }
            }
            else
            {
                isPathCorrect = false;
            }

        }
        else
        {
            isPathCorrect = false;
        }

        return isPathCorrect;
	}


	void Pwd(string[] splittedMessage)
	{
		if (splittedMessage.Length==1)
		{
			textObj.text += playerGO.transform.parent.name + "\n";
		}

		else
		{
			textObj.text += ("pwd non prevede parametri." + "\n");
		}
	}

	void Cd(string[] splittedMessage) 
	{
		if (splittedMessage.Length == 2)
		{
			if ((playerGO.GetComponent<PlayerMovement> ().currentRoom.childrenRooms.Exists (x => x.nomeStanza == splittedMessage [1]))) 
			{
                gameManager.GetComponent<PlayManager>().MoveBeforeChangeRoom(gameManager.GetComponent<LevelGeneration>().GetRoomByName(splittedMessage[1]));
                
            } 
			else if ((splittedMessage [1] == "..") && (playerGO.GetComponent<PlayerMovement> ().currentRoom.nomeStanza != "/")) 
			{
                gameManager.GetComponent<PlayManager>().MoveBeforeChangeRoom(gameManager.GetComponent<LevelGeneration>().GetRoomByName(playerGO.GetComponent<PlayerMovement>().currentRoom.parentRoom.nomeStanza));
             
            }
			else if ((splittedMessage [1] == "/")) 
			{
                gameManager.GetComponent<PlayManager>().MoveBeforeChangeRoom(gameManager.GetComponent<LevelGeneration>().GetRoomByName("/"));

            }
			else if (CheckPath (splittedMessage[1])) 
			{
				string[] path = splittedMessage[1].Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                gameManager.GetComponent<PlayManager>().MoveBeforeChangeRoom(gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[path.Length - 1]));

            }
			else 
			{
				textObj.text += (splittedMessage[1] + " non è un path corretto." + "\n");
			}
		}
		else
		{
			textObj.text += ("cd prevede un parametro." + "\n");
		}
	}

    void Ls(String[] splittedMessage)
    {
        if(splittedMessage.Length == 1)
        {

            if (playerGO.GetComponent<PlayerMovement>().currentRoom.childrenRooms != null)
            {
                foreach (Room r in playerGO.GetComponent<PlayerMovement>().currentRoom.childrenRooms)
                {
                    textObj.text += ("<color=#7F9FCDFF>" + r.nomeStanza + "</color>\n");
                }
            }

            if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti != null)
            {
                foreach (Oggetto o in playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti)
                {
                    if (!o.IsInvisible)
                    {
                        textObj.text += (o.nomeOggetto + "\n");
                    }
                }
            }

        }
        else if(splittedMessage.Length == 2 && splittedMessage[1] == "-a")
        {
            if (playerGO.GetComponent<PlayerMovement>().currentRoom.childrenRooms != null)
            {
                foreach (Room r in playerGO.GetComponent<PlayerMovement>().currentRoom.childrenRooms)
                {
                    textObj.text += ("<color=#7F9FCDFF>" + r.nomeStanza + "</color>\n");
                }
            }

            if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti != null)
            {
                foreach (Oggetto o in playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti)
                {
                    textObj.text += (o.nomeOggetto + "\n");

                    if (o.IsInvisible)
                    {
                        GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + o.nomeOggetto).GetComponent<ObjectBehavior>().isMadeVisible = true;
                    }
                }
            }
        }
        else
        {
            textObj.text += ("ls non prevede parametri." + "\n");
        }
    }

    void Mv(String[] splittedMessage)
    {
        if(splittedMessage.Length == 3)
        {

            if(playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Exists(x => x.nomeOggetto == splittedMessage[1]))
            {
                if (playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]).IsMovable)
                {
                    if ((splittedMessage[2] == "/"))
                    {
                        GameObject selectedObj;
                        Vector3 oldLocalPos;
                        Oggetto selectedOggetto;

                        selectedObj = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[1]);
                        oldLocalPos = selectedObj.transform.localPosition;
                        selectedOggetto = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]);

                        selectedOggetto.CurrentRoom = gameManager.GetComponent<LevelGeneration>().GetRoomByName(splittedMessage[2]); //cambio currentRoom in oggetto
                        playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Remove(selectedOggetto); // lo elimino dalla sua vecchia stanza
                        gameManager.GetComponent<LevelGeneration>().GetRoomByName(splittedMessage[2]).oggetti.Add(selectedOggetto); // lo aggiungo tra gli oggetti della nuova stanza

                        selectedObj.transform.parent = GameObject.Find("/" + splittedMessage[2]).transform;
                        selectedObj.transform.localPosition = oldLocalPos;
                        selectedObj.name = selectedOggetto.nomeOggetto;

                    }
                    else if (CheckPath(splittedMessage[2]))
                    {
                        string[] path = splittedMessage[2].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        GameObject selectedObj;
                        Vector3 oldLocalPos;
                        Oggetto selectedOggetto;


                        selectedObj = GameObject.Find("/" + playerGO.GetComponent<PlayerMovement>().currentRoom.nomeStanza + "/" + splittedMessage[1]);
                        oldLocalPos = selectedObj.transform.localPosition;
                        selectedOggetto = playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Find(x => x.nomeOggetto == splittedMessage[1]);

                        selectedOggetto.CurrentRoom = gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[path.Length - 1]); //cambio currentRoom in oggetto
                        playerGO.GetComponent<PlayerMovement>().currentRoom.oggetti.Remove(selectedOggetto); // lo elimino dalla sua vecchia stanza
                        gameManager.GetComponent<LevelGeneration>().GetRoomByName(path[path.Length - 1]).oggetti.Add(selectedOggetto); // lo aggiungo tra gli oggetti della nuova stanza

                        selectedObj.transform.parent = GameObject.Find("/" + path[path.Length - 1]).transform;
                        selectedObj.transform.localPosition = oldLocalPos;
                        selectedObj.name = selectedOggetto.nomeOggetto;

                    }
                    else
                    {
                        textObj.text += (splittedMessage[2] + " non è un path corretto." + "\n");
                    }
                }
                else
                {
                    textObj.text += (splittedMessage[1] + " non è spostabile." + "\n");
                }
            }

            else
            {
                textObj.text += ("Non è presente nessun oggetto col nome di " + splittedMessage[1] + "in questa stanza :o\n");
            }

        }
        else
        {
            textObj.text += ("mv prevede due parametri." + "\n");
        }
    }

}
