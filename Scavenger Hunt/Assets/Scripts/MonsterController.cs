using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {
	public GameObject player;
	public float speed = 0.5f;

	// Use this for initialization
	void Start () {
		GameObject[] playerSearch = GameObject.FindGameObjectsWithTag("Player");
		if(playerSearch.Length > 0)
			player = playerSearch[0];
	}

	// Update is called once per frame
	void FixedUpdate () {
		//transform.position = Vector3.Lerp(transform.position, player.transform.position);
	}
}
