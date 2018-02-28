using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
	private void OnTriggerEnter(Collider other){
		Debug.Log("hi");
		if(other.gameObject.CompareTag("Player")){
			if(GameManager.instance.isNextCollectible(gameObject)){
				GameManager.instance.collectNext();
				gameObject.SetActive(false);
			}
		}
	}
}
