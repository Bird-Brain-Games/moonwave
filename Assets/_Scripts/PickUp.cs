using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		
		// Rotate to make it look fancy
		transform.Rotate(new Vector3(1f, 0.7f, 1f));		
	}

	void OnTriggerEnter(Collider collider)
    {
        //sets what layer we have collided with
        int layer = collider.gameObject.layer;

        //if we have collided with a player.
        if (layer == 9)
        {
            collider.gameObject.GetComponent<Shoot>().m_shotType = 2;
			//collider.gameObject.GetComponent<Shoot>().m_bulletDelay = 0.05f;
			Destroy(gameObject, 0);
        }
    }
}
