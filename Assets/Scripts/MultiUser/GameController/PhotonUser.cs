using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonUser : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, SimSetup.SS.spawnPoints.Length);
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
