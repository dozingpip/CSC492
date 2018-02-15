using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour {

	// if the player collides with something tagged as Drone, go to the next level
	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player"))
		{
			Debug.Log("hi");
			GameManager.instance.levelOver();
		}
	}
}
