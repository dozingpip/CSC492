using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour {
	Inventory inventory;

	// Use this for initialization
	void Start () {
		inventory = GameObject.Find("inventory").GetComponent<Inventory>();
	}

	// Update is called once per frame
	void Update () {

	}

	public void pickup(GameObject uiToAddTo){
		GameObject image = transform.Find("RawImage").gameObject;
		inventory.addItem(image);
	}
}
