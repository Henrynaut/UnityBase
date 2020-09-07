//This simple script serves to animate the camera with collaboration with MaxCamera script.
//For the purpose of recording demo.
//enable to start animation in play mode

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAnim : MonoBehaviour {
    public MaxCamera camscript;
    public float t = 1;
    public float mult = 2;
    private float d = 0;
	// Use this for initialization

    void OnEnable()
    {
        camscript = (MaxCamera)FindObjectOfType(typeof(MaxCamera));
        if (camscript) StartCoroutine(_Start());
    }


	IEnumerator _Start () {
        yield return new WaitForSeconds(t);
        d = camscript.desiredDistance;
		StartCoroutine(zoomIn());
	}
	
    IEnumerator zoomIn()
    {
        camscript.desiredDistance = d*mult;
        yield return new WaitForSeconds(t);
        StartCoroutine(zoomOut());    
    }

    IEnumerator zoomOut()
    {
        camscript.desiredDistance = d;
        yield return new WaitForSeconds(t);
        //StartCoroutine(zoomIn()); //uncomment for infinite cycles
    }

}
