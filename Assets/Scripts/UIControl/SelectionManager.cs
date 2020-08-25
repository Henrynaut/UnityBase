using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    private Transform _selection;
    private AvatarSetup avatarSetup;
    public Transform rayOrigin;
    public Camera avatarCamera;
    public string objectName;
    public string buttonName1;
    public string buttonName2;
    public string buttonName3;
    public string buttonName4;
    public string buttonName5;
    public string buttonName6;


    public GameObject instructions_button;

    // Start is called before the first frame update
    void Start() {
        avatarSetup = GetComponent<AvatarSetup>();
        //Get Avatar Camera
        avatarCamera = avatarSetup.myCamera;

        instructions_button = GameObject.Find("InstructionText");
        instructions_button.SetActive(false);

    }


    private void Update()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }
        
        rayOrigin = avatarCamera.transform;
        // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // if (Physics.Raycast(ray, out hit, 2))
        if(Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000))
        {
            // Debug.DrawLine(ray.origin, hit.point);
            var selection = hit.transform;
            Debug.Log("Hit");
            Debug.Log(rayOrigin);
            Debug.Log(hit.collider.gameObject.name);
            objectName = hit.collider.gameObject.name;

            //If the tag matches, highlight the button
            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterial;
                }

                _selection = selection;

                // If the raycast points at Button1, toggle on the canvas text instructions
                if(objectName == buttonName1)
                {
                    instructions_button.SetActive(true);
                    //Turn on light
                }
                else if (objectName == buttonName2)
                {
                    instructions_button.SetActive(true);
                    //Raise Jack
                }
                else if (objectName == buttonName3)
                {
                    instructions_button.SetActive(true);
                    //Open airlock
                }
                else if (objectName == buttonName4)
                {
                    instructions_button.SetActive(true);
                    //Open maintenance panel
                }
                else if (objectName == buttonName5)
                {
                    instructions_button.SetActive(true);
                    //Turn off lights
                }
                else if (objectName == buttonName6)
                {
                    instructions_button.SetActive(true);
                    //Close Airlock
                }
                else
                {
                    instructions_button.SetActive(false);
                }

            }
        }

    }

}