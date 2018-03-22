using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

using System.Collections.Generic;       //Allows us to use Lists.

public class GameManager : MonoBehaviour
{
  //Static instance of GameManager which allows it to be accessed by any other script.
  public static GameManager instance = null;

  // the text label that shows the player how they're doing as far as collecting
  private Text state_label;

  private GameObject player;
  private GameObject spawn;
  // the input field for changing the name of the currently selected object
  public GameObject assetNameField;
  // what is the object currently selected for renaming
  private GameObject assetToRename;
  // the clipboard that displays user feedback
  public GameObject clipboard;
  // the text object for the user's final letter grade in this challenge
  public Text letterGrade;
  // displays amount of time left to complete all the items on the clipboard
  public Text timeLeft;


  // represents the index of the current scene
	private int sceneIndex;
	//number of total scenes
	public int scenes;

  // number of items on the clipboard
  public int numTotalCheckmarks;
  // total amount of time the player is given to finish all the items on the
  // clipboard.
  public float alottedTime;
  // time used up so far in the pursuit of getting on all the items on the
  // clipboard
  private float timer;
  // number of clipboard items completed
  private int itemsCompleted;
  // is the game still going?
  private bool gameOn;

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
    timer = 0;
    itemsCompleted = 1;
    gameOn = true;
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

  // when the game is over, calculate the user's grade and change the letter
  // grade to display it
  void gameOver(){
    switch(itemsCompleted){
      case 0:
        letterGrade.text = "F";
        break;
      case 1:
        letterGrade.text = "F";
        break;
      case 2:
        letterGrade.text = "D";
        break;
      case 3:
        letterGrade.text = "C";
        break;
      case 4:
        letterGrade.text = "B";
        break;
      case 5:
        letterGrade.text = "A";
        break;
      default:
        letterGrade.text = "-";
        break;
    }
    toggleClipboard(true);
    gameOn = false;
  }

  // activate materials on all the objects tagged as textured.
  public void toggleAllMats(){
    GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("textured");
    if(parentObjects.Length>0){
      foreach(GameObject parent in parentObjects){
        parent.GetComponent<toggleMaterial>().toggleMat();
      }
    }

    // in doing this, the user completed the "use materials" goal
    checkCheckmark("materials");
  }

  // make one object move towards the other through use of a NavMesh
  public void attract(GameObject object1, GameObject object2, float distanceThreshold){
    NavMeshAgent agent = object1.GetComponent<NavMeshAgent>();
    Vector3 dest = agent.destination;
    Transform target = object2.transform;
    if(Vector3.Distance(dest, target.position) > distanceThreshold){
      Vector3 destination = target.position;
      agent.destination = destination;
    }
  }

  // combine the meshes of two objects.
  public void combine(GameObject object1, GameObject object2){
    object1.GetComponent<Selectable>().Deselected();
    object2.GetComponent<Selectable>().Deselected();
    Debug.Log("combining!");
    GameObject combinedObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
    Destroy(combinedObj.GetComponent<BoxCollider>());

    MeshFilter meshFilter1 = object1.GetComponent<MeshFilter>();
    MeshFilter meshFilter2 = object2.GetComponent<MeshFilter>();

    if(object1.transform.childCount>0){
      foreach(Transform thing in object1.transform){
        thing.parent.SetParent(combinedObj.transform);
      }
    }

    if(object2.transform.childCount>0){
      foreach(Transform thing in object2.transform){
        thing.parent.SetParent(combinedObj.transform);
      }
    }

    CombineInstance[] combine = new CombineInstance[2];
    combine[0].mesh = meshFilter1.mesh;
    combine[0].transform = Matrix4x4.identity;
    object1.SetActive(false);
    combine[1].mesh = meshFilter2.mesh;
    combine[1].transform = meshFilter1.transform.localToWorldMatrix.inverse*meshFilter2.transform.localToWorldMatrix;
    object2.SetActive(false);

    combinedObj.transform.GetComponent<MeshFilter>().mesh = new Mesh();
    combinedObj.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
    combinedObj.transform.SetParent(object2.transform.parent);
    combinedObj.transform.position = meshFilter2.transform.position;
    combinedObj.transform.rotation = meshFilter2.transform.rotation;
    combinedObj.transform.localScale = meshFilter2.transform.localScale;

    combinedObj.AddComponent<BoxCollider>();
    combinedObj.AddComponent<Selectable>();
    combinedObj.name = object1.name + " and " + object2.name + " combined";
    combinedObj.SetActive(true);
    // completed the npc motion clipboard item!
    GameManager.instance.checkCheckmark("npcMotion");
  }

  // Use this for initialization
  void Start () {
    assetNameField.SetActive(false);
  }

  // Update is called once per frame
  void Update () {
    if(gameOn){
      if(Input.GetButtonDown("Jump")){
        toggleClipboard(true);
      }
      if(Input.GetButtonUp("Jump")){
        toggleClipboard(false);
      }else if(Input.GetButtonUp("Submit")){
        GameObject inputText = assetNameField.transform.Find("InputField").Find("Text").gameObject;
        assetToRename.name = inputText.GetComponent<Text>().text;
        assetNameField.SetActive(false);
        checkCheckmark("nameAssets");
      }else if(Input.GetButtonUp("Cancel")){
        assetToRename = null;
        assetNameField.SetActive(false);
      }
      if(itemsCompleted>= numTotalCheckmarks){
        gameOver();
      }
      if(timer<alottedTime){
        timer+=Time.deltaTime;
        timeLeft.text = (alottedTime -timer).ToString();
      }else{
        gameOver();
      }
    }
  }

  // toggle the clipboard game object to be active or inactive
  void toggleClipboard(bool on){
    clipboard.SetActive(on);
  }

  // update the checkmark next to a relevant completed goal/ item
  public void checkCheckmark(string nameOfToggleObject){
    Toggle toggle = clipboard.transform.Find(nameOfToggleObject).gameObject.GetComponent<Toggle>();
    if(!toggle.isOn){
      toggle.isOn = true;
      itemsCompleted++;
    }
  }

  // start the process of renaming an asset
  public void renameAsset(GameObject asset){
    assetNameField.SetActive(true);
    GameObject inputField = assetNameField.transform.Find("InputField").gameObject;
    GameObject placeholder = inputField.transform.Find("Placeholder").gameObject;
    Text text = placeholder.GetComponent<Text>();
    text.text = asset.name;
    assetToRename = asset;
    inputField.GetComponent<InputField>().ActivateInputField();
  }

  //check if the game is still going.
  public bool isGameOn(){
    return gameOn;
  }
}
