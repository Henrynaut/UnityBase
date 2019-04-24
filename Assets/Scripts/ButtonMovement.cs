using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ButtonMovement : MonoBehaviour {

   public static ButtonMovement buttonMovement;
   public bool yawFlag;

    // Start is called before the first frame update
    void Start()
    {
      yawFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.DownArrow))
         {
            transform.position = new Vector3 (-5.7f, .7f, 12.9f);
            yawFlag = true;
            
            Debug.Log("Button Pressed");
         }
         else if (transform.position.y < 0.8f)
         {
            transform.position = transform.position + new Vector3 (0.0f, .001f, 0.0f);
         }
    }
}
