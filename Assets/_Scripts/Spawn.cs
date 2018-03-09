using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    GameObject spawnerContainer;
    Spawner_Available[] spawners;
    Vector3[] spawnPoints;
	// Use this for initialization

	void Awake ()
    {
        //Moved all of this code from start to Awake
        spawnerContainer = GameObject.Find("Spawner holder");
        spawners = spawnerContainer.GetComponentsInChildren<Spawner_Available>();
        spawnPoints = new Vector3[spawners.Length];
        for (int i = 0; i < spawners.Length; i++)
        {
            spawnPoints[i] = spawners[i].GetComponent<Transform>().position;
        }
    }

    public Vector3 getSpawnPoint()
    {
        int[] available = new int[4];
        int size = 0;

        int leastCollisions = 0;
        int spot = 0;

        //loop through each spawn point and check which has the least collisions
        for (int i = 0; i < spawners.Length; i++)
        {
            //the number of players colliding with the given spawn point
            int tempColliders = spawners[i].GetNumColliders();
            if (tempColliders == 0)
            {
                available[size] = i;
                size++;
            }
            else if (tempColliders < leastCollisions)
            {
                Debug.Log("Collision less then least collisions");
                spot = i;
                leastCollisions = tempColliders;
            }
        }
        if (size > 1)
        {
            int temp = Random.Range(0, size);
            return spawnPoints[available[temp]];
        }
        return spawnPoints[spot];
    }

    //used to get the spawn point at a given index
    public Vector3 getInitialSpawn(int index)
    {
        if (index < spawnPoints.Length)
        {
            return spawnPoints[index];
        }
        else
        {
            Debug.Log("Error: spawn index out of bounds, returning 0, 0, 0");
            return new Vector3();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
