using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

	public int Cash { get; set;}
	public int Day { get; set;}
	public float PopulationCurrent { get; set;}
	public float PopulationCeiling { get; set;}
	public int JobsCurrent { get; set;}
	public int JobsCeiling { get; set;}
	public float Food { get; set;}

    public int[] buildingCounts;

    private UIController uiController;

	// Use this for initialization
	void Start () {
        buildingCounts = new int[9];
		uiController = GetComponent<UIController>();
		Cash = 7000;
		Food = 6;
		JobsCeiling = 10;
		PopulationCeiling = 10;

		uiController.UpdateCityData();
		uiController.UpdateDayCount();
	}

	public void EndTurn()
	{
		
		Day++;
		CalculateCash();
		CalculatePopulation();
		CalculateJobs();
		CalculateFood();
		Debug.Log("Day ended.");
		uiController.UpdateCityData();
		uiController.UpdateDayCount();
		Debug.LogFormat
			("Jobs: {0}/{1}, Cash: {2}, pop: {3}/{4}, Food: {5}",
			JobsCurrent, JobsCeiling, Cash, PopulationCurrent, PopulationCeiling, Food);
	}

	void CalculateJobs()
	{
		JobsCeiling = buildingCounts[2] * 10;
		JobsCurrent = Mathf.Min((int)PopulationCurrent, JobsCeiling);
	}

	void CalculateCash()
	{
		Cash += JobsCurrent * 2;
	}

	public void DepositCash(int cash)
	{
		Cash += cash;
	}

	void CalculateFood()
	{
		Food += buildingCounts[3] * 1.5f;
		Food -= PopulationCurrent*.25f;

	}

	void CalculatePopulation()
	{
		//If buildings are removed and the Popceiling falls, this levels out the population
		// if (PopulationCurrent > PopulationCeiling) PopulationCurrent = PopulationCeiling;

		PopulationCeiling = buildingCounts[1] * 5;
		if (Food >= PopulationCurrent && PopulationCurrent < PopulationCeiling)
		{
			Food -= PopulationCurrent*.55f;
			PopulationCurrent = Mathf.Min(PopulationCurrent += Food * .25f, PopulationCeiling);
		}
		else if (Food < PopulationCurrent)
		{
			PopulationCurrent -= (PopulationCurrent - Food) * .5f;
		}

	}
}
