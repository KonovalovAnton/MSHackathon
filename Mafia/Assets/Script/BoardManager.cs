using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class BoardManager : MonoBehaviour {


	public static int columns = 8;
	public static int rows = 10;
	public GameObject floorTile;
	public GameObject outerWall;
	private Transform boardHolder;
	private List <Vector2> gridPositions = new List<Vector2>();
	public static Tile[][] tiles;

	void InitList() {
		tiles = new Tile[columns][];
		gridPositions.Clear();
	}

	void BoardSetup() {
		boardHolder = new GameObject ("Board").transform;

		float x_offset = 0;
		float y_offset = 0;
		float x = 0;
		float y = 0;

		for (int i = 0; i < columns; i += 1) {

			x = x_offset;
			y = y_offset;

			tiles[i] = new Tile[rows]; 
			for (int j = 0; j < rows; j += 1) {

				GameObject toInstance = floorTile;


				GameObject instance = 
					Instantiate(toInstance, new Vector2(x, y), Quaternion.identity) as GameObject;


				tiles[i][j] = new Tile(i, j, new Vector2(x, y), TileType.PASSABLE, instance);


				x += 0.5f;
				y -= 0.25f;

				//instance.transform.SetParent(boardHolder);

			}

			x_offset -= 0.5f;
			y_offset -= 0.25f; 
		}
	}

//	Vector2 randomPosition() {
//
//		int randomIndex_x = UnityEngine.Random.Range(0, columns);
//		int randomIndex_y = UnityEngine.Random.Range(0, rows);
//
//		Vector2 randomPos = gridPositions[randomIndex];
//
//		gridPositions.RemoveAt(randomIndex);
//
//		return randomPos;
//	}

	public static ArrayList getPossiblePaths(int x, int y) {
		ArrayList res = new ArrayList();

		if (x > 0 && y > 0 && ((Tile)tiles[x - 1][y - 1]).tileType == TileType.PASSABLE)
			res.Add(tiles[x- 1][y - 1]);

		if (y > 0 && ((Tile)tiles[x][y - 1]).tileType == TileType.PASSABLE)
			res.Add(tiles[x][y-1]);

		if (x + 1 < columns - 1 && y > 0 && ((Tile)tiles[x + 1][y - 1]).tileType == TileType.PASSABLE)
			res.Add(tiles[x + 1][y - 1]);

		if (x > 0 && ((Tile)tiles[x - 1][y]).tileType == TileType.PASSABLE)
			res.Add(tiles[x -1][y]);

		if (x + 1 < columns - 1 && ((Tile)tiles[x + 1][y]).tileType == TileType.PASSABLE)
			res.Add(tiles[x + 1][y]);

		if (x > 0 && y + 1 < rows - 1 && ((Tile)tiles[x - 1][y + 1]).tileType == TileType.PASSABLE)
			res.Add(tiles[x - 1][y + 1]);

		if (y + 1 < rows - 1 && ((Tile)tiles[x][y + 1]).tileType == TileType.PASSABLE)
			res.Add(tiles[x][y + 1]);

		if (x + 1 < columns - 1 && y + 1 < rows - 1 && ((Tile)tiles[x][y + 1]).tileType == TileType.PASSABLE)
			res.Add(tiles[x + 1][y + 1]);

		return res;
	}

	public static bool isPassable(int x, int y) {
		if(x > columns-1 || x < 0 || y < 0 || y > rows-1)
			return false;
		return ((Tile)tiles[x][y]).tileType == TileType.PASSABLE ? true : false;
	}

	public void SetupScene() {
		InitList();
		BoardSetup();
	}

	// Use this for initialization
	void Start () {
		SetupScene();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
