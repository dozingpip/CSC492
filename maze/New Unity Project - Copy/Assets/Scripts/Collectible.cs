using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
    private void Start()
    {
        GameManager.instance.increaseScore();
    }

    // Disable if picked up
    private void OnTriggerEnter(Collider other)
    {
        //When a collectible is picked up
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.decreaseScore();
            gameObject.SetActive(false);

        }
    }


void LateUpdate(){

		transform.Rotate (new Vector3 (15, 30, 45)*(Time.deltaTime));
	}
}
