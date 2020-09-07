//
//Filename: MaxCamera.cs
//
// original: http://www.unifycommunity.com/wiki/index.php?title=MouseOrbitZoom
//
// --01-18-2010 - create temporary target, if none supplied at start
 
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
 
 
[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
public class MaxCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int zoomRate = 40;
    public float panSpeed = 0.3f;
    public float zoomDampening = 5.0f;

    public int ignoreLayer;
 
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    [HideInInspector]
    public float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;

    private bool dragging = false;

    private float pinchdistance = 0;

    private EventSystem eventsystem;
 
    void Start() 
    {
        eventsystem = FindObjectOfType<EventSystem>() as EventSystem;
        Init(); 
    }
    void OnEnable() { Init(); }


 
    public void Init()
    {
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }
 
        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;
 
        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;
        Vector3 cross = Vector3.Cross(Vector3.right, transform.right);
        xDeg = Vector3.Angle(Vector3.right, transform.right );
        if (cross.y < 0) xDeg = 360 - xDeg;
        yDeg = Vector3.Angle(Vector3.up, transform.up );
    }
 
    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    void LateUpdate()
    {
        bool outsideNewGUI = true;
        if (eventsystem) outsideNewGUI = !eventsystem.IsPointerOverGameObject();

        if (Input.GetMouseButton(1))
        {
            
            if (!Input.GetMouseButtonDown(1))
            {
                
                
                xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                ////////OrbitAngle

                //Clamp the vertical axis for the orbit
                yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
                // set camera rotation 
                desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
                //currentRotation = transform.rotation;
                //rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
                //transform.rotation = rotation;
            }
            else
            {
                if (Input.touchCount > 1) pinchdistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
            }

        }
        // otherwise if middle mouse is selected, we pan by way of transforming the target in screenspace
        else if (Input.GetMouseButtonDown(0) && outsideNewGUI)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Debug.Log("mx "+hit.transform.gameObject.layer.ToString());
                if (hit.transform.gameObject.layer < ignoreLayer)

                {
                    if (!dragging) StartCoroutine(DragTarget(Input.mousePosition));
                }
            }
        }
        currentRotation = transform.rotation;
        rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
        transform.rotation = rotation;
 
        ////////Orbit Position
 
        // affect the desired Zoom distance if we roll the scrollwheel
        float scrollinp = Input.GetAxis("Mouse ScrollWheel");
#if UNITY_WEBGL
        scrollinp = 0.1f*Input.GetAxis("Mouse ScrollWheel");
#endif

        if (Input.touchCount > 1)
        {
            float newpinchdistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
            scrollinp = 0.0005f * (newpinchdistance - pinchdistance);
            pinchdistance = newpinchdistance;
        }

        desiredDistance -= scrollinp * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
        //currentRotation = transform.rotation;
        //rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
        //transform.rotation = rotation;
 
        // calculate position based on the new currentDistance 
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;

        //currentRotation = transform.rotation;
        //rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
        //transform.rotation = rotation;
    }

    IEnumerator DragTarget(Vector3 startingHit)
    {
        dragging = true;
        Vector3 startTargetPos = target.position;
        //while (Input.GetMouseButton (0)&&(!Input.touchSupported||Input.touchSupported&&Input.touchCount==1))
        while (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            Vector3 mouseMove = 0.005f * transform.position.y * (Input.mousePosition - startingHit);
            //Vector3 translation = new Vector3(mouseMove.x, 0, mouseMove.y);
            //float clampVal = 0.04f * (transform.position.y - target.position.y);
            Vector3 zDir = transform.forward;
            zDir.y = 0;
            target.position = startTargetPos - transform.right * mouseMove.x - zDir.normalized * mouseMove.y;
            yield return null;
        }
        dragging = false;
    }
 
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}