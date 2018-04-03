using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DungeonGenerator : MonoBehaviour {

    //Room Arrays
    public GameObject _dungeon;

    //Max room count
    public int _max_room_count=25;
    public int _room_count = 1;

    private List<Vector3> _existing_rooms;

    //public List _spawn_locations;

	// Use this for initialization
	void Start () {


        _existing_rooms = new List<Vector3>();

        //Adding starting room by default
        _existing_rooms.Add(new Vector3(0, 0, 0));
        //Randomly spawn dungeon


        int maxCounter = 1000;
        while (_room_count < _max_room_count && maxCounter>0)
        {
            spawnByTag("Room");
            maxCounter -= 1;
        }

        //Add a boss room
        maxCounter = 1000;
        bool boss_placed = false;
        while (!boss_placed && maxCounter > 0)
        {
            boss_placed = spawnByTag("BossRoom");
            maxCounter -= 1;
        }

        maxCounter = 1000;
        while (GameObject.FindGameObjectsWithTag("RoomTag").Length > 0 && maxCounter > 0)
        {
            spawnByTag("EndRoom");
            maxCounter -= 1;
        }


    }

    bool spawnByTag(string p_room_id) {

        bool spawned = false;

        GameObject[] availableRooms = getChildren(_dungeon);

        GameObject[] openRooms = GameObject.FindGameObjectsWithTag("RoomTag");

        //Choose an open location to spawn
        int roomIndex = Random.Range(0, availableRooms.Length);

        if (availableRooms[roomIndex].CompareTag(p_room_id))
        {

            int openIndex = Random.Range(0, openRooms.Length);
            Debug.Log("open Index: " + openIndex);
            GameObject spawnLocation = openRooms[openIndex];
            GameObject roomToSpawn = availableRooms[roomIndex];

            if (!roomExists(spawnLocation.transform.position))
            {
                //Place at an open space
                GameObject newRoom = Instantiate(roomToSpawn,
                    spawnLocation.transform.position,
                    spawnLocation.transform.rotation*roomToSpawn.transform.rotation);

                //Add to banned locations list
                _existing_rooms.Add(newRoom.transform.position);

                _room_count += 1;

                spawned = true;
            }
            else
            {
                Debug.Log("Can't build there");
            }
            spawnLocation.SetActive(false);
        }

        return spawned;
    }

    GameObject[] getChildren(GameObject p_parent)
    {

        List<GameObject> outlist= new List<GameObject>();
        foreach (Transform child in p_parent.transform)
        {
            if (child != _dungeon.transform)
            {
                outlist.Add(child.gameObject);
            }
        }
        return outlist.ToArray();
    }

    private bool roomExists(Vector3 p_location)
    {

        foreach (Vector3 room in _existing_rooms)
        {
            if (room==p_location)
            {
                return true;
            }

        }
        return false;
    }
	

}
