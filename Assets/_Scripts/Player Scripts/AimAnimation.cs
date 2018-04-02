using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAnimation : MonoBehaviour {

	// Animator component of the player
    public Animator m_Animator;
	

	//The accessers to our controller script
    Controls controls;
    Vector2 aimDir, moveDir;
    Vector3 normalDir, reverseDir;
	float angle, shootingAngle;
	
	// Use this for initialization
	void Start () {
		//m_Animator = GetComponentInChildren<Animator>();
        controls = GetComponent<Controls>();
        normalDir = new Vector3(1f, 1f, 1f);
        reverseDir = new Vector3(-1f, 1f, 1f);

	}
	
	// Update is called once per frame
	void Update () {
		aimDir = controls.GetAim();
        moveDir = controls.GetMove();

        // Caculate the direction of the animation
        if (aimDir.sqrMagnitude > 0f)
        {
            angle = Vector3.Dot(transform.up, Vector3.up);
            shootingAngle = ((aimDir.y + 1) / 2);

            // If the angle is below zero the player is upside down
            // Checks what side they are shooting on and changes scale accordingly
            if (angle < 0)
            {
                transform.localScale = (aimDir.x > 0) ? normalDir : reverseDir;
                // Swap 0 to 1, blend values for upside down
                shootingAngle =  1 - shootingAngle;
            }
            else    // Flipped scale when not upside down
                transform.localScale = (aimDir.x < 0) ? normalDir : reverseDir;

            m_Animator.SetFloat("Aim Direction Y", shootingAngle);

        }
        else if (moveDir.sqrMagnitude > 0f)
        {
            transform.localScale = (Vector3.Dot(transform.right, moveDir) < Vector3.Dot(-transform.right, moveDir))
                ? normalDir : reverseDir;
        }
	}
}
