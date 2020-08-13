using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interpolation between points with a Catmull-Rom spline
public class Catmull : MonoBehaviour
{
    public List<GameObject> controlPointsList;
    public LineRenderer line;
    public float reachedThreshhold = 8;
    public int resolution = 30; 

    private Vector3[] points;
    void Start()
    {
        updateSpline();
    }

    void FixedUpdate()
    {
        if (controlPointsList.Count > 0)
        {
            if ((controlPointsList[0].transform.position - Camera.main.transform.position).magnitude < reachedThreshhold)
            {
                controlPointsList[0].SetActive(false);
                controlPointsList.Remove(controlPointsList[0]);
                updateSpline();
            }
        }
    }

    void updateSpline()
    {
        points = new Vector3[resolution * (controlPointsList.Count - 1)];
        line.positionCount = resolution * (controlPointsList.Count - 1);
        for (int i = 0; i < controlPointsList.Count - 1; i++)
        {
            for (int t = 0; t < resolution; t++)
            {

                points[(i * resolution) + t] = GetCatmullRomPosition(t / (float)resolution,
                            controlPointsList[Mathf.Clamp(i - 1, 0, controlPointsList.Count - 1)].transform.position,
                            controlPointsList[Mathf.Clamp(i + 0, 0, controlPointsList.Count - 1)].transform.position,
                            controlPointsList[Mathf.Clamp(i + 1, 0, controlPointsList.Count - 1)].transform.position,
                            controlPointsList[Mathf.Clamp(i + 2, 0, controlPointsList.Count - 1)].transform.position);
            }
        }

        line.SetPositions(points);
    }

    //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
    //http://www.iquilezles.org/www/articles/minispline/minispline.htm
    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }
}