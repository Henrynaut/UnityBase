using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut_Controller_Rigidbody : MonoBehaviour {

    private string moveInput = "Vertical";
    private string turnInput = "Horizontal";
    private Rigidbody rb;
    private static Animator anim;

    public float rotationRate = 100; //speed of rotation deg/sec
    public float walkRate = 15; //meters/sec (ish) 
    public bool isWalking = false;
    public bool isRunning = false;
    public float sprintRate = 20;
    public float moveRate;
    public float jumpPower = 10;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveRate = sprintRate;
            anim.SetTrigger("isRunning");
            print("running");
            isRunning = true;
        }
        else
        {
            moveRate = walkRate;
            isRunning = false;
        }
        float moveAxis = Input.GetAxis(moveInput);
        float turnAxis = Input.GetAxis(turnInput);
        InputControl(moveAxis, turnAxis);
        if(moveAxis == 0)
        {
            isWalking = false;
            anim.SetTrigger("isNeutral");
        }

        //animation conditionals: 
        if (isWalking && !isRunning) // avoiding state switching b/t running + walking
        {
            if (Input.GetAxis(moveInput) < 0)
            {
                anim.SetTrigger("Backwards");
            }
            else
            {
            anim.SetTrigger("isWalking");
            }

        }
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("isJumping"); //only in jumping state when
            Jump();
        }
        


	}


    private void Jump()
    {
        rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
    }

    private void InputControl(float moveInput, float turnInput)
    {
        Walk(moveInput);
        Turn(turnInput);
    }

    private void Turn(float input)
    {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0); // x and z rotation = 0
    }

    private void Walk(float input)
    {
        isWalking = true;
        rb.AddForce(transform.forward * input * moveRate, ForceMode.Force);
        //transform.Translate(Vector3.forward * input * moveRate*Time.deltaTime); //forward facing vector * input   backwards or forward *time (makes per second)
    }
}
