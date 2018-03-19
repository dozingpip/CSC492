using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	private List<GameObject> items;
	private Vector3 startOffset;
	private Vector3 itemScale;

	// Use this for initialization
	void Start () {
		items = new List<GameObject>();
		foreach(Transform child in transform)
		{
			items.Add(child.gameObject);
		}
		positionItems();
	}

	private void positionItems(){
		for(int i = 0; i< items.Count; i++)
		{
			items[i].transform.position = startOffset + (itemScale * i) + transform.position;
			items[i].transform.localScale = itemScale;
		}
	}

	private void update(){
		positionItems();
	}

	public void addItem(GameObject newItem){
		items.Add(newItem);
		newItem.transform.parent = gameObject.transform;
		update();
	}

	public void removeItem(GameObject removeItem){
		items.Remove(removeItem);
		removeItem.transform.parent = null;
		update();
	}
}
