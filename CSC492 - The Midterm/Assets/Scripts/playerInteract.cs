using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteract : MonoBehaviour {
	GameObject[] chosenObjects;

	// Use this for initialization
	void Start () {
		chosenObjects = new GameObject[2];
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 20)){
				Pickable pick = hit.transform.gameObject.GetComponent<Pickable>();
				if(pick!= null){
					pick.pickup();
				}else if(chosenObjects[1]==null){
					if(chosenObjects[0]==null){
						chosenObjects[0] = hit.transform.gameObject;
					}else{
						chosenObjects[1] = hit.transform.gameObject;
					}
				}else{
					Vector3 point = Vector3.zero;
					attract(chosenObjects, point);
				}
			}
		}
	}

	void attract(GameObject[] objects, Vector3 toWhere){
		for(int i = 0; i< objects.Length; i++){
			// objects[i] move toward toWhere
		}
	}
}
