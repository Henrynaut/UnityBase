using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
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

    public int userInSim;

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
                //Closesthe room so that no new players can join the room until it is re-opened
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }
	}
}
