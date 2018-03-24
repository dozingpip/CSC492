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
  // the input field for changing the name of the currently selected object
  private GameObject assetNameField;
  // what is the object currently selected for renaming
  private GameObject assetToRename;
  // the clipboard that displays user feedback
  private GameObject clipboard;
  // the text object for the user's final letter grade in this challenge
  private Text letterGrade;
  // displays amount of time left to complete all the items on the clipboard
  private Text timeLeft;

  private GameObject playerInteractionLine;


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
  private bool gameOn = false;

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

    GameObject[] cbTemp = GameObject.FindGameObjectsWithTag("clipboard");
    if(cbTemp.Length>0){
      clipboard = cbTemp[0];
      letterGrade = clipboard.transform.Find("letter").gameObject.GetComponent<Text>();

      // timer and game should only start if the clipboard is present (not in main menu)
      timer = 0;
      itemsCompleted = 1;
      gameOn = true;
    }

    GameObject[] tempTime = GameObject.FindGameObjectsWithTag("timeDisplay");
    if(tempTime.Length>0){
      timeLeft = tempTime[0].GetComponent<Text>();
    }

    GameObject[] playerInteractTemp = GameObject.FindGameObjectsWithTag("interactLine");
    if(playerInteractTemp.Length>0){
      playerInteractionLine = playerInteractTemp[0];
    }

    GameObject[] assetRenameFieldTemp = GameObject.FindGameObjectsWithTag("assetRenameField");
    if(assetRenameFieldTemp.Length>0){
      assetNameField = assetRenameFieldTemp[0];
      assetNameField.SetActive(false);
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

    playerInteractionLine.SetActive(true);
    // in doing this, the user completed the "use materials" goal
    finshedClipboardItem("materials");
  }

  // make one object move towards the other through use of a NavMesh and agent
  public void attract(GameObject object1, GameObject object2, float distanceThreshold){
    NavMeshAgent agent = object1.GetComponent<NavMeshAgent>();
    Vector3 dest = agent.destination;
    Transform target = object2.transform;
    Vector3 destination = target.position;
    agent.destination = destination;
  }

  // combine the meshes of two objects.
  public void combine(GameObject object1, GameObject object2){
    // deselect the objects so the particle system markers goes away
    object1.GetComponent<Selectable>().Deselected();
    object2.GetComponent<Selectable>().Deselected();
    Debug.Log("combining!");
    // save the parent of object2 for later
    Transform parent = object2.transform.parent;
    // set the parents of the objects to coombine to null, so they don't have
    // any weird local transformations
    object2.transform.SetParent(null);
    object1.transform.SetParent(null);
    // get the number of times each object has been combined and find the max
    //  of the two
    int maxCombineCount = Mathf.Max(object2.GetComponent<Selectable>().getCombineCount(), object1.GetComponent<Selectable>().getCombineCount());
    // create a new empty game object
    GameObject combinedObj = new GameObject();

    // get the meshfilters of both objects to combine
    MeshFilter meshFilter1 = object1.GetComponent<MeshFilter>();
    MeshFilter meshFilter2 = object2.GetComponent<MeshFilter>();

    // make a combine instance of length 2 (the number of meshes to combine)
    CombineInstance[] combine = new CombineInstance[2];

    // make a materials array for the number of meshes (2)
    Material[] materials = new Material[2];

    // the first mesh to combine is set
    combine[0].mesh = meshFilter1.mesh;
    // the goal is to have the first object be at the position where it first
    // entered the second object's collider, and to do that a matrix
    // transformation is necessary, to move the transform back to towards the
    // origin by the tranformation the second mesh is away from the origin
    // (so inverse of the second mesh) and then move it over again
    // by how far the first mesh is from the origin
    combine[0].transform = meshFilter2.transform.localToWorldMatrix.inverse*meshFilter1.transform.localToWorldMatrix;
    materials[0] = meshFilter1.GetComponent<MeshRenderer>().material;
    object1.SetActive(false);
    combine[1].mesh = meshFilter2.mesh;
    combine[1].transform = Matrix4x4.identity;
    materials[1] = meshFilter2.GetComponent<MeshRenderer>().material;
    object2.SetActive(false);

    combinedObj.AddComponent<MeshFilter>();
    combinedObj.AddComponent<MeshRenderer>();
    combinedObj.GetComponent<MeshRenderer>().materials = materials;
    combinedObj.transform.GetComponent<MeshFilter>().mesh = new Mesh();
    combinedObj.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

    // put the combined mesh at the second mesh's position, rotation, and scale
    combinedObj.transform.position = meshFilter2.transform.position;
    combinedObj.transform.rotation = meshFilter2.transform.rotation;
    combinedObj.transform.localScale = meshFilter2.transform.localScale;
    combinedObj.transform.SetParent(parent);

    // move all of the old objects' children into this new combined object
    if(object1.transform.childCount>0){
      foreach(Transform thing in object1.transform){
        thing.SetParent(combinedObj.transform);
      }
    }

    if(object2.transform.childCount>0){
      foreach(Transform thing in object2.transform){
        thing.SetParent(combinedObj.transform);
      }
    }
    combinedObj.AddComponent<BoxCollider>();
    // make the new combined object selectable again, so it can be combined more
    Selectable selectable = combinedObj.AddComponent<Selectable>();
    selectable.setCombineCount(maxCombineCount);
    // set up the new mesh's navmesh agent
    NavMeshAgent navmeshagent = combinedObj.AddComponent<NavMeshAgent>();
    navmeshagent.radius = selectable.obstacleAvoidanceRadius;
    navmeshagent.baseOffset = Mathf.Max(object1.GetComponent<NavMeshAgent>().baseOffset, object1.GetComponent<NavMeshAgent>().baseOffset);
    combinedObj.name = object1.name + " + " + object2.name;
    combinedObj.SetActive(true);
    // completed the npc motion clipboard item!
    GameManager.instance.finshedClipboardItem("npcMotion");
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
        Text inputText = assetNameField.transform.Find("InputField").Find("Text").gameObject.GetComponent<Text>();
        if(inputText.text != "" && inputText.text != " "){
          assetToRename.name = inputText.text;
          assetNameField.SetActive(false);
          finshedClipboardItem("nameAssets");
        }
      }else if(Input.GetButtonUp("Cancel")){
        assetToRename = null;
        assetNameField.SetActive(false);
      }
      if(itemsCompleted>= numTotalCheckmarks){
        gameOver();
      }
      if(timer<alottedTime){
        timer+=Time.deltaTime;
        timeLeft.text = (alottedTime - timer).ToString();
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
  public void finshedClipboardItem(string nameOfToggleObject){
    Toggle toggle = clipboard.transform.Find(nameOfToggleObject).gameObject.GetComponent<Toggle>();
    if(!toggle.isOn){
      toggle.isOn = true;
      itemsCompleted++;
    }
  }

  public bool isClipboardItemDone(string nameOfToggleObject){
    return clipboard.transform.Find(nameOfToggleObject).gameObject.GetComponent<Toggle>().isOn;
  }

  // start the process of renaming an asset
  public void renameAsset(GameObject asset){
    assetNameField.SetActive(true);
    GameObject inputFieldObject = assetNameField.transform.Find("InputField").gameObject;
    GameObject placeholder = inputFieldObject.transform.Find("Placeholder").gameObject;
    Text text = placeholder.GetComponent<Text>();
    text.text = asset.name;
    assetToRename = asset;
    InputField inputField = inputFieldObject.GetComponent<InputField>();
    // reset the input field's contents
    inputField.Select();
    inputField.text = "";
    // put the cursor in the field, anything the user types will appear in here
    inputField.ActivateInputField();
  }

  //check if the game is still going.
  public bool isGameOn(){
    return gameOn;
  }
}
