using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LogWriter : MonoBehaviour {

    string path;

    string pergamena = "scroll read";
    string addedQuest = "quest added";
    string removedQuest = "quest removed";

    // Use this for initialization
    void Start () {
        path = Application.dataPath + "/Log.txt";
        CreateText();
	}
	
	void CreateText()
    {

        if(!File.Exists(path))
        {
            File.WriteAllText(path, "BASH DUGEON - LOGS\n\n");
        }
        else
        {
            File.AppendAllText(path, "\n\nBASH DUGEON - LOGS\n\n");
        }
    }

    public void ScrollReadToLog(string content)
    {
        File.AppendAllText(path, System.DateTime.Now + " - " + pergamena + " - " + "\"" + content + "...\"" + "\n");
    }

    public void QuestAddedToLog(string content)
    {
        File.AppendAllText(path, System.DateTime.Now + " - " + addedQuest + " - " + "\"" + content + "...\"" + "\n");
    }

    public void QuestRemovedToLog(string content)
    {
        File.AppendAllText(path, System.DateTime.Now + " - " + removedQuest + " - " + "\"" + content + "...\"" + "\n");
    }

}
