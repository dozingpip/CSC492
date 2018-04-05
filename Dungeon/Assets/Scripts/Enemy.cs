using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			Debug.Log("i hit the player, oops");
			GameManager.instance.hurtPlayer();
		}else if(other.gameObject.CompareTag("Bullet")){
			Debug.Log("ow, player hit me");
			if(gameObject.CompareTag("Boss")){
				Debug.Log("player hit the boss!");
				GameManager.instance.KilledBoss();
			}

			// make this object inactive when it's been hit
			gameObject.SetActive(false);
			// destroy the bullet object after it hits this object
			Destroy(other.gameObject);
		}
	}
}
