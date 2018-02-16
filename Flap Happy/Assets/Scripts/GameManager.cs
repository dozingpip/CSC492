using UnityEngine;
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

  // the text that gets added to the state label when you hit the goal on the last lvl
  private string youWon = "You won!";

  // represents the index of the current scene
	private int sceneIndex;
	//number of total scenes
	private static int scenes;

  //number of collectibles found
  private static int score = 0;

  //number of tries the player has left
  private static int lives = 2;

  private static int difficultySetting = 1;

//private static Color collectibleColor;

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
    if(lives>0){
      GameObject[] tempPlayer = GameObject.FindGameObjectsWithTag("Player");
      GameObject[] tempSpawn = GameObject.FindGameObjectsWithTag("Spawn");
      if(tempPlayer.Length > 0 && tempSpawn.Length > 0){
        player = tempPlayer[0];
        spawn = tempSpawn[0];
        playerRespawn();
      }
    }

    GameObject[] scoreObject = GameObject.FindGameObjectsWithTag("score_display");

    if (scoreObject.Length > 0)
    {
      state_label = scoreObject[0].GetComponent<Text> ();
      state_label.text = "score: "+score+ ", lives: "+ lives;
    }
  }

  /*
  when the player finds a collectible, score is incremented and the display
  is updated to reflect that
   */
  public void increaseScore()
  {
    score += 1;
    state_label.text = "score: "+score+ ", lives: "+ lives;
  }

  // Attempt to move onto the next level.
  public void nextLevel()
  {
    sceneIndex+=1;
		loadLevel(sceneIndex);
  }

  // load a specified scene/ level
  public void loadLevel(int scene){
		Debug.Log ("scene"+scene + ", scenes"+ scenes);
    if (scene < scenes && scene >= 0) {
      SceneManager.LoadScene(scene);
    }else{
      if(lives>0 && scene>1){
        state_label.text = youWon+", score: "+score;
      }else{
        state_label.text = "That looks like it hurt! You lost.";
      }
	}
  }

  // change the difficulty/ number of lives the player starts with
  public void changeDifficulty(int difficulty){
		difficultySetting = difficulty;
    switch(difficulty){
      case 0: lives = 3; break;
      case 1: lives = 2; break;
      case 2: lives = 1; break;
      default: lives = 2; break;
    }
  }

/*  public void changeCollectibleColor(int red, int green, int blue){
    collectibleColor = new Color(red, green, blue, 1.0f);
  }
  */

  // move the player into the right spot at the start of a level
  private void playerRespawn(){
    player.transform.position = spawn.transform.position;
  }

  /**
   *  if the player has enugh lives left,
   * decrement the lives variable, update the display, and respawn that player
   */
  public void loseLife(){
    if(lives>1){
      lives--;
	  updateLabel ();
      playerRespawn();
    }else{
      lives--;
      gameOver();
    }
  }

  // load the game over scene
  private void gameOver(){
    loadLevel(4);
  }

	public void resetGame()
	{
		//reset lives count
		changeDifficulty(difficultySetting);
		score = 0;
		updateLabel ();
	}

	public void updateLabel()
	{
		state_label.text = "score: "+score+ ", lives: "+ lives;
	}
}