using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
	Vector2 worldSize = new Vector2(8,8);

	Room[,] DungeonRooms;

	List<Vector2> takenPositions = new List<Vector2>();

	int GridSizeX, GridSizeY, NumberOfRooms = 30;

	public GameObject MapObj;


	void Start ()
	{
		if (NumberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
		{
			NumberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
		}

		GridSizeX = Mathf.RoundToInt(worldSize.x);
		GridSizeY = Mathf.RoundToInt(worldSize.y);

		CreateRooms();
		SetRoomDoors();
		DrawMap();
	}


	void CreateRooms(){
		DungeonRooms = new Room[GridSizeX * 2,GridSizeY * 2];

		DungeonRooms[GridSizeX,GridSizeY] = new Room(Vector2.zero, 1);

		takenPositions.Insert(0,Vector2.zero);

		Vector2 checkPos = Vector2.zero;

		float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

		for (int i =0; i < NumberOfRooms -1; i++)
		{
			float randomPerc = ((float) i) / (((float)NumberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

			checkPos = NewPosition();

			if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
			{
				int iterations = 0;
				do{
					checkPos = SelectiveNewPosition();
					iterations++;
				}while(NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);

				if (iterations >= 50)
				{
					print("Error" + NumberOfNeighbors(checkPos, takenPositions));
				}
			}

			DungeonRooms[(int) checkPos.x + GridSizeX, (int) checkPos.y + GridSizeY] = new Room(checkPos, 0);

			takenPositions.Insert(0,checkPos);
		}	
	}


	Vector2 NewPosition()
	{
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;
		do{
			int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));

			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;

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
			checkingPos = new Vector2(x,y);

		}while (takenPositions.Contains(checkingPos) || x >= GridSizeX || x < -GridSizeX || y >= GridSizeY || y < -GridSizeY);

		return checkingPos;
	}


	Vector2 SelectiveNewPosition()
	{
		int index = 0, inc = 0;
		int x =0, y =0;
		Vector2 checkingPos = Vector2.zero;
		do{
			inc = 0;
			do
			{ 
				index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
				inc ++;
			}while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);

			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;

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
			checkingPos = new Vector2(x,y);

		}while (takenPositions.Contains(checkingPos) || x >= GridSizeX || x < -GridSizeX || y >= GridSizeY || y < -GridSizeY);
		
		if (inc >= 100)
		{
			print("Error");
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
		foreach (Room room in DungeonRooms)
		{
			if (room == null)
			{
				continue;
			}
			Vector2 drawPos = room.GridPos;

			drawPos.x *= 16;
			drawPos.y *= 8;

			RoomSelector mapper = Object.Instantiate(MapObj, drawPos, Quaternion.identity).GetComponent<RoomSelector>();
			mapper.type = room.Type;
			mapper.Up = room.DoorT;
			mapper.Down = room.DoorB;
			mapper.Right = room.DoorR;
			mapper.Left = room.DoorL;
		}
	}


	void SetRoomDoors()
	{
		for (int x = 0; x < ((GridSizeX * 2)); x++)
		{
			for (int y = 0; y < ((GridSizeY * 2)); y++)
			{
				if (DungeonRooms[x,y] == null)
				{
					continue;
				}
				Vector2 gridPosition = new Vector2(x,y);
				if (y - 1 < 0)
				{
					DungeonRooms[x,y].DoorB = false;
				}
				else
				{
					DungeonRooms[x,y].DoorB = (DungeonRooms[x,y-1] != null);
				}
				if (y + 1 >= GridSizeY * 2)
				{
					DungeonRooms[x,y].DoorT = false;
				}
				else
				{
					DungeonRooms[x,y].DoorT = (DungeonRooms[x,y+1] != null);
				}
				if (x - 1 < 0)
				{
					DungeonRooms[x,y].DoorL = false;
				}
				else
				{
					DungeonRooms[x,y].DoorL = (DungeonRooms[x-1,y] != null);
				}
				if (x + 1 >= GridSizeX * 2)
				{
					DungeonRooms[x,y].DoorR = false;
				}
				else
				{
					DungeonRooms[x,y].DoorR = (DungeonRooms[x+1,y] != null);
				}
			}
		}
	}
}
