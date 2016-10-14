using UnityEngine;
using System.Collections;

public class ClientBehavior: Photon.MonoBehaviour {

	public static Hashtable players;
	static int keySelf = -1;

	[PunRPC]
	public void MakeTurn(int x, int y, int id, bool num) {
		Debug.Log ("In PUN");
		ServerLogic.SetTurn (new Turn(x,y,id), num);
	}

	void Update()
	{
		if (players == null) {
			players = new Hashtable ();
		}

		if (Input.GetButtonUp ("Jump")) {
			this.photonView.RPC ("MakeTurn", PhotonTargets.MasterClient, 1,1,0, PhotonNetwork.isMasterClient);
		}
	}

	public static void AddPlayer(int key, Player p) {
		players.Add (key, p);
	}

	public static void MovePlayer(int key, int x, int y) {
		Player p = players [key] as Player;
		p.x = x;
		p.y = y;
		//possibly move players on client here
	}

}

public class Player
{
	public int x;
	public int y;
	int id;
	PlayerType type;

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
