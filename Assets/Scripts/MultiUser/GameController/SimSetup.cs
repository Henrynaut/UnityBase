using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimSetup : MonoBehaviour
{
    //Singleton
    public static SimSetup SS;

    // public TextMeshProUGUI energyDisplay;
    public Transform[] spawnPoints;

    private void OnEnable(){
        if(SimSetup.SS == null){
            SimSetup.SS = this;
        }
    }
}
