using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;


public class AvatarInteraction : MonoBehaviour
{

    private PhotonView PV;
    private AvatarSetup avatarSetup;
    public Transform rayOrigin;
    public int lastSplineID;
    public GameObject splinePrefab;
    public PhotonMessageInfo Info;
    private GameObject currentSpline;
    private objectID objectID;
    public bool enableDrawing;
    // public TextMeshProUGUI energyText;

    // public TextMeshProUGUI energyDisplay;


    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        avatarSetup = GetComponent<AvatarSetup>();
        lastSplineID = 0;       //Initialize SplineID at 0
        enableDrawing = false;
    }

    // Update is called once per frame
    void Update() {
        if(!PV.IsMine){
            return;
        }

        //If Asterisk pressed, toggle drawing bool
        if(Input.GetKeyDown(KeyCode.Comma))
        {
            enableDrawing = !enableDrawing;
            Debug.Log("Drawing = " + enableDrawing);
        }
        //Spawn Transparent Sphere on Left Click
        if(Input.GetMouseButton(0) && enableDrawing)        {
            updateSplinePen();
            // PV.RPC("RPC_Laser_Sphere", RpcTarget.All);
        }

        //Spawn Arrow on Right Click
        if(Input.GetMouseButton(1))        {
            PV.RPC("RPC_Laser_Arrow", RpcTarget.All);
            // energyText.text = "25 Wh";
        }

        //Spawn Banana on Middle Click
        if(Input.GetMouseButton(2))        {
            PV.RPC("RPC_Laser_Object", RpcTarget.All);
        }
    }

    //RPC call to draw a spline sphere point
    [PunRPC]
    void SplineDraw( Vector3 position, Quaternion rotation, int SplineID, PhotonMessageInfo info )
    {
        double timestamp = PhotonNetwork.Time;
        timestamp = info.SentServerTime;
        //Spawn Spline sphere
        currentSpline = Instantiate(splinePrefab, position, rotation);
        Debug.Log(SplineID);
        //Get objectID component from spline sphere and assign lastSplineID
        objectID = currentSpline.GetComponent(typeof(objectID)) as objectID;
        objectID.ID = SplineID;
    }

    void updateSplinePen(){
        // Instead of having a different PhotonView for each
        //     sphere, such as in the RPC_Laser_Sphere() function
        //     I should have all of the spheres be a component
        //     part of the networked user's PhotonView so
        //     that I don't run out of PhotonView IDs
        lastSplineID++;         //Increment lastSplineID by 1

        if( PhotonNetwork.OfflineMode == true)
        {
            //SplineDraw() Offline Mode
            SplineDraw( (rayOrigin.position + (rayOrigin.TransformDirection(Vector3.forward)*2))
                      , Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))
                      , lastSplineID
                      , Info );
        }
        else
        {
            //SplineDraw() RPC Online Mode
            PV.RPC( "SplineDraw"
                          , RpcTarget.All
                          , new object[] { (rayOrigin.position + (rayOrigin.TransformDirection(Vector3.forward)*2))
                                         , Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))
                                         , lastSplineID
                                         }
                          );
        }
    }

    [PunRPC]
    void RPC_Laser_Arrow() {
        RaycastHit hit;
        if(Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000)){
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            Debug.Log("Did Hit");
            // if(hit.transform.tag == "Avatar"){
                // hit.transform.gameObject.GetComponent<AvatarSetup>().userOxygen -= avatarSetup.userEnergy;
            // Spawn Banana at Raycast-Collider Intersection point with random rotation
            // PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Banana"), hit.point, Random.rotation, 0);
            // PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Banana"), hit.point, Quaternion.identity, 0);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Tele_Arrow"), hit.point, Quaternion.identity, 0);
            // }
        }
        else{
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did Not Hit");
        }
    }

    [PunRPC]
    void RPC_Laser_Object() {
        RaycastHit hit;
        if(Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000)){
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            Debug.Log("Did Hit");
            // if(hit.transform.tag == "Avatar"){
                // hit.transform.gameObject.GetComponent<AvatarSetup>().userOxygen -= avatarSetup.userEnergy;
            // Spawn Banana at Raycast-Collider Intersection point with random rotation
            // PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Banana"), hit.point, Random.rotation, 0);
            // PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Banana"), hit.point, Quaternion.identity, 0);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Banana"), hit.point, Quaternion.identity, 0);
            // }
        }
        else{
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did Not Hit");
        }
    }

    // [PunRPC]
    // void RPC_Laser_Sphere() {
    //     // Spawn chalk sphere in front of user
    //     // PhotonNetwork.Instantiate(
    //         Object.Instantiate(
    //         Path.Combine("PhotonPrefabs", "Tele_Sphere"),
    //         (rayOrigin.position + (rayOrigin.TransformDirection(Vector3.forward)*2)),
    //         //Random Z Rotation
    //         Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), 0);
    //     Debug.Log("Drawing with Chalk");
    // }

}
