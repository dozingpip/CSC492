using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holeControl : MonoBehaviour {
	public Transform cue_spawn;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("ball")){
			// the ball got in the hole, delete it, add points, whatever
			GameManager.instance.ballDown();
			other.gameObject.SetActive(false);
		}else if(other.gameObject.CompareTag("cue")){
			// reset the position of the cue ball
			other.gameObject.transform.position = cue_spawn.position;

			// when the cue ball's position is reset it might still have some
			// force applied to it, so need to reset the velocity.
			Rigidbody otherRB = other.gameObject.GetComponent<Rigidbody>();
			otherRB.velocity = Vector3.zero;
			otherRB.angularVelocity = Vector3.zero;
		}
	}
}
