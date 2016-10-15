using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {

	public int id;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Debug.Log("Clicked on ID: " + id);
		if (id > 10000) {
			Player p = (Player)ClientBehavior.players[id];
			Player me = (Player)ClientBehavior.GetMe();
			if (Mathf.Abs(p.x - me.x) + Mathf.Abs(p.y - me.y) <= 1) {
				Debug.Log("Victim selected ID: " + id);
				ClientBehavior.SetVictim(p);
			}
		}
	}

	public void Kill() {
		Debug.Log("ID " + id + " has died");
		gameObject.SetActive(false);
	}
}
