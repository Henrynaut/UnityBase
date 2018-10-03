using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class SceneNoVR : MonoBehaviour {

	// Use this for initialization
	//When this script is enabled, it turns the mode back to 2D
	public void Start () {
		StartCoroutine(DeactivatorVR("none"));
	}
	
	public IEnumerator DeactivatorVR(string noVR)
	{
		UnityEngine.XR.XRSettings.LoadDeviceByName(noVR);
		yield return null;
		UnityEngine.XR.XRSettings.enabled = false;
	}
}
