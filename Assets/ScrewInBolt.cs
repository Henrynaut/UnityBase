using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewInBolt : MonoBehaviour
{
    public float speed = 0.1f;
    bool bolt_in = false;
    bool bolt_out = false;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            bolt_in = true;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            bolt_out = true;
        }

        if (bolt_in == true)
        {
               transform.Translate(0f, -1f * Time.deltaTime, 0f);
        }
        if (transform.position.z < 2.92f)
        {
                bolt_in = false;
        }
        if (bolt_out == true)
        {
            transform.Translate(0f, 1f * Time.deltaTime, 0f);
        }
        if (transform.position.z > 3.2f)
        {
            bolt_out = false;
        }

    }
}
