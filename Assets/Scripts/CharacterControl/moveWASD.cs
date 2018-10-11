using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveWASD: MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(
			Input.GetAxis("Horizontal")*Time.deltaTime,
			Input.GetAxis("UpDown")*Time.deltaTime, 
			Input.GetAxis("Vertical")*Time.deltaTime
		);
	}
}
