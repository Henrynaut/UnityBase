using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class SceneVR : MonoBehaviour {

	// Use this for initialization
	//When this script is enabled, it activates OpenVR Mode (Rift/VIVE/WMR Compatible)
	public void Start () {
		StartCoroutine(ActivatorVR("OpenVR"));

	}

	public IEnumerator ActivatorVR(string sceneVR)
	{
		UnityEngine.XR.XRSettings.LoadDeviceByName(sceneVR);
		yield return null;
		UnityEngine.XR.XRSettings.enabled = true;
	}
	

}
