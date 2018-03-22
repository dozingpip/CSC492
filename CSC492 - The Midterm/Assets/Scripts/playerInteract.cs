using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteract : MonoBehaviour {
	// the objects that have been selected so far to attract to each other
	Queue<GameObject> attractSelection;
	// the distance the two objects should have between them at the end of
	//  the attract
	public float distanceThreshold =0.5f;
	// currently selected selectable object
	Selectable selected = null;
	// the line that represents where the user is pointing
	public LineRenderer mouseMarker;
	// how far away from the user should the selection ray be cast
	public float maxSelectDistance = 200f;

	// Use this for initialization
	void Start () {
		attractSelection = new Queue<GameObject>();
	}

	// Update is called once per frame
	void Update () {
		// first check if the game is still going
		if(GameManager.instance.isGameOn()){
			// get the user's mouse position as a ray starting from mousex and mousey
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			// check if this mouse position ray hits any objects
			if(Physics.Raycast(ray, out hit, maxSelectDistance)){
				// if the ray hit something and use left mouse-clicked
				if(Input.GetMouseButtonUp(0)){
					// did the raytracing clipboard item!
					GameManager.instance.checkCheckmark("raytracing");

					selected = hit.transform.gameObject.GetComponent<Selectable>();
					// if the selected object is selectable and not already selected
					if(selected && !attractSelection.Contains(selected.gameObject)){
						Debug.Log(attractSelection.Count);
						if(attractSelection.Count<1){
							selected.Selected();
							attractSelection.Enqueue(selected.gameObject);
							Debug.Log("chosen: " + selected.gameObject.name+", a: "+ attractSelection.Count);
						}else{
							selected.SelectedSecond();
							attractSelection.Enqueue(selected.gameObject);
						}
					}
					//set destination of navmeshagent to the other object, when they collide,
					// decide which object has precedence, delete the other object's collider
					// and either use combineMesh or parent one to combine.

				// right-click to rename the selected object
				}else if(Input.GetMouseButtonUp(1)){
					GameManager.instance.renameAsset(hit.transform.gameObject);
				}else{
					// as long as the ray isn't hitting the player object
					if(!hit.transform.gameObject.CompareTag("Player")){
						// set the line renderer to display a line from the origin of the
						//  ray (the user's mouse position) to the center of the object
						//  the ray is hitting
						mouseMarker.SetPosition(0, ray.origin);
						mouseMarker.SetPosition(1, hit.transform.position);
						// move the sphere that's also on the line renderer's object to
						//  to the end of the line/ center of the object the ray hit
						mouseMarker.gameObject.transform.position = hit.transform.position;
					}
				}
				// if there are 2 objects selected
				if(attractSelection.Count==2){
					// clear the queue of these 2 objects so that more can be selected
					GameObject obj1 = attractSelection.Dequeue();
					GameObject obj2 = attractSelection.Dequeue();
					// make the second object move towards the first
					GameManager.instance.attract(obj1, obj2, distanceThreshold);
				}
			}
		}
	}
}
