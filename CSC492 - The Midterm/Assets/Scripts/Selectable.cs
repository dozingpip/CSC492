using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {
	// marker prefab to use
	public GameObject marker;

	// the name of the instantiated marker
	string markName;
	public void Selected(){
		// do something to show the player that this thing is chosen
		Vector3 markerOffset = new Vector3(0, 1, 0);
		GameObject mark = Instantiate(marker, transform.position + markerOffset, Quaternion.identity);
		mark.transform.parent = transform;
		markName = mark.name;

	}

	// slight change if this object got selected second
	public void SelectedSecond(){
		GetComponent<BoxCollider>().isTrigger = true;
		Selected();
	}

	// this object got deselected, make sure it's back to normal
	public void Deselected(){
		Destroy(transform.Find(markName).gameObject);
		GetComponent<BoxCollider>().isTrigger = false;
	}

	// getting triggered on a selectable object means the attract() has finished
	public void OnTriggerEnter(Collider other){
		Debug.Log("Hey! you!");
		// if the other object that made this object's trigger go off is selectable
		Selectable otherSelect = other.gameObject.GetComponent<Selectable>();
		if(otherSelect){
			// the attract is done at this point, so can combine the meshes
			GameManager.instance.combine(otherSelect.gameObject, gameObject);
		}
	}

	// is this object selected?
	public bool isSelected(){
		return transform.Find(markName);
	}
}
