using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClose : MonoBehaviour
{
    public GameObject Panel;
    public bool panelIsClosing;
    public Camera MainCamera;

    // Update is called once per frame
    void Update()
    {
        OnMouseOver();

        if (panelIsClosing == true)
        {
            Panel.transform.Translate(Vector3.down * Time.deltaTime * 1);
        }
        if (Panel.transform.position.y < 1.1f)
        {
            panelIsClosing = false;
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
                if (hit.collider.tag == "CloseButton")
                {
                    //hit.collider.gameObject now refers to the 
                    //cube under the mouse cursor if present
                    Debug.Log("Door Close Button Clicked");
                    panelIsClosing = true;
                }
            }


        }
    }
}
