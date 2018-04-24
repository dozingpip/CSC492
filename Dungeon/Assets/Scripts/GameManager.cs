using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//Static instance of GameManager which allows it to be accessed by any other script.
	public static GameManager instance = null;
	
	// the number of hits the player can take before losing the game
	private int playerHealth = 3;
	
	//text display object for the player's remaining health
	private Text healthDisplay;

	// number of rooms to be generated with the current difficulty setting
	private static int numRooms = 5;


	// represents the index of the current scene
	private int sceneIndex;
	//number of total scenes
	public int scenes;

	//Awake is always called before any Start functions
	void Awake()
	{
		//Check if instance already exists
		if (instance == null)
			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		scenes = SceneManager.sceneCountInBuildSettings;

		//Call the InitGame function to initialize the first level
		InitGame();

	}

	//Initializes the game for each level.
	void InitGame()
	{
		SceneManager.sceneLoaded += stageInit;
		sceneIndex = SceneManager.GetActiveScene().buildIndex;
	}

	// Called when a new scene is loaded
	// initialize variables for all the objects game manager needs to know about
	void stageInit(Scene scene, LoadSceneMode mode)
	{
		sceneIndex = SceneManager.GetActiveScene().buildIndex;
		GameObject healthObject = GameObject.FindGameObjectWithTag("health");
		if(healthObject){
			healthDisplay = healthObject.GetComponent<Text>();
		}
		resetGame();
	}

	// load a specified scene/ level
	private void loadLevel(int scene){
		if (scene < scenes && scene >= 0) {
			SceneManager.LoadScene(scene);
		}else Debug.Log("oops, scene " +scene+ " doesnt exist. Actually we have only "+scenes+ " scenes.");
		
		if(scene == 1){
			resetGame();
		}
	}

	//load level by scene name instead of index.
	public void loadLevel(String name){
		if(SceneManager.GetSceneByName(name) != null){
			SceneManager.LoadScene(name);
		}

		if(name == "dungeon"){
			resetGame();
		}
	}

	public void hurtPlayer(){
		if(playerHealth<2){
			playerHealth--;
			loadLevel("MainMenu");
		}else{
			playerHealth--;
			updateHealthDisplay();
		}
	}

	public void KilledBoss(){
		loadLevel("MainMenu");
	}

	private void updateHealthDisplay(){
		if(healthDisplay){
			healthDisplay.text = "Life points left: " + playerHealth;
		}
	}

	// make the game go back to basic settings to allow for replayibility.
	private void resetGame(){
		playerHealth = 3;
		updateHealthDisplay();
		GameObject generator = GameObject.FindGameObjectWithTag("generator");
		if(generator){
			generator.GetComponent<DungeonGenerator>().setMaxRoomCount(numRooms);
		}
	}

	// change the difficulty according/ the number of rooms to be generated
	public void changeDifficulty(int difficulty){
		switch(difficulty){
			case 0:
				numRooms = 3;
				break;
			case 1:
				numRooms = 5;
				break;
			case 2:
				numRooms = 10;
				break;
		}

		Debug.Log("set difficulty to: " + numRooms);
	}
}
