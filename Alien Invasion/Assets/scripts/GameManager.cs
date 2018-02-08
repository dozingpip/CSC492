using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	//Static instance of GameManager which allows it to be accessed by any other script.
  public static GameManager instance = null;

	private int aliensLeft = 0;

  // the text label that shows the player how they're doing as far as collecting
  private Text state_label;

	// represents the index of the current scene
	private int sceneIndex;

	//number of total scenes
	private int scenes;

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

    //get the total scene count
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
  void stageInit(Scene scene, LoadSceneMode mode)
  {
    // get the text tagged as score_display
    GameObject[] scoreObject = GameObject.FindGameObjectsWithTag("score_display");
		if (scoreObject.Length > 0)
    {
      state_label = scoreObject[0].GetComponent<Text> ();
      state_label.text = "";
    }
  }

  // add more aliens to the count
  public void moreAliens(){
    aliensLeft++;
  }

  // if an alien (that isn't the last one) gets killed somehow decrement the count of aliens left
  // or if that was the last one, the level is over.
  public void killedAlien(){
    if(aliensLeft>1)
      aliensLeft--;
    else{
      aliensLeft--;
      levelOver();
    }
  }

	// Move onto the next level if there is one, if there isn't, player wins.
  public void levelOver()
  {
    if (sceneIndex < scenes - 1) {
			SceneManager.LoadScene(sceneIndex + 1);
      sceneIndex+=1;
		}else{
      state_label.text = "Game Over";
		}
  }
}
