using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteract : MonoBehaviour {
	Queue<GameObject> attractSelection;
	public float distanceThreshold =1f;
	Pickable pick = null;
	Choosable chosen = null;
	public GameObject playerUI;

	// Use this for initialization
	void Start () {
		attractSelection = new Queue<GameObject>();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 2000)){
				Debug.Log("hit something");
				pick = hit.transform.gameObject.GetComponent<Pickable>();

				if(pick){
					pick.pickup(playerUI);
				}else{
					chosen = hit.transform.gameObject.GetComponent<Choosable>();
				}
				if(chosen){
					Debug.Log("chosen: "+ hit.transform.gameObject.name);
					if(attractSelection.Count<2){
						attractSelection.Enqueue(hit.transform.gameObject);
					}else{
						attractSelection.Dequeue();
						attractSelection.Enqueue(hit.transform.gameObject);
					}
				}
				//set destination of navmeshagent to the other object, when they collide, decide which object has precedence, delete the other object's collider and either use combineMesh or parent one to combine.
			}
		}
		if(attractSelection.Count==2)
			GameManager.instance.attract(attractSelection.Dequeue(), attractSelection.Dequeue(), distanceThreshold);
	}
}
