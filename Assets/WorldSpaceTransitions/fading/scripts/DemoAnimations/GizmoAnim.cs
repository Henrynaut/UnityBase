//This simple script serves to animate the gizmo.
//For the purpose of recording demo.
//enable to start animation in play mode
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoAnim : MonoBehaviour {
    public Transform gizmo;
    public float delay = 1;
    public float startAdvance = 1;
    public float time = 2;
    public float moveSpan = 1;
    // Use this for initialization

    //private Vector3 startPosition;

    void OnEnable()
    {

        if (gizmo) StartCoroutine(_Start());
    }


	IEnumerator _Start () {
        
        yield return new WaitForSeconds(delay);

		StartCoroutine(move());
	}
	
	// Update is called once per frame
    IEnumerator move()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = gizmo.position;

        Vector3 startAnimPosition = startPosition + startAdvance * gizmo.forward;
        gizmo.position = startAnimPosition;
        yield return new WaitForSeconds(delay);
        Vector3 endPosition = gizmo.position - gizmo.forward * moveSpan;
        while (elapsedTime < time)
        {
            gizmo.position = Vector3.Lerp(startAnimPosition, endPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(delay);

        gizmo.position = startPosition;

    }

}
