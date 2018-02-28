using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private float startTime = 0;
	private Vector3 destination;
	private Vector3 fromWhere;
	private float journeyLength = 0;
	public float speed = 1.0f;

	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetMouseButtonUp(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int layerMask = 1 << 8;
			if(Physics.Raycast(ray, out hit, 20, layerMask)){
				//Vector3 direction = new Vector3(hit.point.x -
				//		transform.position.x, 0, hit.point.z - transform.position.z);
				destination = new Vector3( hit.point.x, transform.position.y, hit.point.x);
				fromWhere = transform.position;
				startTime = Time.time;
				journeyLength = Vector3.Distance(fromWhere, destination);
			}
		}
		if(journeyLength > 0){
			float distCovered = (Time.time - startTime) * speed;
	    float fracJourney = distCovered / journeyLength;
	    transform.position = Vector3.Lerp(fromWhere, destination, fracJourney);
		}
	}
}
