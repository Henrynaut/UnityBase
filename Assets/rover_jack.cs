using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rover_jack : MonoBehaviour
{
    bool jack_up = false;
    bool jack_down = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            jack_down = true;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            jack_up = true;
        }
        if (jack_up == true)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 1);
        }
        if (transform.position.y > 0.3f)
        {
            jack_up = false;
        }
        if (jack_down== true)
        {
            transform.Translate(-Vector3.up * Time.deltaTime * 1);
        }
        if (transform.position.y < -0.21f)
        {
            jack_down = false;
        }

    }
}
