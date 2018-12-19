using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboticSensor : MonoBehaviour {

[SerializeField]
public Vector3 position;
[SerializeField]
public Vector3 rotation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		position = this.transform.position;
		rotation = (this.transform.rotation).eulerAngles;
	}
}
