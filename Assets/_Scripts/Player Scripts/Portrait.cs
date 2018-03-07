using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour {

    public Color color;
    ColourData playerColour;
    MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		// Pull the color from the player's stats, step up to parent and then grab playerstats from the child [jack]
        playerColour = GetComponent<Transform>().parent.gameObject.GetComponentInChildren<PlayerStats>().colourData;
        meshRenderer =  GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        color = playerColour.colour;
        meshRenderer.material.color = color;
	}
}
