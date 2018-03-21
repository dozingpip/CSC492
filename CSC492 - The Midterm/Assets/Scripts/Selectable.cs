using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {
public GameObject marker;
string markName;
	public void Selected(){
		// do something to show the player that this thing is chosen
		Vector3 markerOffset = new Vector3(0, 1, 0);
		GameObject mark = Instantiate(marker, transform.position + markerOffset, Quaternion.identity);
		mark.transform.parent = transform;
		markName = mark.name;
		//GetComponent<BoxCollider>()
	}

	public void Deselected(){
		Debug.Log("deselected");
		Destroy(transform.Find(markName).gameObject);
	}

	public void OnTriggerEnter(Collider other){
		Debug.Log("Hey! you!");
		// if(other) send gamemanager some kind signal saying the attract is done.
		Selectable otherSelect = other.gameObject.GetComponent<Selectable>();
		if(otherSelect){
			GameManager.instance.combine(otherSelect.gameObject, gameObject);
		}
	}

	public bool isSelected(){
		return transform.Find(markName);
	}
}
