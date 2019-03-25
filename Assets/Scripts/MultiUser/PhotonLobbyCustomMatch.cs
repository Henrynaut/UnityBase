using Photon.Pun;
using Photon.Realtime;
// using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

 	//Import DLL's for window name change (Windows only)
	[DllImport("user32.dll", EntryPoint = "SetWindowText")]
	public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
	[DllImport("user32.dll", EntryPoint = "FindWindow")]
	public static extern System.IntPtr FindWindow(System.String className, System.String windowName);

	public List<RoomInfo> roomListings;


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
		roomListings = new List<RoomInfo>();
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
		// RemoveRoomListings();
		int tempIndex;
		foreach(RoomInfo room in roomList){
			if(roomListings != null){
			//Check to see if current room already exists within the list
				//Return index through predicate function
				tempIndex = roomListings.FindIndex(ByName(room.Name));
			}
			//If not found, return -1
			else{
				tempIndex = -1;
			}
			//If a match is found, remove
			if(tempIndex != -1){
				roomListings.RemoveAt(tempIndex);
				Destroy(roomsPanel.GetChild(tempIndex).gameObject);
			}
			//If match is not found, add current room to room listings
			else{
				roomListings.Add(room);
				ListRoom(room);
			}
		}
	}

	//Predicate function that accesses a more complex class in a simple way
	static System.Predicate<RoomInfo> ByName(string name){
		return delegate(RoomInfo room){
			return room.Name == name;
		};
	}

	void RemoveRoomListings(){
		int i = 0;
		while(roomsPanel.childCount !=0){
			//Iterates through room listings and removes
			Destroy(roomsPanel.GetChild(i).gameObject);
			i++;
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
		ChangeTitle(usernameString);
	}
	public void JoinLobbyonClick(){
		if(!PhotonNetwork.InLobby){
			PhotonNetwork.JoinLobby();
		}
	}

	//---Windows only section Start---

	public void ChangeTitle(string newTitle)
	{
		//Make sure Multiuser Moon Base is the current name
		var windowPtr = FindWindow(null, "Multiuser Moon Base");
		//Set the title text using the window handle.
		SetWindowText(windowPtr, "Moon Base " + newTitle);
	}

	//---Windows only section end---
}
