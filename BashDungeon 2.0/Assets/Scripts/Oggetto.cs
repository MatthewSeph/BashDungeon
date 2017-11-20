using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Oggetto {

    private Room currentRoom;
    public string nomeOggetto;
    bool isTar, isZip, isInvisible, isMovable, isTxt;
    string textoTxt;

    public Room CurrentRoom
    {
        get
        {
            return currentRoom;
        }

        set
        {
            this.nomeOggetto = Regex.Replace(this.nomeOggetto, "[0-9]", "");
            currentRoom = value;

            if (currentRoom.oggetti.FindAll(x => x.nomeOggetto.StartsWith(nomeOggetto)).Count >= 1)
            {
                this.nomeOggetto = nomeOggetto + currentRoom.oggetti.FindAll(x => x.nomeOggetto.StartsWith(nomeOggetto)).Count;
            }
        }
    }

    public bool IsMovable
    {
        get
        {
            return isMovable;
        }

        set
        {
            isMovable = value;
        }
    }

    public bool IsInvisible
    {
        get
        {
            return isInvisible;
        }

        set
        {
            isInvisible = value;
            this.nomeOggetto = "." + this.nomeOggetto;
        }
    }

    public Oggetto(Room currentRoom, string nomeOggetto)
    {
        
        this.currentRoom = currentRoom;
        if(currentRoom.oggetti.FindAll(x => x.nomeOggetto.StartsWith(nomeOggetto)).Count >= 1)
        {
            this.nomeOggetto = nomeOggetto + currentRoom.oggetti.FindAll(x => x.nomeOggetto.StartsWith(nomeOggetto)).Count;
        }
       else
        {
            this.nomeOggetto = nomeOggetto;
        }
    }

}
