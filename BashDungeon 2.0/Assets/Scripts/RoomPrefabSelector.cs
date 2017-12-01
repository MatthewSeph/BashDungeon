using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefabSelector : MonoBehaviour
{

    public GameObject roomU, roomD, roomR, roomL,
                        roomUD, roomRL, roomUR, roomUL, roomDR, roomDL,
                        roomULD, roomRUL, roomDRU, roomLDR, roomUDRL;

    public int type; // 0: normal, 1: enter

    public GameObject PickPrefab(bool up, bool right, bool down, bool left)
    {

        if (up)
        {
            if (down)
            {
                if (right)
                {
                    if (left)
                    {
                        return roomUDRL;

                    }
                    else
                    {
                        return roomDRU;

                    }
                }
                else if (left)
                {
                    return roomULD;

                }
                else
                {
                    return roomUD;

                }
            }
            else
            {
                if (right)
                {
                    if (left)
                    {
                        return roomRUL;

                    }
                    else
                    {
                        return roomUR;

                    }
                }
                else if (left)
                {
                    return roomUL;

                }
                else
                {
                    return roomU;

                }
            }
        }
        if (down)
        {
            if (right)
            {
                if (left)
                {
                    return roomLDR;

                }
                else
                {
                    return roomDR;

                }
            }
            else if (left)
            {
                return roomDL;

            }
            else
            {
                return roomD;

            }
        }
        if (right)
        {
            if (left)
            {
                return roomRL;

            }
            else
            {
                return roomR;

            }
        }
        else
        {
            return roomL;

        }
    }

}