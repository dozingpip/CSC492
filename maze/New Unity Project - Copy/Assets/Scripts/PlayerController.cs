using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	//the player object's rigid body
	private Rigidbody _player_body;

	// the camera currently being used
	private GameObject currentCam;

	//camera that follows the player around
  private GameObject followCam;
	// followCam's offset from the player's transform.position
	private Vector3 offset;

	// the camera fixed above the maze.
	private GameObject fixedCam;

	//The speed the player may move
	[Tooltip("The speed the player may move")]
  public float _speed = 10.0f;

	//Disables movement along the X-Axis
	[Tooltip("Disables movement along the X-Axis")]
  public bool disableAxisX = false;

	//Disables movement along the Y-Axis
	[Tooltip("Disables movement along the Y-Axis")]
  public bool disableAxisY = false;

	void Start () {

		_player_body = GetComponent<Rigidbody>();
		followCam = GameObject.FindGameObjectsWithTag("MainCamera")[0];
		offset = followCam.transform.position;

		fixedCam = GameObject.FindGameObjectsWithTag("FixedCam")[0];

		currentCam = followCam;
		fixedCam.SetActive(false);
	}

	/*
	If the player object triggers the collider of something with a "goal" tag,
	tell the game manager that the level has been completed.
	*/
	void OnTriggerEnter(Collider other){
        if (other.CompareTag("goal"))
        {
          GameManager.instance.levelOver();
        }
	}

	// do all movement and physics related things here
	void FixedUpdate() {

		//Reads user input
		float moveHorizontal =  (disableAxisX) ? 0 : Input.GetAxis ("Horizontal");
		float moveVertical =  (disableAxisY) ? 0: Input.GetAxis ("Vertical");

		//Generate an appropriate force vector
		Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);
		followCam.transform.position = transform.position+ offset;
		_player_body.AddForce(movement*_speed);
	}

	//check input and do things not related to physics.
	void Update(){
		// Toggle the current viewing camera with space.
		if(Input.GetKeyUp("space")){
			if(currentCam == followCam){
				followCam.SetActive(false);
				fixedCam.SetActive(true);
				currentCam = fixedCam;
			}else{
				fixedCam.SetActive(false);
				followCam.SetActive(true);
				currentCam = followCam;
			}
		}
	}
}
