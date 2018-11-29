using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour {

	[SerializeField]
	private City city;
	[SerializeField]
	private UIController uiController;
	[SerializeField]
	private Building[] buildings;
	[SerializeField]
	private Board board;
	private Building selectedBuilding;
	[SerializeField]
	private CameraController cameraController;
	private Vector3 mouseClickPos;
	private int buildingID;
	// Use this for initialization
	void Start () {

        //city = GetComponent<City>();
            
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift) && selectedBuilding != null)
		{
			InteractWithBoard(0);
		}
		else if (Input.GetMouseButtonDown(0) && selectedBuilding != null)
		{
			InteractWithBoard(0);
		}

		if (Input.GetMouseButton(1))
		{
			InteractWithBoard(1);
		}
	}

	void InteractWithBoard(int action)
	{
		mouseClickPos = Input.mousePosition;
		Ray ray = cameraController.cameraRaycast(mouseClickPos);
		// Debug.Log(ray);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			Debug.Log("Hit True");
			Vector3 gridPosition = board.CalculateGridPosition(hit.point);
			Debug.Log(gridPosition);
			//Make sure cursor isn't over a UI object and a building isn't there either
			if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())	
			{	
				//If left click and there is no building at position, add building
				if (action == 0 && board.CheckForBuildingAtPosition(gridPosition) == null)
				{
					if (city.Cash >= selectedBuilding.cost)
					{
						city.DepositCash(-selectedBuilding.cost);
						uiController.UpdateCityData();
						city.buildingCounts[selectedBuilding.id]++;
						board.AddBuilding(selectedBuilding, gridPosition);
					}
				}

				//Else, if Right click && there is a building,
				//remove building and refund half of cost & update UI
				else if (action == 1 && board.CheckForBuildingAtPosition(gridPosition) != null)
				{
					city.DepositCash(board.CheckForBuildingAtPosition(gridPosition).cost/2);
					buildingID = board.CheckForBuildingID(gridPosition);
					board.RemoveBuilding(gridPosition);
					//Need to add in a line to update building ID with the ID of the building that is right clicked
					city.buildingCounts[buildingID]--;
					uiController.UpdateCityData();
				}
			}
		}
	}

	public void EnableBuilder(int building)
	{
		selectedBuilding = buildings[building];
		Debug.Log("Selected building: " + selectedBuilding.buildingName);
	}
}
