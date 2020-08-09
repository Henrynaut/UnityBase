using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class AvatarSetup : MonoBehaviour {
    private PhotonView PV;
    private PhotonLobbyCustomMatch lobby;


    public int characterValue;
    public string usernameString;
    private GameObject myCharacter;
    public GameObject myUsername;
    public GameObject usernameText;
    public int userOxygen;
    public int userEnergy;

    public Camera myCamera;
    public AudioListener myAL;
    public Animator animator;

    string[] labels = {
    "WhiteAvatar",
    "GreenAvatar",
    "RedAvatar",
    "BlueAvatar",
    "WhiteAvatar"
    };

    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        lobby = GetComponent<PhotonLobbyCustomMatch>();


        //Only send from local player
        if(PV.IsMine){

            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, UserInfo.UI.mySelectedCharacter);
            //Set username to desired string from lobby
            PV.RPC("RPC_AddUsername", RpcTarget.AllBuffered, PhotonLobbyCustomMatch.lobby.usernameString);
        }
        //If another user's character, then destroy extra camera and audio listener
        else{
            Destroy(myCamera);
            Destroy(myAL);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter){
        //Save the Character Selection ID and instantiate
        characterValue = whichCharacter;

        //Instantiate with Animations, select Avatar from list of string named labels
        //If character already exists, don't instantiate
        myCharacter = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", labels[whichCharacter]), transform.position, transform.rotation);
        myCharacter.transform.parent = transform;
        animator = myCharacter.GetComponent<Animator>();

        //Instantiate Username Label
        myUsername = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "usernameLabel"), transform.position, transform.rotation);
        myUsername.transform.parent = myCharacter.transform;
    }

    [PunRPC]
    void RPC_AddUsername(string inputString){
        usernameText = GameObject.Find("usernameText");
        usernameText.name = inputString;
        usernameText.GetComponent<TextMeshPro>().text = inputString;
        Debug.Log(inputString);
    }
}
