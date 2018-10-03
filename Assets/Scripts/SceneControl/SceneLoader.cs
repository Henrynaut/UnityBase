using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Include to change Unity Scenes
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//If script is enabled, the scene changes to VRMarsScene
		SceneManager.LoadScene("Assets/Scenes/VRMarsScene");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
