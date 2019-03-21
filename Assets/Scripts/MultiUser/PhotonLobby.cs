using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks {

	//Singleton Variable (only one, can be referenced by other scripts, useful!)
	public static PhotonLobby lobby;

	public GameObject connectButton;
	public GameObject cancelButton;

	private void Awake()
	{
		lobby = this; //Creates the singleton, lives within the main menu scene.
	}

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings(); //Connects to Master photon server.
		Debug.Log("Connecting...");
	}
	
	//Callback function
	public override void OnConnectedToMaster(){ //I removed override, didn't need it
		Debug.Log("Player has connected to the Photon master server.");
		connectButton.SetActive(true); //Player is now connected to servers, enables connectButton for joining a simulation.
	}

	public void OnConnectButtonClicked(){
		Debug.Log("The connect button was clicked.");
		connectButton.SetActive(false);
		cancelButton.SetActive(true);
		//Randomly picks a room for user to join
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("Tried to join a random simulation but failed, there aren't any open simulations available.");
		CreateRoom();
	}

	void CreateRoom(){
		Debug.Log("Trying to create a new room.");
		int randomRoomName = Random.Range(0, 10000);
		RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
		PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
	}

	public override void OnJoinedRoom(){
		Debug.Log("You are now in a room.");
	}

	public override void OnCreateRoomFailed(short returnCode, string message){
		Debug.Log("Tried to create a new room but failed, there is already a room with the same name");
		CreateRoom();
	}

	public void OnCancelButtonClicked(){
		Debug.Log("The cancel button was clicked.");
		cancelButton.SetActive(false);
		connectButton.SetActive(true);
		PhotonNetwork.LeaveRoom();
	}
}
