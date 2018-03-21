using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteract : MonoBehaviour {
	Queue<GameObject> attractSelection;
	public float distanceThreshold =1f;
	Pickable pick = null;
	Selectable selected = null;
	public GameObject playerUI;
	public LineRenderer mouseMarker;

	// Use this for initialization
	void Start () {
		attractSelection = new Queue<GameObject>();
	}

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Input.GetMouseButtonUp(0)){
			if(Physics.Raycast(ray, out hit, 2000)){
				Debug.Log("hit something");
				pick = hit.transform.gameObject.GetComponent<Pickable>();

				if(pick){
					pick.pickup(playerUI);
				}else{
					selected = hit.transform.gameObject.GetComponent<Selectable>();
				}
				if(selected && !attractSelection.Contains(selected.gameObject)){
					selected.Selected();
					if(attractSelection.Count<2){
						attractSelection.Enqueue(selected.gameObject);
						Debug.Log("chosen: " + selected.gameObject.name+", a: "+ attractSelection.Count);
					}else{
						attractSelection.Dequeue().GetComponent<Selectable>().Deselected();
						attractSelection.Enqueue(selected.gameObject);
					}
				}
				//set destination of navmeshagent to the other object, when they collide, decide which object has precedence, delete the other object's collider and either use combineMesh or parent one to combine.
			}
		}else{
				if(Physics.Raycast(ray, out hit, 200)){
					if(!hit.transform.gameObject.CompareTag("Player")){
						Debug.Log(hit.transform.gameObject.name);
						mouseMarker.SetPosition(0, ray.origin + new Vector3(0.5f, 0, 0.5f));
						mouseMarker.SetPosition(1, hit.point);
						mouseMarker.gameObject.transform.position = ray.origin;
					}
				}
		}
		if(attractSelection.Count==2){
			GameObject obj1 = attractSelection.Dequeue();
			GameObject obj2 = attractSelection.Dequeue();
			GameManager.instance.attract(obj1, obj2, distanceThreshold);
			obj1.GetComponent<Selectable>().Deselected();
			obj2.GetComponent<Selectable>().Deselected();
		}
	}
}
