using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	// the time the player started moving (when they clicked)
	private float startTime = 0;
	// where the player is going
	private Vector3 destination;

	// where the player originally came from with this click
	private Vector3 fromWhere;

	// the distance between the initial point the player was at when clicking and
	// wherever they clicked.
	private float journeyLength = 0;

	// modifier for how fast the player should lerp between locations
	public float speed = 1.0f;

	// if the player clicks find the world position for that click and try to Lerp
	// the player's position to that point. (click is checked every update, so
	// the newest click)
	void FixedUpdate () {
		if(Input.GetMouseButtonUp(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, 20)){
				// don't change the player's y position no matter where they click, but
				// use the other axes of hit point
				destination = new Vector3( hit.point.x, transform.position.y, hit.point.z);
				fromWhere = transform.position;
				startTime = Time.time; // set the most recent click time to now
				journeyLength = Vector3.Distance(fromWhere, destination);
			}
		}
		// if there is a journey to be had, lerp between where the player started
		// click and where they clicked, and the distance to go this frame is based
		// on speed to then figure out how far along the journey the player should be
		if(journeyLength > 0){
			float distCovered = (Time.time - startTime) * speed;
	    float fracJourney = distCovered / journeyLength;
	    transform.position = Vector3.Lerp(fromWhere, destination, fracJourney);
		}
	}
}
