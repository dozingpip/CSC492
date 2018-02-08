using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// Use this for initialization
	private void Start () {
		GameManager.instance.moreAliens();
	}

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Bullet"))
		{
			GameManager.instance.killedAlien();
			gameObject.SetActive(false);
		}
	}
}
