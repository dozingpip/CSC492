using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Selectable : MonoBehaviour {
	// marker prefab to use
	GameObject marker;
	// is the object currently combining with another object?
	bool combining = false;
	// obstacle avoidance for the navmeshagent on this object (if the object has no navmesh)
	public float obstacleAvoidanceRadius = 0.05f;
	public string prefabForMarker = "marker";
	public Vector3 markerOffset = new Vector3(0, 0, 0);
	//how mnay times this object can be combined with another at maximum
	public int combineLimit = 4;
	//how many times has this object been combined with something else?
	private int combineCount;

	void Start(){
		marker = Resources.Load(prefabForMarker) as GameObject;
	}

	// the name of the instantiated marker
	string markName;
	public void Selected(){
		// do something to show the player that this thing is chosen
		if(!GetComponent<NavMeshAgent>()){
			gameObject.AddComponent<NavMeshAgent>();
			GetComponent<NavMeshAgent>().radius = obstacleAvoidanceRadius;
		}
		GameObject mark = Instantiate(marker, transform.position + markerOffset, Quaternion.identity);
		mark.transform.parent = transform;
		mark.transform.localScale = marker.transform.localScale;
		mark.transform.rotation = marker.transform.rotation;
		markName = mark.name;
	}

	// slight change if this object got selected second
	public void SelectedSecond(){
		GetComponent<BoxCollider>().isTrigger = true;
		Selected();
	}

	// this object got deselected, make sure it's back to normal
	public void Deselected(){
		Destroy(transform.Find(markName).gameObject);
		GetComponent<BoxCollider>().isTrigger = false;
	}

	// getting triggered on a selectable object means the attract() has finished
	public void OnTriggerEnter(Collider other){
		Debug.Log("Hey! you!");
		// if the other object that made this object's trigger go off is selectable
		Selectable otherSelect = other.gameObject.GetComponent<Selectable>();
		if(otherSelect && !combining && GetComponent<BoxCollider>().isTrigger){
			// the attract is done at this point, so can combine the meshes
			GameManager.instance.combine(otherSelect.gameObject, gameObject);
			GetComponent<BoxCollider>().isTrigger = false;
			combining = true;
		}
	}

	// is this object selected?
	public bool isSelected(){
		return transform.Find(markName);
	}

	//can this object be combined with anything anymore, according to the limit
	public bool canBeCombinedMore(){
		return(combineCount<combineLimit);
	}

	public void setCombineCount(int newCombineCount){
		combineCount = newCombineCount;
	}

	public int getCombineCount(){
		return combineCount;
	}
}
