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
	public GameObject roomListingPrefab;
	public Transform roomsPanel;
	public TextMeshProUGUI roomLabel;
	public TextMeshProUGUI StatusLabel;

	private void Awake()
	{
		lobby = this; //Creates the singleton, lives within the main menu scene.
	}

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings(); //Connects to Master photon server.
		Debug.Log("Connecting...");
		StatusLabel.text = ("Connecting...");

	}
	
	//Callback function
	public override void OnConnectedToMaster(){ //I removed override, didn't need it
		Debug.Log("User has connected to the Photon master server.");
		StatusLabel.text = ("This device has connected to the Photon master server.");

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

	public void JoinLobbyonClick(){
		if(!PhotonNetwork.InLobby){
			PhotonNetwork.JoinLobby();
		}
	}
}
