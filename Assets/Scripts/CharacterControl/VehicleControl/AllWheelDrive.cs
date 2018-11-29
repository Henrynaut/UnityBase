using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllWheelDrive : MonoBehaviour {

	private WheelCollider[] wheels;

	public float maxAngle = 30;
	public float maxTorque = 300;
	public GameObject wheelShape;
    public GameObject boxCollider;

	// here we find all the WheelColliders down in the hierarchy
	public void Start()
	{
		wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < wheels.Length; ++i) 
		{
			var wheel = wheels [i];

			// create wheel shapes only when needed
			if (wheelShape != null)
			{
				var ws = GameObject.Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;

                var bc = GameObject.Instantiate (boxCollider);
                bc.transform.parent = ws.transform;
			}
			// create box colliders only when needed
			// if (boxCollider != null)
			// {
			// 	var ws = GameObject.Instantiate (boxCollider);
			// 	ws.transform.parent = wheel.transform;
			// }

		}
	}

	// this is a really simple approach to updating wheels
	// here we simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero
	// this helps us to figure our which wheels are front ones and which are rear
	public void Update()
	{
		float angle = maxAngle * Input.GetAxis("Horizontal");
		float torque = maxTorque * Input.GetAxis("Vertical");

		foreach (WheelCollider wheel in wheels)
		{
			// a simple car where front wheels steer and all wheels drive

            wheel.motorTorque = torque;
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;

			// Add in functionality for MMSEV 6 wheel control

			// update visual wheels if any
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// assume that the only child of the wheelcollider is the wheel shape
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}

		}
	}
}
