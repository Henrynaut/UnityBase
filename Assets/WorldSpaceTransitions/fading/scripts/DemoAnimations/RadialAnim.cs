//This simple script serves to animate the camera with collaboration with MaxCamera script.
//For the purpose of recording demo.
//enable to start animation in play mode

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldSpaceTransitions;

public class RadialAnim : MonoBehaviour {
    
    public float delay = 1;
    public float time = 2;
    public float r2 = 10;
    public float r1 = 1;

    public Material[] materials;

    private GizmoFollow gizmo;

    private float r = 0.0f;

    private Vector3 secpt;
    private Vector3 spl1;
    private Vector3 spl2;

    private float startR;
    // Use this for initialization

    //private Vector3 startPosition;

    void OnEnable()
    {
        gizmo = (GizmoFollow)FindObjectOfType(typeof(GizmoFollow));
        if (gizmo) gizmo.enabled = false;

        Shader.DisableKeyword("FADE_PLANE");
        Shader.DisableKeyword("CLIP_PLANE");
        Shader.EnableKeyword("FADE_SPHERE");
        Shader.EnableKeyword("CLIP_SPHERE");

        startR = Shader.GetGlobalFloat("_Radius");
        secpt = Shader.GetGlobalVector("_SectionPoint");
        spl1 = Shader.GetGlobalVector("_SectionPlane");
        spl2 = Shader.GetGlobalVector("_SectionPlane2");

        Shader.SetGlobalVector("_SectionPoint", transform.position);
        Shader.SetGlobalVector("_SectionPlane", transform.forward);
        Shader.SetGlobalVector("_SectionPlane2", transform.right);


        StartCoroutine(_Start());
    }

    void OnDisable()
    {
        if (gizmo) gizmo.enabled = true;

        secpt = Shader.GetGlobalVector("_SectionPoint");
        spl1 = Shader.GetGlobalVector("_SectionPlane");
        spl2 = Shader.GetGlobalVector("_SectionPlane2");

        Shader.SetGlobalVector("_SectionPoint", secpt);
        Shader.SetGlobalVector("_SectionPlane", spl1);
        Shader.SetGlobalVector("_SectionPlane2", spl2);

        Shader.SetGlobalFloat("_Radius", startR);
        
    }


    IEnumerator _Start () {
        
        Shader.SetGlobalFloat("_Radius", r2);
        foreach (Material m in materials) m.SetFloat("_inverse", 0);
        yield return new WaitForSeconds(delay);
        StartCoroutine(play(r1, r2));
        yield return new WaitForSeconds(delay);
        StartCoroutine(play(r2, r1));
        //yield return new WaitForSeconds(delay);
        foreach (Material m in materials) m.SetFloat("_inverse", 1);
        //yield return new WaitForSeconds(delay);
        StartCoroutine(play(r1, r2));
        yield return new WaitForSeconds(delay);
        StartCoroutine(play(r2, r1));
        yield return new WaitForSeconds(delay);
        foreach (Material m in materials) m.SetFloat("_inverse", 0);
        StartCoroutine(play(r2, r1));
        yield return new WaitForSeconds(delay);
        StartCoroutine(_Start()); //infinite cycles; comment to stop after one cycle

    }
	
	// Update is called once per frame
    IEnumerator play(float rx, float ry)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            r = Mathf.Lerp(ry, rx, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            Shader.SetGlobalFloat("_Radius", r);
            yield return new WaitForEndOfFrame();
        }
    }

}
