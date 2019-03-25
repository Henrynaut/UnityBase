using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhotonLobbyCustomMatch : MonoBehaviourPunCallbacks, ILobbyCallbacks {

	//Singleton Variable (only one, can be referenced by other scripts, useful!)
	public static PhotonLobbyCustomMatch lobby;

	public string roomName;
	public int roomSize;
	public string usernameString;
	public GameObject roomListingPrefab;
	public Transform roomsPanel;
	public TextMeshProUGUI roomLabel;
	public TextMeshProUGUI StatusLabel;
	public TextMeshProUGUI regionLabel;


	private void Awake()
	{
		lobby = this; //Creates the singleton, lives within the main menu scene.
	}

	// Use this for initialization
	void Start () {
		// PhotonNetwork.ConnectToRegionMaster("us"); //Connect to US East Region Master Server.
		PhotonNetwork.ConnectUsingSettings(); //Connects to Master photon server.
		Debug.Log("Connecting...");
		StatusLabel.text = ("Connecting...");
		//Ensures the game object isn't destroyed when opening a new scene
        DontDestroyOnLoad(this.gameObject);
	}
	
	//Callback function
	public override void OnConnectedToMaster(){
		Debug.Log("User has connected to the Photon master server.");
		StatusLabel.text = ("This device has connected to the Photon master server.");

        //Set my nickname based on number
        PhotonNetwork.NickName = "User " + Random.Range(0, 1000);

		//When the master client loads a scene, all the other users connected
		//    to the master client will also load that scene.
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList){
		base.OnRoomListUpdate(roomList);
		RemoveRoomListings();
		foreach(RoomInfo room in roomList){
			ListRoom(room);
		}
	}

	void RemoveRoomListings(){
		while(roomsPanel.childCount !=0){
			Destroy(roomsPanel.GetChild(0).gameObject);
		}
	}

	void ListRoom(RoomInfo room){
		if(room.IsOpen && room.IsVisible){
			GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
			RoomButton tempButton = tempListing.GetComponent<RoomButton>();
			tempButton.roomName = room.Name;
			tempButton.roomSize = room.MaxPlayers;
			tempButton.SetRoom();
		}
	}

	public void CreateRoom(){
		Debug.Log("Trying to create a new room.");
		StatusLabel.text = ("Trying to create a new room.");

		roomLabel.text = ("Room ID: " + roomName);
		RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
		PhotonNetwork.CreateRoom(roomName, roomOps);
	}

	public override void OnCreateRoomFailed(short returnCode, string message){
		Debug.Log("Tried to create a new room but failed, there is already a room with the same name");
		StatusLabel.text = ("Tried to create a new room but failed, there is already a room with the same name");

		// CreateRoom();
	}

	public void OnRoomNameChanged(string nameIn){
		roomName = nameIn;
	}

	public void OnRoomSizeChanged(string sizeIn){
		roomSize = int.Parse(sizeIn);
	}

    public void OnUsernameChanged(string usernameIn){
		usernameString = usernameIn;
		//Set user NickName based on usernameString
        PhotonNetwork.NickName = usernameString;
	}
	public void JoinLobbyonClick(){
		if(!PhotonNetwork.InLobby){
			PhotonNetwork.JoinLobby();
		}
	}
}
