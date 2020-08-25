using System.Collections;
using Photon.Voice.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 
 public class buttonObject : MonoBehaviour {
 
    public GameObject definedButton;
    public GameObject otherButton;

    public Recorder VoiceRecorder;
    public GraphicRaycaster canvasRaycaster;
    public List<RaycastResult> list;
    public Vector2 screenPoint;
 
     // Use this for initialization
     void Start () {    
         definedButton = this.gameObject;
     }
     
     // Update is called once per frame
    void Update () {

        //If left mouse is clicked, print name of UI element 
        if (Input.GetMouseButtonDown(0))
        {
            // Graphic Raycast setup
            list = new List<RaycastResult>();
            screenPoint = Input.mousePosition;
            Debug.Log(screenPoint);
            PointerEventData ed = new PointerEventData(EventSystem.current);
            ed.position = screenPoint;
            canvasRaycaster.Raycast(ed, list);

            //If something is hit, print the object name
            if (list != null && list.Count > 0)
            {
                Debug.Log("Hit: " + list[0].gameObject.name);

                //If the hit canvas object is the definedButton, toggle to otherbutton
                if (list[0].gameObject.name == definedButton.name)
                {
                    definedButton.SetActive(false);
                    otherButton.SetActive(true);
                    Debug.Log("Toggling " + otherButton.name + " on.");

                    //If the otherButton toggled on is the muteButton
                    //set the voice recorder transmit state to false
                    if (otherButton.name == "MutedIcon")
                    {
                        VoiceRecorder.TransmitEnabled = false;
                        Debug.Log("Voice Disabled.");
                    }
                    //If the otherButton toggled on is the unmuteButton
                    //set the voice recorder transmit state to false
                    else if (otherButton.name == "UnmutedIcon")
                    {
                        VoiceRecorder.TransmitEnabled = true;
                        Debug.Log("Voice Enabled.");
                    }
                }
            }
            else
            {
                Debug.Log("No Canvas Object Hit.");
            }
        }  

    }

 }

 