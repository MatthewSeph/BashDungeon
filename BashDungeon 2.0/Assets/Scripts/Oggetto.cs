using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Oggetto
{

    private Room currentRoom;
    public string nomeOggetto;
    bool isTar, isZip, isInvisible, isMovable, isTxt, isRemovable, isNPC;
    string testoTxT;
    bool isActive = true;

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

            if (currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count >= 1)
            {
                this.nomeOggetto = currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count + nomeOggetto;
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

    public bool IsActive
    {
        get
        {
            return isActive;
        }

        set
        {
            CurrentRoom = CurrentRoom;
            isActive = value;
        }
    }

    public bool IsRemovable
    {
        get
        {
            return isRemovable;
        }

        set
        {
            isRemovable = value;
        }
    }

    public bool IsTxt
    {
        get
        {
            return isTxt;
        }

        set
        {
            isTxt = value;
        }
    }

    public string TestoTxT
    {
        get
        {
            return testoTxT;
        }

        set
        {
            testoTxT = value;
        }
    }

    public bool IsNPC
    {
        get
        {
            return isNPC;
        }

        set
        {
            isNPC = value;
        }
    }

    public Oggetto(Room currentRoom, string nomeOggetto)
    {

        this.currentRoom = currentRoom;
        if (currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count >= 1)
        {
            this.nomeOggetto = currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count + nomeOggetto;
        }
        else
        {
            this.nomeOggetto = nomeOggetto;
        }
    }

}
