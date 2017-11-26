using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGeneration : MonoBehaviour {
	Vector2 worldSize = new Vector2(5,5);

	Room[,] rooms;
	List<Vector2> takenPositions = new List<Vector2>();
	int gridSizeX, gridSizeY, numberOfRooms = 25;
    List<Room> roomsOrderByDistance = new List<Room>();
	List<Room> roomsWithNoChildren = new List<Room>();
    public GameObject player;
    List<Oggetto> oggettiCreati = new List<Oggetto>();
    List<Room> levelRooms = new List<Room>();

    public List<GameObject> LootPrefabs = new List<GameObject>(); 

	void Start () {
		player = GameObject.Find ("Player");

		if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2)){
			numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
		}
		gridSizeX = Mathf.RoundToInt(worldSize.x);
		gridSizeY = Mathf.RoundToInt(worldSize.y);

		CreateRooms();
		SetRoomDoors();
        SetDistances();
        RoomsOrderByDistance();
		CheckRoomWithNoChildrenSorted();
		DrawMap();
        SetLevelTypeRooms();
        OggettiNelleStanze();
        SpawnOggetti();
        SpawnPlayer();

    }
	void CreateRooms(){
		//setup
		rooms = new Room[gridSizeX * 2,gridSizeY * 2];
		rooms[gridSizeX,gridSizeY] = new Room(Vector2.zero, 1);
		takenPositions.Insert(0,Vector2.zero);
		Vector2 checkPos = Vector2.zero;
		//magic numbers
		float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
		//add rooms
		for (int i =0; i < numberOfRooms -1; i++){
			float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
			//grab new position
			checkPos = NewPosition();
			//test new position
			if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare){
				int iterations = 0;
				do{
					checkPos = SelectiveNewPosition();
					iterations++;
				}while(NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
				if (iterations >= 50)
					print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
			}
			//finalize position
			rooms[(int) checkPos.x + gridSizeX, (int) checkPos.y + gridSizeY] = new Room(checkPos, 0);
			takenPositions.Insert(i+1,checkPos);

		}	
	}
	Vector2 NewPosition(){
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;
		do{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);
			if (UpDown){
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
		return checkingPos;
	}
	Vector2 SelectiveNewPosition(){
		int index = 0, inc = 0;
		int x =0, y =0;
		Vector2 checkingPos = Vector2.zero;
		do{
			inc = 0;
			do{
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc ++;
			}while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;
			bool UpDown = (Random.value < 0.5f);
			bool positive = (Random.value < 0.5f);
			if (UpDown){
				if (positive){
					y += 1;
				}else{
					y -= 1;
				}
			}else{
				if (positive){
					x += 1;
				}else{
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
		if (inc >= 100){
			print("Error: could not find position with only one neighbor");
		}
		return checkingPos;
	}

	int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions){
		int ret = 0;
		if (usedPositions.Contains(checkingPos + Vector2.right)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.left)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.up)){
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.down)){
			ret++;
		}
		return ret;
	}

	void DrawMap(){
		foreach (Room room in roomsOrderByDistance){

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

	void SetRoomDoors(){

        for(int i = 0; i<numberOfRooms-1; i++)
        {   
			//Se la stanza adiacente esiste e non ha un Parent allora esso gli viene settato
            if (takenPositions.Contains(takenPositions[i] + Vector2.up))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY ].GetParentRoom() == null) && (rooms[ (int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

					rooms [(int)(takenPositions [i]).x + gridSizeX, (int)(takenPositions [i]).y + gridSizeY].childrenRooms.Add(rooms [(int)(takenPositions [i] + Vector2.up).x + gridSizeX, (int)(takenPositions [i] + Vector2.up).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY].doorBot = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorTop = true;
                }
            }

            if (takenPositions.Contains(takenPositions[i] + Vector2.right))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].GetParentRoom() == null) && (rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

					rooms [(int)(takenPositions [i]).x + gridSizeX, (int)(takenPositions [i]).y + gridSizeY].childrenRooms.Insert (0, rooms [(int)(takenPositions [i] + Vector2.right).x + gridSizeX, (int)(takenPositions [i] + Vector2.right).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].doorLeft = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorRight = true;
                }
            }

            if (takenPositions.Contains(takenPositions[i] + Vector2.down))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].GetParentRoom() == null) && (rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

					rooms [(int)(takenPositions [i]).x + gridSizeX, (int)(takenPositions [i]).y + gridSizeY].childrenRooms.Insert (0, rooms [(int)(takenPositions [i] + Vector2.down).x + gridSizeX, (int)(takenPositions [i] + Vector2.down).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].doorTop = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorBot = true;
                }
            }

            if (takenPositions.Contains(takenPositions[i] + Vector2.left))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].GetParentRoom() == null) && (rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

					rooms [(int)(takenPositions [i]).x + gridSizeX, (int)(takenPositions [i]).y + gridSizeY].childrenRooms.Insert (0, rooms [(int)(takenPositions [i] + Vector2.left).x + gridSizeX, (int)(takenPositions [i] + Vector2.left).y + gridSizeY]);

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

            roomsOrderByDistance.Insert( 0 , room);
        }

        roomsOrderByDistance.Sort(SortByDistance);
    }

	void CheckRoomWithNoChildrenSorted()
	{
		foreach (Room room in roomsOrderByDistance) 
		{
			if (room.childrenRooms.Count == 0) 
			{
				roomsWithNoChildren.Add (room);

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
		if ( !(roomsWithNoChildren.Count <= LootPrefabs.Count)) { //se ho abbastanza stanze terminali setto le stanze con loot, altrimenti rigenero il dungeon
			Vector3 lootPosition = Vector3.zero;
            

			for (int i = 0; i < roomsWithNoChildren.Count; i++) {

                if (!levelRooms.Contains(roomsWithNoChildren[i].parentRoom) && roomsWithNoChildren[i].parentRoom.type != 1)
                {
                    levelRooms.Add(roomsWithNoChildren[i].parentRoom);
                    roomsWithNoChildren[i].parentRoom.type = 2;

                    //Creo un oggetto nelle stanze-Livello tanto per vedere se funziona.. andrà rimosso e andrà fatto un controllo con il type della room.
                    Oggetto oggetto = new Oggetto(roomsWithNoChildren[i].parentRoom, "cassa");
                    roomsWithNoChildren[i].parentRoom.oggetti.Add(oggetto);
                    oggettiCreati.Add(oggetto);
                }
                if (levelRooms.Count == 5)
                {
                    break;
                }

            }
            if(levelRooms.Count < 5)
            {
                Debug.Log("Dungeon rigenerato per mancanza di ''stanze terminali'' utili");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
		}
        else 
		{
			Debug.Log ("Dungeon rigenerato per mancanza di ''stanze terminali''");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

    void OggettiNelleStanze()
    {
        //Creo un oggetto in root per testing.
        Oggetto oggetto = new Oggetto((roomsOrderByDistance[roomsOrderByDistance.Count - 1]), "cassa");
        oggetto.IsMovable = true;
        oggetto.IsRemovable = true;
        oggetto.IsTxt = true;
        oggetto.TextoTxt = "Una bella stringa per una cassa :)\n12345566363626229990";
        roomsOrderByDistance[roomsOrderByDistance.Count - 1].oggetti.Add(oggetto);
        oggettiCreati.Add(oggetto);
        Oggetto oggetto1 = new Oggetto((roomsOrderByDistance[roomsOrderByDistance.Count - 1]), "cassa");
        oggetto1.IsInvisible = true;
        oggetto1.IsTxt = true;
        oggetto1.TextoTxt = "Una bella stringa per questa cassa :)\n12345566363626229991";
        roomsOrderByDistance[roomsOrderByDistance.Count - 1].oggetti.Add(oggetto1);
        Oggetto oggetto2 = new Oggetto((roomsOrderByDistance[roomsOrderByDistance.Count - 1]), "pergamena");
        oggetto2.IsMovable = true;
        oggetto2.IsTxt = true;
        oggetto2.TextoTxt = "Il comando grep con c# e' un po' stronzetto con gli asterischi";
        roomsOrderByDistance[roomsOrderByDistance.Count - 1].oggetti.Add(oggetto2);
        oggettiCreati.Add(oggetto2);
        // Dovremo controllare la lista levelRooms e a seconda del tipo e della difficoltà del livello creare oggetti adeguati
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
        

        player.GetComponent<PlayerMovement>().currentRoom = roomsOrderByDistance[roomsOrderByDistance.Count-1];
        player.transform.parent = (GameObject.Find("//").transform);
        player.transform.localPosition = player.transform.position;


    }

	public Room GetRoomByName(string name)
	{
        Room foundRoom = roomsOrderByDistance.Find(x => x.nomeStanza == name);

        return foundRoom;
	}
}