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
    GameObject consoleCanvas;

    int righeMax = 14;

    void Start()
    {
        consoleText = GameObject.Find("ConsoleText");
        playerGO = GameObject.Find("Player");
        consoleCanvas = GameObject.Find("ConsoleCanvas");

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
                // textObj.text += "Il messaggio scritto era: " + messaggio + "\n";
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
        string[] splitMess;

        splitMess = message.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

        Debug.Log("Messaggio: " + splitMess[0].ToCharArray(0 , 1)[0]);

        ControlloMessaggio(splitMess);
    }


    void ControlloMessaggio(string[] splitMess)
    {

        string comando = splitMess[0];

        switch (comando)
        {
            case "pwd":
                if (splitMess.Length<=1)
                {
                    textObj.text += GameObject.Find("Player").transform.parent.name + "\n";
                }

                else
                {
                    textObj.text += ("pwd non prevede parametri" + "\n");
                }
                break;


            case "cd":
                if ((splitMess.Length == 2) && (GameObject.Find("Player").GetComponent<PlayerMovement>().currentRoom.childrenRooms.Exists(x => x.nomeStanza == splitMess[1])))
                {
                    GameObject.Find("Player").transform.parent = (GameObject.Find("/" + splitMess[1]).transform);
                    Debug.Log(GameObject.Find("Player").transform.parent.name);
                    
                }
                else
                {
                    textObj.text += ("cd prevede un parametro" + "\n");
                }

                break;
        }

    }
}
