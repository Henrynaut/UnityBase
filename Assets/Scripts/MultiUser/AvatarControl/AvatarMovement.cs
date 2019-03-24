using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovement : MonoBehaviour
{
    
    //Photon Movement Replication
    private PhotonView PV;
    private CharacterController myCC;
    public float movementSpeed;
    public float rotationSpeed;

    //Animation Variables
    private AvatarSetup avatarSetup;
    // public bool isWalking = false;
    // public bool isRunning = false;
    // public float sprintRate = 20;
    // public float moveRate;

    public float jumpSpeed = 8;
    //Lunar Gravity
    public float gravity = 1.62f;

    private Vector3 velocity;
    private float vSpeed = 0; // current vertical velocity


    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        myCC = GetComponent<CharacterController>(); 
        avatarSetup = GetComponent<AvatarSetup>();
    }

    // Update is called once per frame
    void Update() {
        if(PV.IsMine){
            BasicMovement();
            BasicJumping();
            BasicRotation();
        }
    }

    void BasicMovement() {
        //If W is pressed, move character forward and execute walking animation
        if(Input.GetKey(KeyCode.W)){
            avatarSetup.animator.SetTrigger("isWalking");
            myCC.Move(transform.forward * Time.deltaTime * movementSpeed);
        }

        //If W is not pressed, return to idle animation
        else {
            avatarSetup.animator.SetTrigger("isNeutral");
        }

        if(Input.GetKey(KeyCode.A)){
            myCC.Move(-transform.right * Time.deltaTime * movementSpeed);
        }

        if(Input.GetKey(KeyCode.S)){
            myCC.Move(-transform.forward * Time.deltaTime * movementSpeed);
            //Set Backwards Animation Trigger
            avatarSetup.animator.SetTrigger("Backwards");
        }

        if(Input.GetKey(KeyCode.D)){
            myCC.Move(transform.right * Time.deltaTime * movementSpeed);
        }
    }

    void BasicJumping(){
        velocity = transform.forward * Input.GetAxis("Vertical") * movementSpeed;
        if (myCC.isGrounded) {
            //If grounded, the vertical speed = 0
            vSpeed = 0;
            //If Jump commanded, vertical speed = jumpSpeed
            if (Input.GetKeyDown("space")) {
            vSpeed = jumpSpeed;

            //Set Jumping Animation Trigger
            avatarSetup.animator.SetTrigger("isJumping");
            }
        }

        //Apply force of gravity to the vertical descent speed
        vSpeed -= gravity * Time.deltaTime;
        //Include vertical speed in velocity
        velocity.y = vSpeed;
        // convert vel to displacement and Move the character accordingly:
        myCC.Move(velocity * Time.deltaTime);
    }

    void BasicRotation(){
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate (new Vector3(0, mouseX, 0));
    }


}
