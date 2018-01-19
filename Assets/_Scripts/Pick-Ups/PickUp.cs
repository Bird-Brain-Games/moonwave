using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {


    public bool scanned = false;

    private float rotateX;
    private float rotateY;
    private float rotateZ;


    void Start (){
        rotateX = Random.Range( -1f, 1f);
        rotateY = Random.Range( -1f, 1f);
        rotateZ = Random.Range( -1f, 1f);
    }
    
    void Update () {
		
		// Rotate to make it look fancy
        if (scanned){

            transform.Rotate(new Vector3(rotateX, rotateY, rotateY));
        }
			
	}

	void OnTriggerStay(Collider collider)
    {
        if (scanned){
            //sets what layer we have collided with
            int layer = collider.gameObject.layer;

            //if we have collided with a player.
            if (layer == 9)
            {
                collider.gameObject.GetComponent<Shoot>().m_shotType = Random.Range(1,4);
			
			    Destroy(gameObject, 0);
            }
        }
    }
}
