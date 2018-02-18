using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections.Generic;       //Allows us to use Lists.

public class GameManager : MonoBehaviour
{
  //Static instance of GameManager which allows it to be accessed by any other script.
  public static GameManager instance = null;

  // represents the index of the current scene
	private int sceneIndex;
	//number of total scenes
	private static int scenes;

  private int ballsLeft = 0;

  private static float totalTime = 0;

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
  void stageInit(Scene scene, LoadSceneMode mode)
  {
	  sceneIndex = SceneManager.GetActiveScene().buildIndex;

    // will only find things in the game scene and find all the balls
    // in order to end the game when there are none left
    GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
    if(balls.Length> 0){
      ballsLeft = balls.Length;
    }

    // if the time display is in the scene (it's only in game over)
    // change it to show the total time taken to complete the most
    // recent game.
    GameObject[] displayObject = GameObject.FindGameObjectsWithTag("total_time");
    if(displayObject.Length> 0){
      displayObject[0].GetComponent<Text>().text = "Time taken: "+ totalTime.ToString();
    }
  }

  // load a specified scene/ level
  public void loadLevel(int scene){
    if (scene < scenes && scene >= 0) {
      SceneManager.LoadScene(scene);
    }
  }

  // a ball went down the hole.  If it was the last one, game over.
  public void ballDown(){
    if(ballsLeft>1)
      ballsLeft--;
    else{
      ballsLeft--;
      gameOver();
    }
  }

  // when the game is reset to play a second time the timer won't get
  // reset as it's a static variable, so need to do it manually.
  public void resetTimer(){
    totalTime = 0;
  }

  // load the game over scene
  private void gameOver(){
    loadLevel(2);
  }

  // every frame increment time taken to complete the game
  // only if the game is still going (so shouldn't do
  // anything in the main menu or game over scene)
  void Update(){
    if(ballsLeft> 0)
      totalTime += Time.deltaTime;
  }
}
