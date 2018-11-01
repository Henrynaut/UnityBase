using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveJSON : MonoBehaviour {

	// Use this for initialization
	//When Enabled, this script saves all building object data to a JSON
	//save file that can be loaded into the scene as a sort of city layout

	// [Serializable]
	// public class SceneData
	// {
	// 	public Vector3 location;
	// 	public Vector3 rotation;
	// 	public int buildingID;
	// }

	void Start () {
		//Find all game objects with "(Clone)" in the Scene
		//Save Location, Rotation, & BuildingID of each object
		//Convert the data into a json format
		// location = new Vector3(1.0f,2.0f,3.0f);
		// string json = JsonUtility.ToJson(SceneData); 
		//Need to convert this into an Unreal-type format as well
	}
	
}
