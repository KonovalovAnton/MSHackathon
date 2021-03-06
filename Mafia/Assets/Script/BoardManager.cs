﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class BoardManager : MonoBehaviour {


	public static ArrayList colors = new ArrayList();

	public static int columns = 10;
	public static int rows = 10;
	public GameObject floorTile;
	public GameObject outerWall;
	private Transform boardHolder;
	private List <Vector2> gridPositions = new List<Vector2>();
	public static Tile[][] tiles;

	void InitList() {
		tiles = new Tile[columns][];
		gridPositions.Clear();
		colors.Add(new Color(1f, 0.596f, 0f)); // orange
		colors.Add(new Color(0.32f, 0.42f, 0.99f)); // blue
		colors.Add(new Color(1f, 0.92f, 0.23f)); // yellow
		colors.Add(new Color(0.3f, 0.68f, 0.31f)); // green
		colors.Add(new Color(0.36f, 0.25f, 0.21f)); // brown
		colors.Add(new Color(0.31f, 0.17f, 0.65f)); // purple
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
		AwesomeHardcodeStuff();
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

	public void AwesomeHardcodeStuff() {
		// Awesome car
		((Tile)tiles[0][0]).tileType = TileType.UNPASSABLE;
		((Tile)tiles[0][1]).tileType = TileType.UNPASSABLE;
		((Tile)tiles[1][0]).tileType = TileType.UNPASSABLE;
		((Tile)tiles[1][1]).tileType = TileType.UNPASSABLE;
		((Tile)tiles[2][0]).tileType = TileType.UNPASSABLE;
		((Tile)tiles[2][1]).tileType = TileType.UNPASSABLE;

		// Lamps
		((Tile)tiles[2][8]).tileType = TileType.UNPASSABLE;
		((Tile)tiles[5][2]).tileType = TileType.UNPASSABLE;

		// Bench
		((Tile)tiles[6][1]).tileType = TileType.UNPASSABLE;
		((Tile)tiles[7][1]).tileType = TileType.UNPASSABLE;
		((Tile)tiles[8][1]).tileType = TileType.UNPASSABLE;
	}

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
