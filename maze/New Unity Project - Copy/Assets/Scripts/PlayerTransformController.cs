using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Commented code is for organized people
// "No, I am not organized." -Thomas Clinton, January 25rth 2018

public class PlayerTransformController : MonoBehaviour {
	public float speed = 1f;
	public GameObject followCam;
	public GameObject fixedCam;
	public Text score_text;
	public Text game_over_text;

	private Rigidbody playerRB;
	private GameObject currentCam;
	private Vector3 offset;

	// the player's current score, initially 0
	private static float score = 0;
	private static float collectiblesInGame = 0;
	private float collectiblesInLevel;

	// represents the index of the current scene
	private int sceneIndex;
	//number of total scenes
	private int scenes;

	//This makes it so I can actually have some code triggered when a scene loads up
	// (makes the OnScreenLoaded function a subscriber of sceneLoaded)
	// yay delegates.
	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
		scenes = SceneManager.sceneCountInBuildSettings;
	}

	// Called when a scene is first loaded (whether it's the starting one or one loaded using loadScene())
	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		/* we don't actually know how many collectibles are in all the scenes together
				unless we go to all the scenes and count (or if it was hard-coded...),
				so when loading up a new scene, make sure to add the level's collectibles
				to the total count of collectibles in the game.
		*/
		collectiblesInLevel= GameObject.Find("Collectibles").transform.childCount;
		collectiblesInGame += collectiblesInLevel;
		//update scene index
		sceneIndex = SceneManager.GetActiveScene().buildIndex;
	}

	// Use this for initialization
	// start fires off after OnSceneLoaded and before the first update.
	void Start () {
		playerRB = GetComponent<Rigidbody>();
		//the initial distance between the player object and the camera following it.
		offset = followCam.transform.position;
		score_text.text = "current score: "+score;
	}

	// Update is called once per frame
	void Update () {
		float moveVertical = Input.GetAxis("Vertical");
		float moveHorizontal = Input.GetAxis("Horizontal");

		Vector3 moveVector = new Vector3(moveHorizontal, 0, moveVertical);
		playerRB.AddForce(moveVector * speed);

		// adjusts the following camera to the new position of the player object,
		// keeping in mind the offset.
		followCam.transform.position = transform.position + offset;

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

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Collectible")){
			score += 1;
			other.gameObject.SetActive(false);
			score_text.text = "score: "+ score;
		}else if(other.CompareTag("goal")){
			if (sceneIndex < scenes - 1) {
				SceneManager.LoadScene(sceneIndex + 1);
			}else{
				if(score < collectiblesInGame){
					game_over_text.text = "You won, but you missed some collectibles! D:";
				}else{
					game_over_text.text = "You won and got all the collectibles, do you feel achieved?";
				}
				game_over_text.gameObject.SetActive(true);
			}
		}
	}

	// not sure if I need this, but when the script is disabled, it
	// unsubscribes from the sceneLoaded trigger.
	void OnDisable(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
