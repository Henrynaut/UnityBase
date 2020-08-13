using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight_script : MonoBehaviour
{

    public Light flashLight;

    public Camera sceneCamera;

    private bool lightBool;

    void Start()
    {
        lightBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        flashLight.transform.position = sceneCamera.transform.position;
        if (Input.GetKeyDown(KeyCode.H))
        {
            lightBool = !lightBool;
            flashLight.enabled = lightBool;
        }
    }
}
