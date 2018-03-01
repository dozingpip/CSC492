using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {
	private GameObject player;
	public float speed = 0.5f;

	// Use this for initialization
	void Start () {
		GameObject[] playerSearch = GameObject.FindGameObjectsWithTag("Player");
		if(playerSearch.Length > 0)
			player = playerSearch[0];
	}

	// the monster moves a little bit toward the player, wherever they are this frame.
	// the distance to move is modified by the time since last frame so it looks like
	// smooth movement.
	void FixedUpdate () {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
	}

	// if the monster hits the player, it's game over for everyone
	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			GameManager.instance.gameOver();
		}
	}
}
