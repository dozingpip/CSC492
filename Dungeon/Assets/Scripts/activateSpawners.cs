using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateSpawners : MonoBehaviour {
	// list of spawn locations in this room
	List<GameObject> spawners;
	public List<GameObject> enemyPrefabs;
	// Use this for initialization
	void Start () {
		spawners = new List<GameObject>();
		Transform spawnLocations = transform.parent.Find("SpawnLocations");
		// get all the possible spawn locaitons for this room.
		if(spawnLocations!= null){
			foreach(Transform thing in spawnLocations){
				if(thing.gameObject.CompareTag("Spawner")){
					spawners.Add(thing.gameObject);
				}
			}
		}else{
			Debug.Log("no spawn locations");
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player") && spawners.Count>0){
			// pick how many of the spawners to actually use, must be at least 1, 
			// can be up to number of spawners in room
			int spawnHowMany = Random.Range(1, spawners.Count);
			for(int i = 0; i< spawnHowMany; i++){
				// which spawn location to use
				int randomSpawnLocation = Random.Range(0, spawners.Count);
				Transform spawnTransform = spawners[randomSpawnLocation].transform.Find("Portal");

				if(spawnTransform!=null){
					// choose a randomized enemy prefab for spawning
					int randomNumber = Random.Range(0, enemyPrefabs.Count);
					GameObject enemyPrefab = enemyPrefabs[randomNumber];
					// where to spawn the enemy.  Y coord is offset b/c otherwise the enemy would be in the ground
					Vector3 position = new Vector3(spawnTransform.position.x,
					 spawnTransform.position.y+enemyPrefab.transform.localScale.y, spawnTransform.position.z);
					GameObject enemy = GameObject.Instantiate(enemyPrefab, position, spawnTransform.rotation);
					enemy.transform.SetParent(transform.parent);

					// remove this spawn location from the list so it can't be used again
					spawners.Remove(spawners[randomSpawnLocation]);
					// destroy the spawnLocation game object so it isn't visible and can't be used for spawning again
					Destroy(spawnTransform.gameObject);
				}
			}
			
			// remove the unused spawners from the list and from the world.
			int spawnersLeft = spawners.Count;
			for(int i =spawnersLeft-1; i >0; i--){
				Destroy(spawners[i]);
				spawners.Remove(spawners[i]);
			}
		}
	}
}
