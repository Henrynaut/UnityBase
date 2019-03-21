using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManager;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room info (Singleton variable)
    public static PhotonRoom room;
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

    private void Awake(){
        if(PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            //Replace with new instance if non-matching
            if(PhotonRoom.room != this){
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestoryOnLoad(this.gameObject);
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
        //Get list of users in room
        photonUsers = PhotonNetwork.PlayerList;
        usersInRoom = photonUsers.Length;
        myNumberInRoom = usersInRoom;
        //Set my nickname based on number
        PhotonNetwork.NickName = myNumberInRoom.ToString();
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
        else
        {
            StartSim();
        }
	}

    public override void OnUserEnteredRoom(Player newUser){
        base.OnUserEnteredRoom(newUser);
        Debug.Log("A new user has joined the room");
        photonUsers = PhotonNetwork.UserList;
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
    void StartSim()
    {
        isSimLoaded = true;
        if (!PhotonNetwork.IsMasterClient){
            return;
            }
        if(MultiUserSettings.multiUserSettings.delayStart){
            //Closes the room so that no new users can join the room until it is re-opened
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiUserSettings.multiUserSettings.multiUserScene)
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
        if(usersInSim == PhotonNetwork.UserList.Length){
            PV.RPC("RPC_CreateUser", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreateUser(){
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer", transform.position, quaternion.identity, 0));
    }
}
