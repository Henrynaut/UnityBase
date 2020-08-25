using System.Collections;
using Photon.Voice.Unity;
using System.Collections.Generic;
using UnityEngine;

public class VoiceControl : MonoBehaviour
{

    public Recorder VoiceRecorder;
    private GameObject mutedIcon;
    private GameObject unmutedIcon;

    // Print state of voice recorder
    void Start()
    {
        VoiceRecorder.TransmitEnabled = false;       //Muted state by default
        Debug.Log("Recorder state = " + VoiceRecorder.TransmitEnabled);
        mutedIcon = GameObject.Find("MutedIcon");
        unmutedIcon = GameObject.Find("UnmutedIcon");
        muteIconUpdate(VoiceRecorder.TransmitEnabled);
    }

    void Update()
    {
        //Find GameObjects if they were lost on level change
        if (mutedIcon == null)
        {
            mutedIcon = GameObject.Find("MutedIcon");
            muteIconUpdate(VoiceRecorder.TransmitEnabled);
            Debug.Log("Found MutedIcon");
        }
        if (unmutedIcon == null)
        {
        unmutedIcon = GameObject.Find("UnmutedIcon");
        VoiceRecorder.TransmitEnabled = true;           //Unmute upon level load
        muteIconUpdate(VoiceRecorder.TransmitEnabled);
        Debug.Log("Found UnmutedIcon");
        }

        //If the M key is pressed, toggle between mute and unmute icons, and change transmit state
        if (Input.GetKeyDown(KeyCode.M))
        {
            VoiceRecorder.TransmitEnabled = !VoiceRecorder.TransmitEnabled;
            if (VoiceRecorder.TransmitEnabled)
            {
                Debug.Log("Microphone Unmuted.");
                muteIconUpdate(VoiceRecorder.TransmitEnabled);
            }
            else
            {
                Debug.Log("Microphone Muted.");
                muteIconUpdate(VoiceRecorder.TransmitEnabled);
            }
        }
    }

    void muteIconUpdate(bool transmitState)
    {
        if (transmitState)
        {
            unmutedIcon.SetActive(true);        //toggle unmuted icon On
            mutedIcon.SetActive(false);
        }
        else
        {
            mutedIcon.SetActive(true);          //toggle muted icon on
            unmutedIcon.SetActive(false);
        }
    }

}