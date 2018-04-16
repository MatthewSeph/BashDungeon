using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelGeneration : MonoBehaviour
{
    Vector2 worldSize = new Vector2(5, 5);

    Room[,] rooms;
    List<Vector2> takenPositions = new List<Vector2>();
    int gridSizeX, gridSizeY;
    [SerializeField] int numberOfRooms = 22;
    List<Room> roomsOrderByDistance = new List<Room>();
    List<Room> roomsWithNoChildren = new List<Room>();
    public GameObject player;
    public List<Oggetto> oggettiCreati = new List<Oggetto>();
    List<Room> levelRooms = new List<Room>();

    public List<GameObject> LootPrefabs = new List<GameObject>();
    public GameObject rootPrefab;

    public TextAsset xmlPergamene;
    string dataToParse;
    

    void Start()
    {
        player = GameObject.Find("Player");

        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(worldSize.x);
        gridSizeY = Mathf.RoundToInt(worldSize.y);


        CreateRooms();
        Debug.Log("created rooms");
        SetRoomDoors();
        Debug.Log("door set");
        SetDistances();
        Debug.Log("distance set");
        RoomsOrderByDistance();
        Debug.Log("room ordered by distance");
        CheckRoomWithNoChildrenSorted();
        Debug.Log("rooms no children sorted");
        DrawMap();
        Debug.Log("map drawed");
        SetLevelTypeRooms();
        Debug.Log("rooms type set");
        OggettiNelleStanze();
        Debug.Log("spawn oggetti");

        dataToParse = xmlPergamene.text;
        SpawnRandomPergamene(dataToParse);
        Debug.Log("spawn pergamene");

        SpawnOggetti();
        Debug.Log("spawn oggetti");
        SpawnPlayer();
        Debug.Log("spawn player");

    }
    void CreateRooms()
    {
        //setup
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1);
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;
        //magic numbers
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        //add rooms
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
            //grab new position
            checkPos = NewPosition();
            //test new position
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
                if (iterations >= 50)
                    print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
            }
            //finalize position
            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 0);
            takenPositions.Insert(i + 1, checkPos);

        }
    }
    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;
    }
    Vector2 SelectiveNewPosition()
    {
        int index = 0, inc = 0;
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            inc = 0;
            do
            {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        if (inc >= 100)
        {
            print("Error: could not find position with only one neighbor");
        }
        return checkingPos;
    }

    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0;
        if (usedPositions.Contains(checkingPos + Vector2.right))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.left))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.up))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.down))
        {
            ret++;
        }
        return ret;
    }

    void DrawMap()
    {
        foreach (Room room in roomsOrderByDistance)
        {

            Vector2 drawPos = room.gridPos;
            drawPos.x *= 24;
            drawPos.y *= 24;
            Vector3 roomPosition;
            GameObject selectedPrefab = gameObject.GetComponent<RoomPrefabSelector>().PickPrefab(room.doorTop, room.doorRight, room.doorBot, room.doorLeft);

            GameObject stanza = Instantiate(selectedPrefab) as GameObject;

            roomPosition.x = drawPos.x;
            roomPosition.y = 0;
            roomPosition.z = drawPos.y;
            stanza.transform.position = roomPosition;
            stanza.name = room.nomeStanza;

        }
    }

    void SetRoomDoors()
    {

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            //Se la stanza adiacente esiste e non ha un Parent allora esso gli viene settato
            if (takenPositions.Contains(takenPositions[i] + Vector2.up))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY].GetParentRoom() == null) && (rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

                    rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY].childrenRooms.Add(rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY].doorBot = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorTop = true;
                }
            }

            if (takenPositions.Contains(takenPositions[i] + Vector2.right))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].GetParentRoom() == null) && (rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

                    rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY].childrenRooms.Insert(0, rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].doorLeft = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorRight = true;
                }
            }

            if (takenPositions.Contains(takenPositions[i] + Vector2.down))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].GetParentRoom() == null) && (rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

                    rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY].childrenRooms.Insert(0, rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].doorTop = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorBot = true;
                }
            }

            if (takenPositions.Contains(takenPositions[i] + Vector2.left))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].GetParentRoom() == null) && (rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

                    rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY].childrenRooms.Insert(0, rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].doorRight = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorLeft = true;
                }
            }

        }

    }


    void SetDistances()
    {
        int distance;
        Room currentRoom;

        foreach (Room room in rooms)
        {
            if (room == null)
            {
                continue;
            }

            currentRoom = room;
            distance = 0;

            while (currentRoom.type != 1)
            {
                currentRoom = currentRoom.GetParentRoom();

                distance++;
            }

            room.distance = distance;
        }
    }

    void RoomsOrderByDistance()
    {
        foreach (Room room in rooms)
        {
            if (room == null)
            {
                continue;
            }

            roomsOrderByDistance.Insert(0, room);
        }

        roomsOrderByDistance.Sort(SortByDistance);
    }

    void CheckRoomWithNoChildrenSorted()
    {
        foreach (Room room in roomsOrderByDistance)
        {
            if (room.childrenRooms.Count == 0)
            {
                roomsWithNoChildren.Add(room);

            }
        }
        roomsWithNoChildren.Sort(SortByDistance);

    }


    static int SortByDistance(Room r1, Room r2)
    {
        return r2.distance.CompareTo(r1.distance);
    }

    void SetLevelTypeRooms()
    {

        //se ho abbastanza stanze terminali setto le stanze con loot, altrimenti rigenero il dungeon
        if (!(roomsWithNoChildren.Count <= LootPrefabs.Count))
        { 
            Vector3 lootPosition = Vector3.zero;


            for (int i = 0; i < roomsWithNoChildren.Count; i++)
            {
                if (levelRooms.Count == LootPrefabs.Count)
                {
                    break;
                }

                if (!levelRooms.Contains(roomsWithNoChildren[i].parentRoom) && roomsWithNoChildren[i].parentRoom.type != 1 && levelRooms.Count < LootPrefabs.Count)
                {
                    Debug.Log("stanza livello " + roomsWithNoChildren[i].parentRoom.gridPos);
                    Debug.Log("stanza loot " + roomsWithNoChildren[i].gridPos);
                    levelRooms.Add(roomsWithNoChildren[i].parentRoom);
                    roomsWithNoChildren[i].parentRoom.type = 2;
                    roomsWithNoChildren[i].type = 3;
                }
            }
            if (levelRooms.Count < LootPrefabs.Count)
            {
                Debug.Log("Dungeon rigenerato per mancanza di ''stanze terminali'' utili");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            Debug.Log("Dungeon rigenerato per mancanza di ''stanze terminali''");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OggettiNelleStanze()
    {
        int levelDifficoulty = 0;
        int maxLevelDifficoulty = LootPrefabs.Count;
        int numeroFrammentoPergamena = 0;
        // Dovremo controllare la lista levelRooms e a seconda del tipo e della difficoltà del livello creare oggetti adeguati
        foreach(Room room in roomsOrderByDistance)
        {
            Transform[] allChildren;
            GameObject chosenPrefab;
            
            if (room.type == 1)
            {
                chosenPrefab = rootPrefab;
                
            }
            else if(room.type == 2)
            {
                chosenPrefab = gameObject.GetComponent<ObjectPrefabSelector>().PickLevelPrefab(levelDifficoulty);
                Debug.Log("tipi settati");
            }
            else if(room.type == 0)
            {
                chosenPrefab = gameObject.GetComponent<ObjectPrefabSelector>().PickStandardRoomPrefab();
                Debug.Log("tipi settati");
            }
            else if(room.type == 3)
            {
                chosenPrefab = gameObject.GetComponent<ObjectPrefabSelector>().PickLootPrefab(numeroFrammentoPergamena);
                numeroFrammentoPergamena++;
                Debug.Log("tipi settati");
            }
            else
            {
                continue;
            }

           

            GameObject prefabInstanziata = Instantiate(chosenPrefab) as GameObject;
            prefabInstanziata.transform.parent = GameObject.Find("/" + room.nomeStanza).transform;
            prefabInstanziata.transform.localPosition = prefabInstanziata.transform.position;

            if (room.type == 2)
            {
                Room lootRoom = null;

                foreach(Room childrenRoom in room.childrenRooms)
                {
                    if(childrenRoom.type == 3)
                    {
                        lootRoom = childrenRoom;
                        
                    }

                }

                if (lootRoom != null)
                {
                    Vector2 rotationDirection = gameObject.GetComponent<PlayManager>().RoomDirection(room, lootRoom);
                    if(rotationDirection == Vector2.right)
                    {
                        prefabInstanziata.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    else if(rotationDirection == Vector2.down)
                    {
                        prefabInstanziata.transform.Rotate(new Vector3(0, 180, 0));
                    }
                    else if (rotationDirection == Vector2.left)
                    {
                        prefabInstanziata.transform.Rotate(new Vector3(0, 270, 0));
                    }

                }
            }
            else if(room.type == 0)
            {
                int randomHelper = Random.Range(0, 3);
                switch (randomHelper)
                {
                    case 0: prefabInstanziata.transform.Rotate(new Vector3(0, 90, 0));
                        break;
                    case 1: prefabInstanziata.transform.Rotate(new Vector3(0, 180, 0));
                        break;
                    case 2: prefabInstanziata.transform.Rotate(new Vector3(0, 270, 0));
                        break;
                }
            }
            allChildren = prefabInstanziata.GetComponentsInChildren<Transform>(); //prendo tutti gli oggetti

            foreach (Transform child in allChildren)
            {
                if (!child.name.Contains("Prefab") && child.parent.name.Contains("Prefab"))
                {
                    child.transform.parent = GameObject.Find("/" + room.nomeStanza).transform;
                    Oggetto item = new Oggetto(GetRoomByName(prefabInstanziata.transform.parent.name), Regex.Replace(child.name, "[0-9]", ""));
                    item.IsMovable = false;
                    GetRoomByName(prefabInstanziata.transform.parent.name).oggetti.Add(item);
                    child.name = item.nomeOggetto;
                    if (child.name.Contains("NPC"))
                    {
                        item.IsNPC = true;
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(new StringReader(xmlPergamene.text));

                        string npcXmlPath = "//bashdungeon/npc";
                        XmlNodeList listOfNPCStrings = xmlDoc.SelectNodes(npcXmlPath);
                        foreach (XmlNode node in listOfNPCStrings)
                        {

                            if (child.name.Contains(node.FirstChild.InnerXml))
                            {

                                item.TestoTxT = node.LastChild.InnerXml;
                                break;
                            }
                        }
                    }
                    if (child.name.Contains("pergamena") && !child.name.Contains("frammento"))
                    {
                        item.IsTxt = true;

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(new StringReader(xmlPergamene.text));

                        string npcXmlPath = "//bashdungeon/pergamenaTutorial";
                        XmlNodeList listOfNPCStrings = xmlDoc.SelectNodes(npcXmlPath);
                        foreach (XmlNode node in listOfNPCStrings)
                        {

                            if (child.name.Contains(node.FirstChild.InnerXml))
                            {

                                item.TestoTxT = node.LastChild.InnerXml;
                                break;
                            }
                        }
                    }
                    if (child.name.Contains("frammentoPergamena"))
                    {
                        item.IsMovable = true;
                    }
                    if(child.name.Contains("Eliminabile"))
                    {
                        item.IsRemovable = true;
                    }
                    if (child.name.Contains("Nascosto") || child.name.Contains("Nascosta"))
                    {
                        item.IsInvisible = true;
                        child.name = item.nomeOggetto;
                    }
                }

            }

            Destroy(prefabInstanziata);
            if (room.type == 2 && levelDifficoulty <= maxLevelDifficoulty)
            {
                levelDifficoulty++;
            }
        }
        
        



        
    }

    void SpawnRandomPergamene(string xmlData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xmlData));

        string pergameneXmlPath = "//bashdungeon/pergamena";
        XmlNodeList listOfPergameneStrings = xmlDoc.SelectNodes(pergameneXmlPath);

        foreach(XmlNode node in listOfPergameneStrings)
        {

            bool isRoomFine = false;
            Room randomRoom = null;
            while (!isRoomFine){
                randomRoom = RandomRoomNoLevelOrRoot();
                if(!randomRoom.oggetti.Exists(x => x.nomeOggetto.Contains("pergamena")) || randomRoom.oggetti.FindAll(x => x.nomeOggetto.Contains("pergamena")).Count < 2)
                {
                    isRoomFine = true;
                }
            }
            Oggetto pergamena = new Oggetto(randomRoom, "pergamena");
            pergamena.IsTxt = true;
            pergamena.TestoTxT = node.FirstChild.InnerXml;
            randomRoom.oggetti.Add(pergamena);

            GameObject pergamenaPrefab = gameObject.GetComponent<ObjectPrefabSelector>().PickObjectPrefab(Regex.Replace(pergamena.nomeOggetto, "[0-9]", ""));

            GameObject pergamenaIstanziata = Instantiate(pergamenaPrefab) as GameObject;
            Vector3 oggettoPosition = Vector3.zero;

            oggettoPosition.y = pergamenaIstanziata.transform.position.y;
            oggettoPosition.x = Random.Range(-8, 5) + (pergamena.CurrentRoom.gridPos.x * 24);
            oggettoPosition.z = Random.Range(-6, 6) + (pergamena.CurrentRoom.gridPos.y * 24);

            pergamenaIstanziata.transform.position = oggettoPosition;
            pergamenaIstanziata.transform.Rotate(Vector3.up, Random.Range(-180.0f, 180.0f));

            pergamenaIstanziata.name = pergamena.nomeOggetto;
            pergamenaIstanziata.transform.parent = GameObject.Find("/" + pergamena.CurrentRoom.nomeStanza).transform;
        }
    }

    void SpawnOggetti()
    {
        Vector3 oggettoPosition = Vector3.zero;

        foreach (Oggetto oggetto in oggettiCreati)
        {

            GameObject selectedPrefab = gameObject.GetComponent<ObjectPrefabSelector>().PickObjectPrefab(Regex.Replace(oggetto.nomeOggetto, "[0-9]", ""));

            GameObject oggettoIstanziato = Instantiate(selectedPrefab) as GameObject;

            oggettoPosition.y = oggettoIstanziato.transform.position.y;
            oggettoPosition.x = oggettoIstanziato.transform.position.x + (oggetto.CurrentRoom.gridPos.x * 24);
            oggettoPosition.z = oggettoIstanziato.transform.position.z + (oggetto.CurrentRoom.gridPos.y * 24);

            oggettoIstanziato.transform.position = oggettoPosition;

            oggettoIstanziato.name = oggetto.nomeOggetto;
            oggettoIstanziato.transform.parent = GameObject.Find("/" + oggetto.CurrentRoom.nomeStanza).transform;

        }
    }


    void SpawnPlayer()
    {

        player.GetComponent<PlayerMovement>().currentRoom = roomsOrderByDistance[roomsOrderByDistance.Count - 1];
        player.transform.parent = (GameObject.Find("//").transform);
        player.transform.localPosition = player.transform.position;
    }

    public Room GetRoomByName(string name)
    {
        Room foundRoom = roomsOrderByDistance.Find(x => x.nomeStanza == name);

        return foundRoom;
    }

    public Room RandomRoomNoLevelOrRoot()
    {
        Room chosenRoom;
        chosenRoom = roomsOrderByDistance[Random.Range(0, roomsOrderByDistance.Count - 2)]; //Root si trova in roomsOrderByDistance.Count -1, dunque lo escludo

        if (levelRooms.Contains(chosenRoom) || levelRooms.Exists(x => x.childrenRooms.Contains(chosenRoom))) //se la stanza scelta random è un livello o un suo figlio la scarto e ne cerco un altra
        {
            return RandomRoomNoLevelOrRoot();
        }
        else
        {
            return chosenRoom;
        }
    }
}