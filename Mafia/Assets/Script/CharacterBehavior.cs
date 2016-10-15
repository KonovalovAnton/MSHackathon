using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

	public int id;
	public Sprite dead;
	public Sprite[] hair;
	public Sprite[] face;
	public Sprite[] shirt;
	public Transform left_pivot;
	public Transform right_pivot;

	// Use this for initialization
	void Start () {
		left_pivot = GameObject.Find("left_pivot").transform;
		right_pivot = GameObject.Find("right_pivot").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Debug.Log("Clicked on ID: " + id);
		if (!PhotonNetwork.isMasterClient) {
			if (id > 10000) {
				Player p = (Player)ClientBehavior.players[id];
				Player me = (Player)ClientBehavior.GetMe();
				if (Mathf.Abs(p.x - me.x) + Mathf.Abs(p.y - me.y) <= 1) {
					Debug.Log("Victim selected ID: " + id);
					ClientBehavior.SetVictim(p);
				}
			}
		}
		else {
			Player p = (Player)ClientBehavior.players[id];
			right_pivot.GetChild(0).GetComponent<SpriteRenderer>().sprite = hair[p.portrait.hair];
			right_pivot.GetChild(1).GetComponent<SpriteRenderer>().sprite = face[p.portrait.head];
			right_pivot.GetChild(2).GetComponent<SpriteRenderer>().sprite = shirt[p.portrait.shirt];

			if (id > 10000) {
				ClientBehavior.SetWitness(p);
			}
		}
	}

	public void Kill(int victim_id) {
		Debug.Log("ID " + id + " has died");
		//gameObject.SetActive(false);
		Player deadMan = (Player)ClientBehavior.players[victim_id];
		foreach (Player p in ClientBehavior.players.Values) {
			if (p.type == PlayerType.Bot) {
				int r = Mathf.Abs(deadMan.x - p.x) + Mathf.Abs(deadMan.y - p.y);
				p.probability = 1f - Mathf.Pow(0.9f, r);
			}
		}
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		sr.sprite = dead;
		sr.color = Color.white;
		transform.localScale = new Vector3(0.5f,0.5f,0.5f);

		ClientBehavior.players.Remove(victim_id);
	}

	public void Witness(Player p) {
		Portrait port = p.PredictPortrait();

		left_pivot.GetChild(0).GetComponent<SpriteRenderer>().sprite = hair[port.hair];
		left_pivot.GetChild(1).GetComponent<SpriteRenderer>().sprite = face[port.head];
		left_pivot.GetChild(2).GetComponent<SpriteRenderer>().sprite = shirt[port.shirt];


	}
}
