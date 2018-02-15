using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	//player's rigid body
	private Rigidbody2D playerRB;

	// a multiplier for how much force a click puts on the player's rigidbody
	public float thrust = 1;
	// Use this for initialization
	void Start () {
		playerRB = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetMouseButtonDown(0)){
			playerRB.AddForce(transform.up * thrust);
		}
	}

	// when the player's collider hits an obstacle, lose a life,
	// if player hits a collectible, increase score
	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "obstacle")
			GameManager.instance.loseLife();
		else if(other.gameObject.tag == "collectible")
			GameManager.instance.increaseScore();
		else if(other.gameObject.tag == "goal")
			GameManager.instance.nextLevel();
	}
}
