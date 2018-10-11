using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveWASD: MonoBehaviour {

public float characterSpeed;

	// Use this for initialization
	void Start () {
		characterSpeed = 3f;		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(
			characterSpeed*Input.GetAxis("Horizontal")*Time.deltaTime,
			characterSpeed*Input.GetAxis("UpDown")*Time.deltaTime, 
			characterSpeed*Input.GetAxis("Vertical")*Time.deltaTime
		);
	}
}
