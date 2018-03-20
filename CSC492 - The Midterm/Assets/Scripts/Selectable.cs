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
	}

	public void Deselected(){
		Debug.Log("deselected");
		Destroy(transform.Find(markName).gameObject);
	}

	public void OnTriggerEnter(Collider other){
		//if(other) send gamemanager some kind signal saying the attract is done.
	}

	public bool isSelected(){
		return transform.Find(markName);
	}
}
