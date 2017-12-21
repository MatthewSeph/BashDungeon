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
    bool hasBeenRead = false;
    bool isActive = true;

    bool canCF = false, canXF = false, canZCF = false, canZXF = false;

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
                if (nomeOggetto.EndsWith(".tar"))
                {
                    this.nomeOggetto = nomeOggetto.Replace(".tar", currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count + ".tar");
                }
                else if (nomeOggetto.EndsWith(".gz"))
                {
                    this.nomeOggetto = nomeOggetto.Replace(".tar", currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count + ".tar");
                }
                else
                {
                    this.nomeOggetto = nomeOggetto + currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count;
                }

                Debug.Log(this.nomeOggetto);
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
            //CurrentRoom = currentRoom;
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

    public bool IsZip
    {
        get
        {
            return isZip;
        }

        set
        {
            isZip = value;
        }
    }

    public bool IsTar
    {
        get
        {
            return isTar;
        }

        set
        {
            if(nomeOggetto.EndsWith(".tar"))
            {
                CanXF = true;
            }
            else if(nomeOggetto.EndsWith(".tar.gz"))
            {
                CanZXF = true;
            }
                
            isTar = value;
        }
    }

    public bool CanCF
    {
        get
        {
            return canCF;
        }

        set
        {
            canCF = value;
        }
    }

    public bool CanZCF
    {
        get
        {
            return canZCF;
        }

        set
        {
            canZCF = value;
        }
    }

    public bool CanZXF
    {
        get
        {
            return canZXF;
        }

        set
        {
            canZXF = value;
        }
    }

    public bool CanXF
    {
        get
        {
            return canXF;
        }

        set
        {
            canXF = value;
        }
    }

    public bool HasBeenRead
    {
        get
        {
            return hasBeenRead;
        }

        set
        {
            hasBeenRead = value;
        }
    }

    public Oggetto(Room currentRoom, string nomeOggetto)
    {

        this.currentRoom = currentRoom;
        if (currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count >= 1)
        {
            if (nomeOggetto.EndsWith(".tar"))
            {
                this.nomeOggetto = nomeOggetto.Replace(".tar", currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count + ".tar");
            }
            else if (nomeOggetto.EndsWith(".gz"))
            {
                this.nomeOggetto = nomeOggetto.Replace(".tar", currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count + ".tar");
            }
            else
            {
                this.nomeOggetto = nomeOggetto + currentRoom.oggetti.FindAll(x => x.nomeOggetto.Contains(nomeOggetto) && x.IsActive).Count;
            }
        }
        else
        {
            this.nomeOggetto = nomeOggetto;
        }
    }

}
