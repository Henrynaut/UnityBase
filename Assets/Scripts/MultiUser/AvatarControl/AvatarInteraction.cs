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
    // public TextMeshProUGUI energyText;

    // public TextMeshProUGUI energyDisplay;


    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        avatarSetup = GetComponent<AvatarSetup>();
    }

    // Update is called once per frame
    void Update() {
        if(!PV.IsMine){
            return;
        }
        if(Input.GetMouseButton(0))        {
            PV.RPC("RPC_Laser", RpcTarget.All);
            // energyText.text = "25 Wh";
        }
    }

    [PunRPC]
    void RPC_Laser() {
        RaycastHit hit;
        if(Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000)){
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            Debug.Log("Did Hit");
            // if(hit.transform.tag == "Avatar"){
                // hit.transform.gameObject.GetComponent<AvatarSetup>().userOxygen -= avatarSetup.userEnergy;
            // Spawn Banana at Raycast-Collider Intersection point with random rotation
            // PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "Banana"), hit.point, Random.rotation, 0);
            PhotonNetwork.InstantiateSceneObject(Path.Combine("PhotonPrefabs", "Banana"), hit.point, Quaternion.identity, 0);
            // }
        }
        else{
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did Not Hit");
        }
    }

}
