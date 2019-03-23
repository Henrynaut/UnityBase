using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour {
    private PhotonView PV;
    public int characterValue;
    public GameObject myCharacter;

    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();

        //Only send from local player
        if(PV.IsMine){
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, UserInfo.UI.mySelectedCharacter);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter){
        //Save the Character Selection ID and instantiate
        characterValue = whichCharacter;
        myCharacter = Instantiate(UserInfo.UI.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
    }
}
