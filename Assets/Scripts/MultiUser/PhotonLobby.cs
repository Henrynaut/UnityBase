using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhotonLobby : MonoBehaviourPunCallbacks {

	//Singleton Variable (only one, can be referenced by other scripts, useful!)
	public static PhotonLobby lobby;

	public GameObject connectButton;
	public GameObject cancelButton;
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
		Debug.Log("User has connected to the SUITS master server.");
		StatusLabel.text = ("This device has connected to the SUITS master server.");

		//When the master client loads a scene, all the other users connected
		//    to the master client will also load that scene.
		PhotonNetwork.AutomaticallySyncScene = true;
		connectButton.SetActive(true); //User is now connected to servers, enables connectButton for joining a simulation.
	}

	public void OnConnectButtonClicked(){
		Debug.Log("The connect button was clicked.");
		StatusLabel.text = ("The connect button was clicked.");

		connectButton.SetActive(false);
		cancelButton.SetActive(true);
		//Randomly picks a room for user to join
		PhotonNetwork.JoinRandomRoom(null, 0); //0 means any amount of max players
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("Tried to join a random simulation but failed, there aren't any open simulations available.");
		StatusLabel.text = ("Tried to join a random simulation but failed, there aren't any open simulations available.");

		CreateRoom();
	}

	void CreateRoom(){
		Debug.Log("Trying to create a new room.");
		StatusLabel.text = ("Trying to create a new room.");

		//Set the room name to be 12 until we have more than 10 users.
		// int randomRoomName = 12;
		int randomRoomName = Random.Range(0, 10000);
		roomLabel.text = ("Room #: " + randomRoomName.ToString());
		RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiUserSettings.multiUserSettings.maxUsers };
		PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
	}

	public override void OnCreateRoomFailed(short returnCode, string message){
		Debug.Log("Tried to create a new room but failed, there is already a room with the same name");
		StatusLabel.text = ("Tried to create a new room but failed, there is already a room with the same name");

		CreateRoom();
	}

	public void OnCancelButtonClicked(){
		Debug.Log("The cancel button was clicked.");
		StatusLabel.text = ("The cancel button was clicked.");

		cancelButton.SetActive(false);
		connectButton.SetActive(true);
		PhotonNetwork.LeaveRoom();
	}
}
