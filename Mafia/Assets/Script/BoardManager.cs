using UnityEngine;
using System.Collections.Generic;
using System;

public class BoardManager : MonoBehaviour {


	public int columns = 8;
	public int rows = 10;
	public GameObject floorTile;
	public GameObject outerWall;
	private Transform boardHolder;
	private List <Vector2> gridPositions = new List<Vector2>();

	void InitList() {
		gridPositions.Clear();

		for (int x = 1; x < columns - 1; x += 1) {
			for (int y = 1; y < rows - 1; y += 1) {
				gridPositions.Add(new Vector2(x, y));
			}
		}
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

			for (int j = 0; j < rows; j += 1) {

				GameObject toInstance = floorTile;


				GameObject instance = 
					Instantiate(toInstance, new Vector2(x, y), Quaternion.identity) as GameObject;

				x += 0.5f;
				y -= 0.25f;

				instance.transform.SetParent(boardHolder);

			}

			x_offset -= 0.5f;
			y_offset -= 0.25f; 
		}
	}

	Vector2 randomPosition() {

		int randomIndex = UnityEngine.Random.Range(0, gridPositions.Count);

		Vector2 randomPos = gridPositions[randomIndex];

		gridPositions.RemoveAt(randomIndex);

		return randomPos;
	}

	public void SetupScene() {
		BoardSetup();
		InitList();
	}

	// Use this for initialization
	void Start () {
		SetupScene();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
