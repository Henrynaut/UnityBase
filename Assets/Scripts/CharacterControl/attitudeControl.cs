using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attitudeControl : MonoBehaviour {

public float rotationSpeed;
public float rollVelocity = 0.0f;
public float pitchVelocity = 0.0f;
public float yawVelocity = 0.0f;
public float startingRotation = 0.0f;
public GameObject pgtLight;

public bool activateRotation = false;
public bool forwardModePGT = true;

//Set rollModeMultiplier to -1 for forward rotation (clockwise), 1 for reverse (counter-clockwise)
public float rollModeMultiplier = -1.0f;


	// Use this for initialization
	void Start () {
		rotationSpeed = 0.1f;
		rollVelocity = 0.0f;
		pitchVelocity = 0.0f;
		yawVelocity = 0.0f;
		startingRotation = this.transform.rotation.eulerAngles.y;
		
	}
	
	// Update is called once per frame
	// Add in details for changing velocity and acceleration as well
	protected void Update () {

		if(Input.GetKeyDown(KeyCode.F))
			{
				//If F is pressed, toggle activate Rotation
				activateRotation = !activateRotation;
			}

		if(Input.GetKeyDown(KeyCode.Q))
			{
				//If Q is pressed, toggle PGT Mode
				forwardModePGT = !forwardModePGT;

				//If not forwardMode, rotate PGT in reverse direction (counter-clockwise)
				if(!forwardModePGT)
				{
					rollModeMultiplier = 1.0f;
				}
				else
				{
					rollModeMultiplier = -1.0f;
				}
			}

		if(activateRotation == true)
			{
			//If activateRotation, start rotation of PGT drill bit
			rollVelocity = rollModeMultiplier*5.0f;

			// rollVelocity
			transform.Rotate(0f, 0f, rollVelocity, Space.Self);
			// pitchVelocity
			transform.Rotate(pitchVelocity, 0f, 0f, Space.Self);
		
			// yawVelocity
			//Press 1 or 2 to increment or decrement yaw velocity
			// yawVelocity = yawVelocity + rotationSpeed*Input.GetAxis("yawVelocity")*Time.deltaTime;		
			transform.Rotate(0f, yawVelocity, 0f, Space.Self);

			//turn on PGT light
			pgtLight.SetActive(false);
			}

		//Stop Rotational velocities if bool is False
			if(activateRotation == false){
			rollVelocity = 0f;
			pitchVelocity = 0f;
			//Convert this yaw input into logic that controls turning of the wheels
			yawVelocity = 0f;

			//turn off PGT Light
			pgtLight.SetActive(false);
			} 
	}
}
