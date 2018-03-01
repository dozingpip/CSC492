using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections.Generic;       //Allows us to use Lists.

public class GameManager : MonoBehaviour
{
  //Static instance of GameManager which allows it to be accessed by any other script.
  public static GameManager instance = null;

  // the text label that shows the player how they're doing as far as collecting
  private Text state_label;

  private GameObject player;
  private GameObject spawn;

  // represents the index of the current scene
	private int sceneIndex;
	//number of total scenes
	public int scenes;

  private Transform collectibles;

  private int nextCollectibleIndex;

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
    GameObject[] collectibles_ = GameObject.FindGameObjectsWithTag("collectible");

    if(collectibles_.Length > 0){
      collectibles = collectibles_[0].transform;
      nextCollectibleIndex = 0;
    }
  }

  // check the given index against the next collectible index
  public bool isNextCollectible(int compareTo){
    if(compareTo == nextCollectibleIndex){
      return true;
    }else{
      return false;
    }
  }

  // keep track of whether the next collectible is valid and tell it to do whatever
  // it does when it thinks it's next
  public void collectNext(){
    if(nextCollectibleIndex < collectibles.childCount-1){
      nextCollectibleIndex++;
      collectibles.GetChild(nextCollectibleIndex).gameObject.GetComponent<Collectible>().meNext();
    }else{
      nextLevel();
    }
  }

  // Attempt to move onto the next level.
  private void nextLevel()
  {
    sceneIndex++;
		loadLevel(sceneIndex);
  }

  // load a specified scene/ level
  private void loadLevel(int scene){
    if (scene < scenes && scene >= 0) {
      SceneManager.LoadScene(scene);
    }else Debug.Log("oops, scene " +scene+ " doesnt exist. Actually we have only "+scenes+ " scenes.");
  }

  //load level by scene name instead of index.
  public void loadLevel(String name){
    if(SceneManager.GetSceneByName(name) != null){
      SceneManager.LoadScene(name);
    }
  }

  // load the game over scene
  public void gameOver(){
    loadLevel("Game_Over");
  }
}
