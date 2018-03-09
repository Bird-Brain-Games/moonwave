using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAnimation : MonoBehaviour {

	// Animator component of the player
    public Animator m_Animator;
	

	//The accessers to our controller script
    Controls controls;
    Vector2 aimDir, moveDir;
	float angle, shootingAngle;
	
	// Use this for initialization
	void Start () {
		//m_Animator = GetComponentInChildren<Animator>();
        controls = GetComponent<Controls>();
	}
	
	// Update is called once per frame
	void Update () {
		aimDir = controls.GetAim();
        moveDir = controls.GetMove();

        /////// Shooting Animation Stuff /////////

        angle = Vector3.Dot(transform.up, Vector3.up);
        
		// Caculate the direction of the animation
        shootingAngle = ((aimDir.y + 1) / 2);

        // If the angle is below zero the player is upside down
        if (angle < 0)
        {
            // Checks what side they are shooting on and changes scale accordingly
            if (aimDir.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f); //(c# code)
            }
            // Swap 0 to 1, blend values for upside down
            shootingAngle =  1 - shootingAngle;

        }
        else
        {
            // Flipped scale when not upside down
            if (aimDir.x > 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f); //(c# code)
            }
        }

        m_Animator.SetFloat("Aim Direction Y", shootingAngle);
        
	}
}
