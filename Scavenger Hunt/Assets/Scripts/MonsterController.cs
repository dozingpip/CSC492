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

	// Update is called once per frame
	void FixedUpdate () {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			GameManager.instance.gameOver();
		}
	}
}
