using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maintenancePanel : MonoBehaviour
{
    // object to move
    public GameObject panel;
    // start and end positions
    private Vector3 startPos;
    private Vector3 endPos;
    // distance object will move
    private float distance = 0.5f;
    // time from start to end
    private float lerpTime = 3;
    private float currentLerpTime = 0;
    // hit a key to start movement
    private bool keyHit = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = panel.transform.position;
        endPos = panel.transform.position + Vector3.up * distance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.X))
        {
            keyHit = true;
        }
        if(keyHit == true)
        {
            currentLerpTime += Time.deltaTime;
            if(currentLerpTime >= lerpTime)
            {
                currentLerpTime = lerpTime;
            }
            float Perc = currentLerpTime / lerpTime;
            panel.transform.position = Vector3.Lerp(startPos, endPos, Perc);
        }
    }
}
