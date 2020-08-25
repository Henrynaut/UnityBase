using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material pressedMaterial;


    private Transform _selection;
    private AvatarSetup avatarSetup;
    private bool hitSelectable;
    public Transform rayOrigin;
    public Camera avatarCamera;
    public string objectName;
    public string buttonName1;
    public string buttonName2;
    public string buttonName3;
    public string buttonName4;
    public string buttonName5;
    public string buttonName6;
    public Renderer selectedRenderer;


    public GameObject instructions_button;

    // Start is called before the first frame update
    void Start() {
        avatarSetup = GetComponent<AvatarSetup>();
        //Get Avatar Camera
        avatarCamera = avatarSetup.myCamera;

        instructions_button = GameObject.Find("InstructionText");
        instructions_button.SetActive(false);
        hitSelectable = false;
        objectName = "null";

    }


    private void Update()
    {
        if (_selection != null)
        {
            selectedRenderer = _selection.GetComponent<Renderer>();
            selectedRenderer.material = defaultMaterial;
            _selection = null;
        }

        rayOrigin = avatarCamera.transform;
        RaycastHit hit;
        if(Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000))
        {
            var selection = hit.transform;
            Debug.Log("Hit");
            Debug.Log(rayOrigin);
            Debug.Log(hit.collider.gameObject.name);
            objectName = hit.collider.gameObject.name;

            //If the tag matches, highlight the button
            if (selection.CompareTag(selectableTag))
            {
                selectedRenderer = selection.GetComponent<Renderer>();
                if (selectedRenderer != null )
                {
                    selectedRenderer.material = highlightMaterial;
                }

                _selection = selection;

                instructions_button.SetActive(true);
                hitSelectable = true;
            }
            else
            {
                instructions_button.SetActive(false);
                hitSelectable =false;
            }
        }

         //If the F key is pressed and hit object is selectable, call buttonPressed function
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Calling buttonPressed function.");
            buttonPressed(objectName, selectedRenderer);
        }
    }

    void buttonPressed(string stringName, Renderer selectedRend)
    {
        // If the raycast points at Button1, toggle on the canvas text instructions
        if(stringName == buttonName1)
        {
            Debug.Log(stringName + " clicked!");
            //Turn on light
            selectedRend.material = pressedMaterial;

        }
        else if (stringName == buttonName2)
        {
            instructions_button.SetActive(false);
            Debug.Log(stringName + " clicked!");
            //Raise Jack
        }
        else if (stringName == buttonName3)
        {
            instructions_button.SetActive(false);
            Debug.Log(stringName + " clicked!");
            //Open airlock
        }
        else if (stringName == buttonName4)
        {
            instructions_button.SetActive(false);
            Debug.Log(stringName + " clicked!");
            //Open maintenance panel
        }
        else if (stringName == buttonName5)
        {
            instructions_button.SetActive(false);
            Debug.Log(stringName + " clicked!");
            //Turn off lights
        }
        else if (stringName == buttonName6)
        {
            instructions_button.SetActive(false);
            Debug.Log(stringName + " clicked!");
            //Close Airlock
        }
        return;
    }
}