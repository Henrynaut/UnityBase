using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attitudeControl : MonoBehaviour {

public float rotationSpeed;
public float roll = 0.0f;
public float pitch = 0.0f;
public float yaw = 0.0f;

public bool activateRotation = true;

	// Use this for initialization
	void Start () {
		rotationSpeed = 0.1f;
		roll = 0.0f;
		pitch = 0.0f;
		yaw = 0.0f;
	}
	
	// Update is called once per frame
	protected void Update () {
		if(activateRotation == true)
		{
		// roll
		transform.Rotate(0f, 0f, roll, Space.Self);
		// pitch
		transform.Rotate(pitch, 0f, 0f, Space.Self);
	
		// yaw
		yaw = yaw + rotationSpeed*Input.GetAxis("Yaw")*Time.deltaTime;		
		transform.Rotate(0f, yaw, 0f, Space.Self);
		}
		//Stop Rotation if bool is False
		if(activateRotation == false){
		roll = 0f;
		pitch = 0f;
		yaw = 0f;
		}  
	}
}
