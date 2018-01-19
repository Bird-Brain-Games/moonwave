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
		
		// Move the scanner up and down around moon
		
		transform.Translate(0f, -0.45f, 0f);

		// Set the Uniform for the moon
		parentRend.material.SetFloat("_Scan", transform.localPosition.y);


		// Kill the scanner
		 if (time - startTime > scanTime){
			 
			transform.parent.GetComponent<PickUp>().scanned = true;
        	Destroy(gameObject, 0);  // Destroys the scanner after it has scanned
        }

        time = Time.time;
	}
}
