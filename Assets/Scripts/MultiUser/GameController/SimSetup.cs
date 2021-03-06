﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void DisconnectUser(){
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad(){
        PhotonNetwork.LeaveRoom();
        //Wait until user is actually disconnected from room to load into the menu scene
        while(PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(MultiUserSettings.multiUserSettings.menuScene);
    }
}
