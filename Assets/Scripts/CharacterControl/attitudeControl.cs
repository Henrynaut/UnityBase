﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attitudeControl : MonoBehaviour {

public float rotationSpeed;
public float rollVelocity = 0.0f;
public float pitchVelocity = 0.0f;
public float yawVelocity = 0.0f;

public bool activateRotation = true;

	// Use this for initialization
	void Start () {
		rotationSpeed = 0.1f;
		rollVelocity = 0.0f;
		pitchVelocity = 0.0f;
		yawVelocity = 0.0f;
	}
	
	// Update is called once per frame
	// Add in details for changing velocity and acceleration as well
	protected void Update () {
		if(activateRotation == true)
		{
		// rollVelocity
		transform.Rotate(0f, 0f, rollVelocity, Space.Self);
		// pitchVelocity
		transform.Rotate(pitchVelocity, 0f, 0f, Space.Self);
	
		// yawVelocity
		//Press 1 or 2 to increment or decrement yaw velocity
		yawVelocity = yawVelocity + rotationSpeed*Input.GetAxis("yawVelocity")*Time.deltaTime;		
		transform.Rotate(0f, yawVelocity, 0f, Space.Self);
		}
		//Stop Rotational velocities if bool is False
		if(activateRotation == false){
		rollVelocity = 0f;
		pitchVelocity = 0f;
		//Convert this yaw input into logic that controls turning of the wheels
		yawVelocity = 0f;
		}  
	}
}
