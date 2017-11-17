using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oggetto {

    public Room currentRoom;
    public string nomeOggetto;
    bool isTar, isZip, isInvisible, isMovable, isTxt;
    string textoTxt;


    public Oggetto(Room currentRoom, string nomeOggetto)
    {
        this.currentRoom = currentRoom;
        this.nomeOggetto = nomeOggetto;
    }

}
