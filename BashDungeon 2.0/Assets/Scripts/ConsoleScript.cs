using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConsoleScript : MonoBehaviour {

    public string messaggio;
    public Text textObj;

    GameObject consoleText;
    GameObject consoleCanvas;

    int righeMax = 14;

    void Start()
    {
        consoleText = GameObject.Find("ConsoleText");
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
                textObj.text += "Il messaggio scritto era: " + messaggio + "\n";
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





}
