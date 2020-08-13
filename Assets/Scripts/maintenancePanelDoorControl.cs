using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maintenancePanelDoorControl : MonoBehaviour
{
    public GameObject Door;
    public bool DoorOpen;
    public bool DoorClose;
    public Camera MainCamera;

    // Update is called once per frame
    void Update()
    {
        OnMouseOver();
        if (DoorOpen == true)
        {
            Door.transform.Translate(Vector3.up * Time.deltaTime * 1);
        }
        if (Door.transform.position.y > 2f)
        {
            DoorOpen = false;
        }
        if (DoorClose == true)
        {
            Door.transform.Translate(Vector3.down * Time.deltaTime * 1);
        }
        if (Door.transform.position.y < 1.1f)
        {
            DoorClose = false;
        }
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Sends an invisible laser pointer from camera to clicked object
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //If the hit object tag is "OpenButton", then change panelIsOpening to true
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "OpenButton")
                {
                    //hit.collider.gameObject now refers to the 
                    //cube under the mouse cursor if present
                    Debug.Log("Door Open Button Clicked");
                    if (Door.transform.position.y < 2)
                    {
                        DoorOpen = true;
                    }
                    if (Door.transform.position.y >= 2)
                    {
                        DoorClose = true;
                    }
                }
            }


        }
    }
}
