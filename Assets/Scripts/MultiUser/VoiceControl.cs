using System.Collections;
using Photon.Voice.Unity;
using System.Collections.Generic;
using UnityEngine;

public class VoiceControl : MonoBehaviour
{

    public Recorder VoiceRecorder;

    // Print state of voice recorder
    void Start()
    {
        Debug.Log("Recorder state = " + VoiceRecorder.TransmitEnabled);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            VoiceRecorder.TransmitEnabled = !VoiceRecorder.TransmitEnabled;
            if (VoiceRecorder.TransmitEnabled)
            {
                Debug.Log("Microphone Unmuted.");
            }
            else
            {
                Debug.Log("Microphone Muted.");
            }
        }
    }

}