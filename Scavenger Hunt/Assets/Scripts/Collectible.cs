using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	// light used to represent whether or not this is the next collectible
	private Light l;
	void Start(){
		l = GetComponent<Light>();
		l.intensity = 0;
		//if this one starts out being the next collectible, tell it that it's next.
		if(GameManager.instance.isNextCollectible(transform.GetSiblingIndex())){
			meNext();
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			if(GameManager.instance.isNextCollectible(transform.GetSiblingIndex())){
				GameManager.instance.collectNext();
				gameObject.SetActive(false);
			}
		}
	}

	public void meNext(){
		l.intensity = 2;
	}
}
