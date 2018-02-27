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

  // the text that gets added to the state label after collecting everything on the lvl
  private string gotCollectibles = "You got all the things in this level!";
  // the text that gets added to the state label when you hit the goal on the last lvl
  private string youWon = "You won!";

  // how many collectibles are left on the current level.
  private int score = 0;
  // keeping track of how well the player did on previous levels
  private int scoreOnPrevLevels = 0;

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
    GameObject[] scoreObject = GameObject.FindGameObjectsWithTag("score_display");

    if (scoreObject.Length > 0)
    {
      state_label = scoreObject[0].GetComponent<Text> ();
      state_label.text = "";
    }
  }

  //only used to keep track of how many collectibles are on a level at the start
  public void increaseScore()
  {
    score += 1;
    state_label.text = score+" remaining";
  }

  //lower the score and update the state label on how many collectibles remain
  public void decreaseScore()
  {
    score -= 1;
    state_label.text = score+" remaining";
    if (score <= 0)
    {
      if(state_label.text == youWon){
        state_label.text += gotCollectibles;
      }else
        state_label.text = gotCollectibles;
    }

  }

  // Move onto the next level if there is one, if there isn't, player wins.
  public void levelOver()
  {
    if (sceneIndex < scenes - 1) {
      if(score>0){
        scoreOnPrevLevels+=score;
        score = 0;
      }
			SceneManager.LoadScene(sceneIndex + 1);
      sceneIndex+=1;
		}else{
      if(state_label.text==gotCollectibles){
			  state_label.text += youWon;
      }else
        state_label.text = youWon+" "+score+" remaining on this level, "+scoreOnPrevLevels+" on previous levels.";
		}
  }
}
