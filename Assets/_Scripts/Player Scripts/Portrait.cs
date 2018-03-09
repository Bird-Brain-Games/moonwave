using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour {

    public Color color;
    ColourData playerColour;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Pull the color from the player's stats, step up to parent and then grab playerstats from the child [jack]
        playerColour = GetComponent<Transform>().parent.gameObject.GetComponentInChildren<PlayerStats>().colourData;
        color = playerColour.colour;
        GetComponent<MeshRenderer>().material.color = color;
	}
}
