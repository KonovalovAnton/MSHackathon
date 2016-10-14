using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2d;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D>();

		rb2d = GetComponent<Rigidbody2D>();

		inverseMoveTime = 1f / moveTime;
	}
	
//	protected bool Move (int xDir, int yDir, out RaycastHit2D ray) {
//		Vector2 start = transform.position;
//
//		Vector2 end = start + new Vector2 (xDir, yDir);
//
//		boxCollider.enabled = false;
//
//
//	}
}
