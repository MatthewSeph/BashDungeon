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

    GameObject consoleText;
    //GameObject consoleCanvas;

    int righeMax = 14;

    void Start()
    {
        consoleText = GameObject.Find("ConsoleText");
        playerGO = GameObject.Find("Player");
        //consoleCanvas = GameObject.Find("ConsoleCanvas");

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

        ControlloMessaggio(splittedMessage);
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

			default:
				textObj.text += ("NON LO FATE" + "\n");
				break;
        }

    }


	void Pwd(string[] splittedMessage)
	{
		if (splittedMessage.Length<=1)
		{
			textObj.text += playerGO.transform.parent.name + "\n";
		}

		else
		{
			textObj.text += ("pwd non prevede parametri" + "\n");
		}
	}

	void Cd(string[] splittedMessage) 
	{
		if (splittedMessage.Length == 2)
		{
			if ((playerGO.GetComponent<PlayerMovement> ().currentRoom.childrenRooms.Exists (x => x.nomeStanza == splittedMessage [1]))) 
			{
				playerGO.transform.parent = (GameObject.Find ("/" + splittedMessage [1]).transform);
				Debug.Log (GameObject.Find ("Player").transform.parent.name);
			} 
			else 
			{
				textObj.text += (splittedMessage[1] + " non è un path corretto" + "\n");
			}
		}
		else
		{
			textObj.text += ("cd prevede un parametro" + "\n");
		}
	}

}
