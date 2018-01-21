using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    Spawner_Available[] spawners;
    Vector3[] spawnPoints;
	// Use this for initialization
	void Start () {
        spawners = GetComponentsInChildren<Spawner_Available>();
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
        for (int i = 0; i < spawners.Length; i++)
        {
            int tempColliders = spawners[i].GetNumColliders();
            if (tempColliders == 0)
            {
                available[size] = i;
                size++;
            }
            else if (tempColliders < leastCollisions)
            {
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
	
	// Update is called once per frame
	void Update () {
		
	}
}
