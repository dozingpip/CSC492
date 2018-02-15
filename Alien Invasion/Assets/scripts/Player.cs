using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	Transform playerT;
	// speed modifier for player tank movement
	public float _speed = 1;

	// Use this for initialization
	void Start () {
		playerT = transform;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(gameObject.CompareTag("Tank")){
			//Reads user input
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical =  Input.GetAxis ("Vertical");

			//Generate an appropriate force vector
			Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);
			playerT.position+=movement*_speed;
		}
	}
}
