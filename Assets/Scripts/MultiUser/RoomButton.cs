using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class RoomButton : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sizeText;

    public string roomName;
    public int roomSize;

    public void SetRoom(){
        nameText.text = roomName;
        sizeText.text = roomSize.ToString();
    }

    public void JoinRoomOnClick(){
        PhotonNetwork.JoinRoom(roomName);
    }
}