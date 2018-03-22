using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// it's not a ui button, that's why it's real
public class realButton : MonoBehaviour {

	// activates the use of materials in the game
	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			GameManager.instance.toggleAllMats();
		}
	}
}
