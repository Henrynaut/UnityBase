using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
	public int id;
	public int cost;
	public string buildingName;

    public static explicit operator GameObject(Building v)
    {
        throw new NotImplementedException();
    }
}

