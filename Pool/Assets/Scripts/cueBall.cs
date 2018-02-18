using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cueBall : MonoBehaviour {
	Rigidbody cueRB;
	float mouseHeldTime;
	public float modifier = 1;

	// Use this for initialization
	void Start () {
		mouseHeldTime = 0;
		cueRB = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
    // add the amount of time between this frame the last frame to the time
    // the button has been held down (if it's still being pressed)
		if(Input.GetMouseButton(0)){
			mouseHeldTime+= Time.deltaTime;
		}

    /* Once the left mouse button is released, a few things happen.
     * A ray is cast from the mouse position and because of the layermask the
		 * force add will only happen if the ray intersects with the table layer,
		 * and the vector for the direction of the force to apply on the cue ball
		 * depends on where that intersection point is in relation to the cue ball.
     * Then I multiply that vector by the amount of time the mouse was held and
		 * by a modifier. Finally reset the mouse held timer so that it doesn't
		 * build up every time the user presses the mouse button.
     */
		if(Input.GetMouseButtonUp(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int layerMask = 1 << 8;
			if(Physics.Raycast(ray, out hit, 20, layerMask)){
				Vector3 direction = new Vector3(hit.point.x -
						transform.position.x, 0, hit.point.z - transform.position.z);
				cueRB.AddForce(direction * mouseHeldTime * modifier);
				mouseHeldTime = 0;
			}
		}
	}
}
