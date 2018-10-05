using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class loadJSON : MonoBehaviour {

	[SerializeField]
	private string jsonPath;

	private parserJSON entityCollection;

	[ContextMenu("Load Entities")]
	private void loadEntities()
	{
		//Read JSON file
		using (StreamReader stream = new StreamReader(jsonPath))
		{
			//Store entities into string
			string json = stream.ReadToEnd();
			entityCollection = JsonUtility.FromJson<parserJSON>(json);
		}

		//Place Entity into Unity Scene

		//Output to Console 
		Debug.Log("Entities Loaded: " + entityCollection.Entities.Length);
	} 
}
