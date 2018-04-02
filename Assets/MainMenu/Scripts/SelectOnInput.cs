using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;

	// Use this for initialization
	void Start () {
		eventSystem.SetSelectedGameObject(selectedObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("vertical") != 0 && !buttonSelected)       /// Needs to be changed
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
