using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			if(GameManager.instance.isNextCollectible(gameObject)){
				GameManager.instance.collectNext();
				gameObject.SetActive(false);
			}
		}
	}
}
