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
    private static Animator anim;
    public bool isWalking = false;
    public bool isRunning = false;
    public float sprintRate = 20;
    public float moveRate;
    public float jumpPower = 10;

    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        myCC = GetComponent<CharacterController>(); 
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update() {
        if(PV.IsMine){
            BasicMovement();
            BasicRotation();
        }
    }

    void BasicMovement() {
        //If W is pressed, move character forward and execute walking animation
        if(Input.GetKey(KeyCode.W)){
            myCC.Move(transform.forward * Time.deltaTime * movementSpeed);
            isWalking = true;
            anim.SetTrigger("isWalking");
        }
        //If W is not pressed, return to idle animation
        else {
            isWalking = false;
            anim.SetTrigger("isNeutral");
        }

        if(Input.GetKey(KeyCode.A)){
            myCC.Move(-transform.right * Time.deltaTime * movementSpeed);
        }

        if(Input.GetKey(KeyCode.S)){
            myCC.Move(-transform.forward * Time.deltaTime * movementSpeed);
        }

        if(Input.GetKey(KeyCode.D)){
            myCC.Move(transform.right * Time.deltaTime * movementSpeed);
        }

        // if (Input.GetKey(KeyCode.Space)){
        //     anim.SetTrigger("isJumping"); //only in jumping state when
        //     Jump();
        // }
        
    }

    void BasicRotation(){
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate (new Vector3(0, mouseX, 0));
    }

}
