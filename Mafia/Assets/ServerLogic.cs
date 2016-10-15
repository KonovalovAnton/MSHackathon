using UnityEngine;
using System.Collections;

public class ServerLogic : Photon.MonoBehaviour {

	const int NUM_BOTS = 5;

	static TurnInfo police;
	static TurnInfo mafia;
	static int idSource = 10001;

	public static void SetTurn(Turn t, bool master) {
		if (master) {
			if (!police.isReady) {
				Debug.Log ("SET 0");
				police.turn = t;
				police.isReady = true;
			}
		} else {
			if (!mafia.isReady) {
				Debug.Log ("SET 1");
				mafia.turn = t;
				mafia.isReady = true;
			}
		}
	}

	void Update () {
		if (PhotonNetwork.playerList.Length == 2 && PhotonNetwork.isMasterClient) {

			if (police == null) {
				police = new TurnInfo ();
				mafia = new TurnInfo ();
				PlaceGuys ();
			}

			if (police.isReady && mafia.isReady) {
				Debug.Log ("Turn made!");
				MoveGuys ();
				police.isReady = false;
				mafia.isReady = false;
			}
		}
	}

	void GetPlace(out int x, out int y) {
		do
		{
			x = Random.Range(0, BoardManager.columns);
			y = Random.Range(0, BoardManager.rows);

		} while (!BoardManager.isPassable(x, y));
	}

	void PlaceGuys()
	{
		int x , y; 

		for (int i = 0; i < NUM_BOTS; i++) {
			GetPlace(out x, out y);

			this.photonView.RPC ("GenCharacter", PhotonTargets.AllBuffered, idSource++, x, y, PlayerType.Bot);
		}
		GetPlace(out x, out y);
		this.photonView.RPC ("GenCharacter", PhotonTargets.AllBuffered, 0, x, y, PlayerType.Police);

		GetPlace(out x, out y);
		this.photonView.RPC ("GenCharacter", PhotonTargets.AllBuffered, 1, x, y, PlayerType.Mafia);
	}

	void MoveGuys()
	{
		this.photonView.RPC ("MoveCharacter", PhotonTargets.AllBuffered, 0, police.turn.x, police.turn.y);
		this.photonView.RPC ("MoveCharacter", PhotonTargets.AllBuffered, 1, mafia.turn.x, mafia.turn.y);
		//move bots algorythm here: where p.type == PlayerType.Bot
		if (mafia.turn.victim_id > 10000) {
			this.photonView.RPC("Murder", PhotonTargets.AllBuffered, mafia.turn.victim_id);
		}
			
		//ClientBehavior.players.Values
		foreach(Player p in ClientBehavior.players.Values){
			if(p.type == PlayerType.Bot) {
				MoveBot(p);
			}
		}
	}

	void MoveBot(Player p) {
		for(int i = 0; i < 5; i++) {
			ArrayList l = BoardManager.getPossiblePaths(p.x,p.y);
			Tile t = (Tile)l[Random.Range(0,l.Count)];
			p.x = t.x;
			p.y = t.y;
		}
		this.photonView.RPC("MoveCharacter", PhotonTargets.AllBuffered, p.id, p.x, p.y);
	}

	[PunRPC]
	public void Murder(int victim_id) {
		ClientBehavior.Murder(victim_id);
	}

	[PunRPC]
	public void GenCharacter(int id, int x, int y, PlayerType t)
	{
		if (PhotonNetwork.isMasterClient && id == 0)
			ClientBehavior.keySelf = id;
		else if (!PhotonNetwork.isMasterClient && id == 1)
			ClientBehavior.keySelf = id;
		
		ClientBehavior.AddPlayer (id, new Player (id, t, x, y));
	}

	[PunRPC]
	public void MoveCharacter(int id, int x, int y)
	{
		ClientBehavior.MovePlayer (id, x, y);
	}
}


public class TurnInfo
{
	public Turn turn;
	public bool isReady = false;
}


public class Turn
{
	public int id;
	public int x;
	public int y;
	public int victim_id;
	public int witness_id;

	public Turn(int x, int y, int id, int victim_id, int witns_id)
	{
		this.x = x;
		this.y = y;
		this.id = id;
		this.victim_id = victim_id;
		this.witness_id = witns_id;
	}
}

