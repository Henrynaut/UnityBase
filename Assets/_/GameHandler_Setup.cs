using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;

public class GameHandler_Setup : MonoBehaviour {

    [SerializeField] private CameraFollow cameraFollow;
    private Vector3 cameraPosition;
    private float orthoSize = 60f;

    private void Start() {
        cameraPosition = new Vector3(150, 100);
        cameraFollow.Setup(() => cameraPosition, () => orthoSize, true, true);
    }

    private void Update() {
        float cameraSpeed = 100f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            cameraPosition += new Vector3(-1, 0) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            cameraPosition += new Vector3(+1, 0) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            cameraPosition += new Vector3(0, +1) * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            cameraPosition += new Vector3(0, -1) * cameraSpeed * Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            orthoSize -= 10f;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            orthoSize += 10f;
        }
    }

}
