using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public GameObject Panel;
    public bool panelIsOpening;
    public Camera MainCamera;

    // Update is called once per frame
    void Update()
    {
        OnMouseOver();

        if (panelIsOpening==true)
        {
            Panel.transform.Translate(Vector3.up * Time.deltaTime * 1);
        }
        if (Panel.transform.position.y > 2f)
        {
            panelIsOpening = false;
        }
    }

    //Mouse Click Function
     void OnMouseOver()
     {
        if(Input.GetMouseButtonDown(0))
        {
            //Sends an invisible laser pointer from camera to clicked object
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
              //If the hit object tag is "OpenButton", then change panelIsOpening to true
              if (Physics.Raycast(ray, out hit)) {
                if(hit.collider.tag == "OpenButton"){
                        //hit.collider.gameObject now refers to the 
                        //cube under the mouse cursor if present
                    Debug.Log("Door Open Button Clicked");
                    panelIsOpening = true;
                }
            }


        }
    }      


        
    // void OnMouseDown() // should detect when clicking on collider with mouse
    // {
    //     panelIsOpening = true;
    // }
}
