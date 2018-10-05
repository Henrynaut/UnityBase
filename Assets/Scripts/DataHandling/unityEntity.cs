using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This file parses the json Entity in a serialized manner
[SerializeField]
public class unityEntity : MonoBehaviour {

	public entityParameters[] Parameters;
	public string Name;
	public string Type;
	public bool Loadable_Mesh;
	public bool Generate_Collider;
	public string Mesh_Path;
	public string Material_Path;
	public string Prefab_Path;
	public float Scale;
	public float Mass;

	// public override string ToString();
}
