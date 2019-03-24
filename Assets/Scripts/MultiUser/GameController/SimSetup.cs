using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimSetup : MonoBehaviour
{
    //Singleton
    public static SimSetup SS;

    public Transform[] spawnPoints;

    private void OnEnable(){

        if(SimSetup.SS == null){
            SimSetup.SS = this;
        }
    }
}
