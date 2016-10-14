using UnityEngine;
using System.Collections;

public class ServerLogic : Photon.MonoBehaviour {

	const int NUM_BOTS = 5;

	static TurnInfo police;
	static TurnInfo mafia;
	static int idSource = 10000;

	public static void SetTurn(Turn t, bool master) {
		if (master) {
			Debug.Log ("SET 0");
			police.turn = t;
			police.isReady = true;
		} else {
			Debug.Log ("SET 1");
			mafia.turn = t;
			mafia.isReady = true;
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

	void PlaceGuys()
	{
		for (int i = 0; i < NUM_BOTS; i++) {
			//TODO replace zeroes with map based random
			this.photonView.RPC ("GenCharacter", PhotonTargets.AllBuffered, idSource++, 0, 0, PlayerType.Bot);
		}
		this.photonView.RPC ("GenCharacter", PhotonTargets.AllBuffered, 0, 0, 0, PlayerType.Police);
		this.photonView.RPC ("GenCharacter", PhotonTargets.AllBuffered, 1, 0, 0, PlayerType.Mafia);
	}

	void MoveGuys()
	{
		this.photonView.RPC ("MoveCharacter", PhotonTargets.AllBuffered, 0, police.turn.x, police.turn.y);
		this.photonView.RPC ("MoveCharacter", PhotonTargets.AllBuffered, 1, mafia.turn.x, mafia.turn.y);
		//move bots algorythm here: where p.type == PlayerType.Bot
		//ClientBehavior.players.Values
	}

	[PunRPC]
	public void GenCharacter(int id, int x, int y, PlayerType t)
	{
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

	public Turn(int x, int y,int id)
	{
		this.x = x;
		this.y = y;
		this.id = id;
	}
}

