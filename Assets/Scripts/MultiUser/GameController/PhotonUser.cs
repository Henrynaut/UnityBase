using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonUser : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;

    //Photon User Singleton
    public static PhotonUser PU;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        PhotonUser.PU = this;                 //Create singleton reference to this user script

        //Spawn at UserID Defined Spawn Point (from PhotonView Owner Actor Number - 1, Pv.OwnerActorNr)
            int spawnPicker = PV.OwnerActorNr - 1;
        //Spawn at Random Spawn Point
            // int spawnPicker = Random.Range(0, SimSetup.SS.spawnPoints.Length);
        if(PV.IsMine){
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "UserAvatar"),
                 SimSetup.SS.spawnPoints[spawnPicker].position, SimSetup.SS.spawnPoints[spawnPicker].rotation, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
