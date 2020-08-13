using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class userSetup : MonoBehaviour
{

    public int characterValue;
    public string usernameString;
    public GameObject myCharacter;
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

    }

}
