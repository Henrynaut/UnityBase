using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PhotonRoomCustomMatch : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room info (Singleton variable)
    public static PhotonRoomCustomMatch room;
    private PhotonView PV;

    public bool isSimLoaded;
    public int currentScene;

    //Player Info
    Player[] photonUsers;
    public int usersInRoom;
    public int myNumberInRoom;

    public int usersInSim;

    //Delayed Start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxUsers;
    private float atMaxUsers;
    private float timeToStart;

    public GameObject lobbyGO;
    public GameObject roomGO;
    public Transform usersPanel;
    public GameObject userListingPrefab;
    public GameObject startButton;

    private void Awake(){
        if(PhotonRoomCustomMatch.room == null)
        {
            PhotonRoomCustomMatch.room = this;
        }
        else
        {
            //Replace with new instance if non-matching
            if(PhotonRoomCustomMatch.room != this){
                Destroy(PhotonRoomCustomMatch.room.gameObject);
                PhotonRoomCustomMatch.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable(){
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        //Call the Event Listener
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable(){
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    // Use this for initialization
	void Start () {
		PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxUsers = startingTime;
        atMaxUsers = 6;
        timeToStart = startingTime;
	}

    // Update is called once per frame
    void Update () {
        if(MultiUserSettings.multiUserSettings.delayStart){
            if(usersInRoom == 1){
                RestartTimer();
            }
            if(!isSimLoaded){
                //Check to see if we are ready to start counting down
                if(readyToStart){
                    atMaxUsers -= Time.deltaTime;   //Decrement Time
                    lessThanMaxUsers = atMaxUsers;
                    timeToStart = atMaxUsers;
                }
                else if (readyToCount){
                    lessThanMaxUsers -= Time.deltaTime;
                    timeToStart = lessThanMaxUsers;
                }
                Debug.Log("Display time to start to the users" + timeToStart);
                if(timeToStart <= 0){
                    //Once timer hits 0, start the simulation
                    StartSim();
                }

            }
        }
    }

	public override void OnJoinedRoom(){
		base.OnJoinedRoom();
        Debug.Log("You are now in a room.");

        lobbyGO.SetActive(false);
        roomGO.SetActive(true);
        if(PhotonNetwork.IsMasterClient){
            startButton.SetActive(true);
        }

        ClearUserListings();
        ListUsers();

        //Get list of users in room
        photonUsers = PhotonNetwork.PlayerList;
        usersInRoom = photonUsers.Length;
        myNumberInRoom = usersInRoom;

        //For Delay Start Only
        if(MultiUserSettings.multiUserSettings.delayStart){
            Debug.Log("Displaying users in room out of max users possible (" + usersInRoom + ":" + MultiUserSettings.multiUserSettings.maxUsers + ")");
            if(usersInRoom > 1){
                readyToCount = true;
            }
            if(usersInRoom == MultiUserSettings.multiUserSettings.maxUsers){
                readyToStart = true;
                //If not the Master Client, return
                if (!PhotonNetwork.IsMasterClient){
                    return;
                }
                //Closes the room so that no new users can join the room until it is re-opened
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
	}

    void ClearUserListings(){
        //Remove each user from panel starting from the end
        for(int i = usersPanel.childCount -1; i>= 0; i--){
            Destroy(usersPanel.GetChild(i).gameObject);
        }
    }

    void ListUsers(){
        if(PhotonNetwork.InRoom){
            foreach(Player user in PhotonNetwork.PlayerList){
                GameObject tempListing = Instantiate(userListingPrefab, usersPanel);
                TextMeshProUGUI tempText = tempListing.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                tempText.text = user.NickName;
            }
        }
    }


    public override void OnPlayerEnteredRoom(Player newUser){
        base.OnPlayerEnteredRoom(newUser);
        Debug.Log("A new user has joined the room");
        //Clear old Listings and List New Users
        ClearUserListings();
        ListUsers();
        photonUsers = PhotonNetwork.PlayerList;
        usersInRoom++;
        if(MultiUserSettings.multiUserSettings.delayStart){
            Debug.Log("Displaying users in room out of max users possible (" + usersInRoom + ":" + MultiUserSettings.multiUserSettings.maxUsers + ")");

            if(usersInRoom > 1)
            {
                readyToCount = true;
            }
            if(usersInRoom == MultiUserSettings.multiUserSettings.maxUsers){
                readyToStart = true;
                //If not the Master Client, return
                if (!PhotonNetwork.IsMasterClient){
                    return;
                }
                //Closes the room so that no new users can join the room until it is re-opened
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    //Start Simulation Function
    public void StartSim()
    {
        isSimLoaded = true;
        if (!PhotonNetwork.IsMasterClient){
            return;
            }
        if(MultiUserSettings.multiUserSettings.delayStart){
            //Closes the room so that no new users can join the room until it is re-opened
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiUserSettings.multiUserSettings.multiUserScene);
    }

    void RestartTimer(){
        lessThanMaxUsers = startingTime;
        timeToStart = startingTime;
        atMaxUsers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode){
        currentScene = scene.buildIndex;
        if(currentScene == MultiUserSettings.multiUserSettings.multiUserScene){
            isSimLoaded = true;

            if(MultiUserSettings.multiUserSettings.delayStart){
                //Sends an RPC call (message) to the Master Client to create all user game objects
                PV.RPC("RPC_LoadedSimScene", RpcTarget.MasterClient);
            }
            else{
                RPC_CreateUser();
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedSimScene(){
        usersInSim++;
        if(usersInSim == PhotonNetwork.PlayerList.Length){
            PV.RPC("RPC_CreateUser", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreateUser(){
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkUser"), transform.position, Quaternion.identity, 0);
    }

    public override void OnPlayerLeftRoom(Player otherUser){
        base.OnPlayerLeftRoom(otherUser);
        Debug.Log(otherUser.NickName + " has left the sim");
        usersInRoom--;
        ClearUserListings();
        ListUsers();
    }
}
