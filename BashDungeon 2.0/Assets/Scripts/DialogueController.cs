using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour {

    string[] dialogueMessages;
    int dialogueCount;
    GameObject gameManager;

    public GameObject dialogueButtonText;

    public string[] DialogueMessages
    {
        get
        {
            return dialogueMessages;
        }

        set
        {
            dialogueMessages = value;
        }
    }

    // Update is called once per frame
    void Start () {
        gameManager = GameObject.Find("GameManager");
    }

    public void SetText(string fullText)
    {
        dialogueMessages = fullText.Split('\n');
        gameObject.GetComponent<Text>().text = dialogueMessages[0];
        dialogueCount = 0;
        ButtonTextManage();
        
    }

    public void ButtonNext()
    {
        if(DialogueMessages.Length-1 > dialogueCount)
        {
            
            dialogueCount++;
            gameObject.GetComponent<Text>().text = dialogueMessages[dialogueCount];
            ButtonTextManage();

        }
        else
        {
            gameManager.GetComponent<PlayManager>().OnCloseDialogues();
        }
    }

    public void ButtonTextManage()
    {

        if (dialogueCount == DialogueMessages.Length - 1)
        {
            dialogueButtonText.GetComponent<Text>().text = "OK!";
        }
        else
        {
            dialogueButtonText.GetComponent<Text>().text = "AVANTI";
        }
    }

}
