using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jackMovement : MonoBehaviour
{
    public GameObject Jack;
    public bool JackUp;
    public bool JackDown;
    public Camera MainCamera;

    // Update is called once per frame
    void Update()
    {
        OnMouseOver();
        if (JackUp == true)
        {
            Jack.transform.Translate(Vector3.up * Time.deltaTime * 1);
        }
        if (Jack.transform.position.y > 1f)
        {
            JackUp = false;
        }
        if (JackDown == true)
        {
            Jack.transform.Translate(Vector3.down * Time.deltaTime * 1);
        }
        if (Jack.transform.position.y < 0.625f)
        {
            JackDown = false;
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
                if (hit.collider.tag == "JackButton")
                {
                    //hit.collider.gameObject now refers to the 
                    //cube under the mouse cursor if present
                    Debug.Log("Door Open Button Clicked");
                    if (Jack.transform.position.y < 1)
                    {
                        JackUp = true;
                    }
                    if (Jack.transform.position.y >= 1)
                    {
                        JackDown = true;
                    }
                }
            }


        }
    }
}
