using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour {

    private Animator _anim;
    private HandGrabbing _handGrab;
 

	// Use this for initialization
	void Start ()
    {
        _anim = GetComponentInChildren<Animator>();
        _handGrab = GetComponent<HandGrabbing>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		//if we are pressing grab, set animator bool IsGrabbing to true
        if(Input.GetAxis(_handGrab.InputName) >= 0.01f)
        {
            //if hand is closed and has already grabbed, send to closed state, grabbing to False
            if (_anim.GetBool("IsClosed"))
            {
                _anim.SetBool("IsGrabbing", false);
                //STATE 01

            }
            else if (!_anim.GetBool("IsGrabbing"))
            {
                _anim.SetBool("IsGrabbing", true);
                //Set IsClosed Flag to true (yes, closed)
                _anim.SetBool("IsClosed", true);
                //STATE 11

            }
        }
        //if we let go of grab, set Closed to false
        else
        {
            _anim.SetBool("IsClosed", false);
            //STATE 00
        }

	}
}
