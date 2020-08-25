
using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class DragObject : MonoBehaviour

{

    private Vector3 mOffset;
    private AvatarSetup avatarSetup;
    private float mZCoord;

    public Camera avatarCamera;

    void Update() {
        // avatarSetup = GameObject.Find("UserAvatar(Clone)").GetComponent<AvatarSetup>();
        //Get Avatar Camera
        if (avatarCamera == null)
        {
        avatarCamera = GameObject.Find("AvatarCamera").GetComponent<Camera>();
        }
    }


    void OnMouseDown()

    {

        mZCoord = avatarCamera.WorldToScreenPoint(

            gameObject.transform.position).z;



        // Store offset = gameobject world pos - mouse world pos

        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        Debug.Log("Mouse Pressed.");
    }



    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = mZCoord;



        // Convert it to world points

        return avatarCamera.ScreenToWorldPoint(mousePoint);

    }



    void OnMouseDrag()

    {

        transform.position = GetMouseAsWorldPoint() + mOffset;

    }

}