using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthScan : MonoBehaviour {

	public Renderer parentRend;
	public bool scanComplete;

	float scanTime = 0.6f;
    float startTime;
    float time;

	// Use this for initialization
	void Start () {
		
		startTime = Time.time;
        time = Time.time;
		transform.localPosition = new Vector3 (transform.localPosition.x, 1.2f, transform.localPosition.z);
		parentRend = transform.parent.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		// Move the scanner down the pickup		
		transform.Translate(0f, -0.45f, 0f);

		// Set the Uniform for the scanning shader
		parentRend.material.SetFloat("_Scan", transform.localPosition.y);


		// Kill the scanner after it has scanned
		 if (time - startTime > scanTime){
			 
			transform.parent.GetComponent<PickUp>().scanned = true;
        	Destroy(gameObject, 0);
        }

        time = Time.time;
	}
}
