using UnityEngine;
using System.Collections;

public class ClientBehavior: Photon.MonoBehaviour {

	public static Hashtable players;
	public static int keySelf = -1;
	static Object playerPrefab;
	public GameObject playerPref;
	static Tile startPos;
	static int maxSteps = 4;
	static int stepCounter = 0;

	[PunRPC]
	public void MakeTurn(int x, int y, int id, bool num, int victim_id) {
		Debug.Log ("In PUN");
		ServerLogic.SetTurn (new Turn(x,y,id, victim_id), num);
	}

	void Start() {
		playerPrefab = playerPref;
	}

	void Update()
	{
		if (players == null) {
			players = new Hashtable ();
		}

		if(keySelf!=-1) {
			
			Player me = (Player)players[keySelf];

			if (Input.GetButtonUp ("Jump")) {
				int vic = victim == null ? -1 : victim.id;
				Debug.Log("ClientBehavior vic: " + vic);
				this.photonView.RPC ("MakeTurn", PhotonTargets.MasterClient,me.x,me.y,keySelf, PhotonNetwork.isMasterClient, vic);
			}

			if (Input.GetKeyUp(KeyCode.W)) {
				if(stepCounter < maxSteps && BoardManager.isPassable(me.x - 1,me.y - 1)) {
					stepCounter++;
					Tile go = BoardManager.tiles[me.x - 1][me.y - 1];
					Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
					me.prefab.transform.position = go.data + pitch;
					me.x--;
					me.y--;
					return;
				}
			}
			if (Input.GetKeyUp(KeyCode.Q)) {
				if(stepCounter < maxSteps && BoardManager.isPassable(me.x,me.y - 1)) {
					stepCounter++;
					Tile go = BoardManager.tiles[me.x][me.y - 1];
					Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
					me.prefab.transform.position = go.data + pitch;
					me.y--;
					return;
				}
			}
			if (Input.GetKeyUp(KeyCode.A)) {
				if(stepCounter < maxSteps && BoardManager.isPassable(me.x + 1,me.y - 1)) {
					stepCounter++;
					Tile go = BoardManager.tiles[me.x + 1][me.y - 1];
					Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
					me.prefab.transform.position = go.data + pitch;
					me.x++;
					me.y--;
					return;
				}
			}
			if (Input.GetKeyUp(KeyCode.E)) {
				if(stepCounter < maxSteps && BoardManager.isPassable(me.x - 1,me.y)) {
					stepCounter++;
					Tile go = BoardManager.tiles[me.x - 1][me.y];
					Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
					me.prefab.transform.position = go.data + pitch;
					me.x--;
					return;
				}
			}
			if (Input.GetKeyUp(KeyCode.S)) {
				stepCounter = 0;
				Tile go = BoardManager.tiles[startPos.x][startPos.y];
				Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
				me.prefab.transform.position = go.data + pitch;
				me.x = startPos.x;
				me.y = startPos.y;
			}
			if (Input.GetKeyUp(KeyCode.Z)) {
				if(stepCounter < maxSteps && BoardManager.isPassable(me.x + 1,me.y)) {
					stepCounter++;
					Tile go = BoardManager.tiles[me.x + 1][me.y];
					Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
					me.prefab.transform.position = go.data + pitch;
					me.x++;
					return;
				}
			}
			if (Input.GetKeyUp(KeyCode.D)) {
				if(stepCounter < maxSteps && BoardManager.isPassable(me.x - 1,me.y + 1)) {
					stepCounter++;
					Tile go = BoardManager.tiles[me.x - 1][me.y + 1];
					Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
					me.prefab.transform.position = go.data + pitch;
					me.x--;
					me.y++;
					return;
				}
			}
			if (Input.GetKeyUp(KeyCode.C)) {
				if(stepCounter < maxSteps && BoardManager.isPassable(me.x,me.y + 1)) {
					stepCounter++;
					Tile go = BoardManager.tiles[me.x][me.y + 1];
					Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
					me.prefab.transform.position = go.data + pitch;
					me.y++;
					return;
				}
			}
			if (Input.GetKeyUp(KeyCode.X)) {
				if(stepCounter < maxSteps && BoardManager.isPassable(me.x + 1,me.y + 1)) {
					stepCounter++;
					Tile go = BoardManager.tiles[me.x + 1][me.y + 1];
					Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
					me.prefab.transform.position = go.data + pitch;
					me.x++;
					me.y++;
					return;
				}
			}
		}
	}

	public static void AddPlayer(int key, Player p) {
		players.Add (key, p);
		Tile tile = BoardManager.tiles[p.x][p.y];
		//Transform t = Transform.Instantiate(playerPrefab, tile.data, Quaternion.identity) as Transform;
		Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
		GameObject t = Instantiate(playerPrefab, tile.data + pitch, Quaternion.identity) as GameObject;
		t.GetComponent<SpriteRenderer>().sortingOrder = 2 + p.y;
		if (key == keySelf) {
			t.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
			startPos = tile.deepCopy();
		} else 
		{
			int index = Random.Range(0,BoardManager.colors.Count);
			t.GetComponent<SpriteRenderer>().color = (Color)BoardManager.colors[index];
			BoardManager.colors.RemoveAt(index);
		}

		p.prefab = t.transform;
		t.GetComponent<CharacterBehavior>().id = key;
	}

	public static void MovePlayer(int key, int x, int y) {
		Player p = players [key] as Player;
		p.x = x;
		p.y = y;
		//possibly move players on client here
		Tile tile = BoardManager.tiles[p.x][p.y];
		//Transform t = Transform.Instantiate(playerPrefab, tile.data, Quaternion.identity) as Transform;
		Vector2 pitch = new Vector2(Random.value * 0.16f - 0.08f,Random.value * 0.1f - 0.08f);
		p.prefab.transform.position = tile.data + pitch;

		if (key == keySelf) {
			stepCounter = 0;
			startPos = (Tile)BoardManager.tiles[x][y];
			victim = null;
		}
	}

	public static Player GetMe() {
		return (Player)players[keySelf];
	}

	public static void Murder(int victim_id) {
		Player p = (Player)players[victim_id];
		p.prefab.GetComponent<CharacterBehavior>().Kill();
		players.Remove(victim_id);
	}

	static Player victim;

	public static void SetVictim(Player p) {
		victim = p;
	}

}

public class Player
{
	public int x;
	public int y;
	public int id;
	public PlayerType type;
	public Transform prefab;

	public Player(int id, PlayerType type,int x, int y)
	{
		this.id = id;
		this.type = type;
		this.x = x;
		this.y = y;
	}
}

public enum PlayerType
{
	Police,
	Mafia,
	Bot
}
