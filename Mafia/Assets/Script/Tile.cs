using UnityEngine;
using System.Collections;

public class Tile {

	public int x;
	public int y;
	public Vector2 data;
	public TileType tileType;
	public GameObject gameObject;

	public Tile (int x, int y, Vector2 trueCoords, TileType tileType, GameObject gameObj) {
		this.x  = x;
		this.y = y;
		this.data = trueCoords;
		this.tileType = tileType;
		this.gameObject = gameObj;
	}

	public Tile deepCopy() {
		return new Tile(this.x, this.y, this.data, this.tileType, this.gameObject);
	}

}

public enum TileType {
	PASSABLE,
	UNPASSABLE
}