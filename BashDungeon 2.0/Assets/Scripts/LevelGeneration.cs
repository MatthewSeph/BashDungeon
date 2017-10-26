using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {
	Vector2 worldSize = new Vector2(5,5);
	Room[,] rooms;
	List<Vector2> takenPositions = new List<Vector2>();
	int gridSizeX, gridSizeY, numberOfRooms = 20;
	public GameObject roomWhiteObj;
	void Start () {
		if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2)){
			numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
		}
		gridSizeX = Mathf.RoundToInt(worldSize.x);
		gridSizeY = Mathf.RoundToInt(worldSize.y);
		CreateRooms();
		SetRoomDoors();
		DrawMap();
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
		foreach (Room room in rooms){
			if (room == null){
				continue;
			}
			Vector2 drawPos = room.gridPos;
			drawPos.x *= 16;
			drawPos.y *= 8;
			MapSpriteSelector mapper = Object.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
			mapper.type = room.type;
			mapper.up = room.doorTop;
			mapper.down = room.doorBot;
			mapper.right = room.doorRight;
			mapper.left = room.doorLeft;
		}
	}

	 void SetRoomDoors(){

        for(int i = 0; i<numberOfRooms-1; i++)
        {   

            if (takenPositions.Contains(takenPositions[i] + Vector2.up))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY ].GetParentRoom() == null) && (rooms[ (int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.up).x + gridSizeX, (int)(takenPositions[i] + Vector2.up).y + gridSizeY].doorBot = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorTop = true;
                }
            }

            if (takenPositions.Contains(takenPositions[i] + Vector2.right))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].GetParentRoom() == null) && (rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.right).x + gridSizeX, (int)(takenPositions[i] + Vector2.right).y + gridSizeY].doorRight = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorRight = true;
                }
            }

            if (takenPositions.Contains(takenPositions[i] + Vector2.down))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].GetParentRoom() == null) && (rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.down).x + gridSizeX, (int)(takenPositions[i] + Vector2.down).y + gridSizeY].doorTop = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorBot = true;
                }
            }

            if (takenPositions.Contains(takenPositions[i] + Vector2.left))
            {
                if ((rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].GetParentRoom() == null) && (rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].type != 1))
                {
                    rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].SetParentRoom(rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]);

                    rooms[(int)(takenPositions[i] + Vector2.left).x + gridSizeX, (int)(takenPositions[i] + Vector2.left).y + gridSizeY].doorRight = true;

                    (rooms[(int)(takenPositions[i]).x + gridSizeX, (int)(takenPositions[i]).y + gridSizeY]).doorLeft = true;
                }
            }

        }













        /*for (int x = 0; x < ((gridSizeX * 2)); x++){
			for (int y = 0; y < ((gridSizeY * 2)); y++){
				if (rooms[x,y] == null){
					continue;
				}
				Vector2 gridPosition = new Vector2(x,y);
				if (y - 1 < 0){ //check above
					rooms[x,y].doorBot = false;
				}else{
					rooms[x,y].doorBot = (rooms[x,y-1] != null);
				}
				if (y + 1 >= gridSizeY * 2){ //check bellow
					rooms[x,y].doorTop = false;
				}else{
					rooms[x,y].doorTop = (rooms[x,y+1] != null);
				}
				if (x - 1 < 0){ //check left
					rooms[x,y].doorLeft = false;
				}else{
					rooms[x,y].doorLeft = (rooms[x - 1,y] != null);
				}
				if (x + 1 >= gridSizeX * 2){ //check right
					rooms[x,y].doorRight = false;
				}else{
					rooms[x,y].doorRight = (rooms[x+1,y] != null);
				}
			}
		}*/
	}
}
