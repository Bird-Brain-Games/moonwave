using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDuration : MonoBehaviour {

    public float MaxDuration;
    float currentDuration;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		if (currentDuration < MaxDuration)
        {
            currentDuration += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject, 0);
        }
	}
}
