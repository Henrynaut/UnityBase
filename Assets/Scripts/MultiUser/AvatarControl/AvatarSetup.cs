using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AvatarSetup : MonoBehaviour {
    private PhotonView PV;
    public int characterValue;
    public string usernameString;
    public GameObject myCharacter;
    public TextMeshProUGUI myUsername;

    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();

        //Only send from local player
        if(PV.IsMine){
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, UserInfo.UI.mySelectedCharacter);
        }
    }

    public void OnUsernameChanged(string usernameIn){
		usernameString = usernameIn;
	}

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter){
        //Save the Character Selection ID and instantiate
        characterValue = whichCharacter;
        myCharacter = Instantiate(UserInfo.UI.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
        // myUsername.text = usernameString;
    }
}
