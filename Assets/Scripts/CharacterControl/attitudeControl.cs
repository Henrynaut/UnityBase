using System.Collections;
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
	protected void Update () {
		if(activateRotation == true)
		{
		// rollVelocity
		transform.Rotate(0f, 0f, rollVelocity, Space.Self);
		// pitchVelocity
		transform.Rotate(pitchVelocity, 0f, 0f, Space.Self);
	
		// yawVelocity
		// yawVelocity = yawVelocity + rotationSpeed*Input.GetAxis("yawVelocity")*Time.deltaTime;		
		transform.Rotate(0f, yawVelocity, 0f, Space.Self);
		}
		//Stop Rotation if bool is False
		if(activateRotation == false){
		rollVelocity = 0f;
		pitchVelocity = 0f;
		yawVelocity = 0f;
		}  
	}
}
