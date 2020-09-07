using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


namespace AdvancedGizmo
{
    public class Gizmo : MonoBehaviour
    {

        [Space(10)]
        public int layer;
        [Space(10)]
        public Collider xAxis;
        public Collider yAxis;
        public Collider zAxis;
        [Space(10)]
        public Collider xyPlane;
        public Collider xzPlane;
        public Collider yzPlane;
        [Space(10)]
        public Collider xyRotate;
        public Collider xzRotate;
        public Collider yzRotate;
        [Space(10)]
        public Collider sphereRotate;
        [Space(10)]
        public float rotationSpeed = 4.0f;

        private enum GizmoAxis { X, Y, Z, XY, XZ, YZ, XYRotate, XZRotate, YZRotate, none };
        private GizmoAxis selectedAxis;

        private RaycastHit hit;
        private Ray ray, ray1;
        private Plane dragplane;
        private float rayDistance, newRotY, rayDistancePrev, distance;
        private Vector3 hitvector, mousePos, lookCamera, startDrag, startPos, startDragRot, lookHitPoint;
        private bool dragging, rotating;

        private EventSystem ES;

        private float sphereRadius;

        private GameObject rotationParent;

        Vector3 rotationAxis = Vector3.zero;

        public int i = 1;


        void Start()
        {
            ES = EventSystem.current;
            layer = 1 << layer;
            sphereRadius = sphereRotate.bounds.extents.x;
            ChangeMode();
        }

        void Update()
        {

            bool mouseOutsideGUI = true;
            if (ES) mouseOutsideGUI = !EventSystem.current.IsPointerOverGameObject();

            if (Input.GetMouseButtonDown(1) && mouseOutsideGUI)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000f, layer))
                {
                    if (hit.transform.parent == transform) ChangeMode();
                }
            }

            if (Input.GetMouseButtonDown(0) && mouseOutsideGUI)
            {

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                dragplane = new Plane();

                if (Physics.Raycast(ray, out hit, 1000f, layer))
                {

                    hitvector = hit.point - transform.position;

                    if (hit.collider == xAxis)
                    {
                        selectedAxis = GizmoAxis.X;
                        dragplane.SetNormalAndPosition(transform.up, transform.position);
                    }
                    else if (hit.collider == yAxis)
                    {
                        selectedAxis = GizmoAxis.Y;
                        dragplane.SetNormalAndPosition(transform.forward, transform.position);
                    }
                    else if (hit.collider == zAxis)
                    {
                        selectedAxis = GizmoAxis.Z;
                        dragplane.SetNormalAndPosition(transform.up, transform.position);
                    }
                    else if (hit.collider == xyPlane)
                    {
                        selectedAxis = GizmoAxis.XY;
                        dragplane.SetNormalAndPosition(transform.forward, transform.position);
                    }
                    else if (hit.collider == xzPlane)
                    {
                        selectedAxis = GizmoAxis.XZ;
                        dragplane.SetNormalAndPosition(transform.up, transform.position);
                    }
                    else if (hit.collider == yzPlane)
                    {
                        selectedAxis = GizmoAxis.YZ;
                        dragplane.SetNormalAndPosition(transform.right, transform.position);
                    }
                    else if (hit.collider == xyRotate)
                    {
                        selectedAxis = GizmoAxis.XYRotate;
                        rotationAxis = -transform.forward;
                        lookHitPoint = hit.point - transform.position - Vector3.Project(hit.point - transform.position, transform.forward);
                        //Debug.DrawLine(transform.position, transform.position + lookHitPoint, Color.cyan, 10.0f);
                        dragplane.SetNormalAndPosition(lookHitPoint, hit.point);
                        //DrawPlane(lookHitPoint, transform.position + lookHitPoint, 10.0f);
                        rotating = true;
                    }
                    else if (hit.collider == xzRotate)
                    {
                        selectedAxis = GizmoAxis.XZRotate;
                        rotationAxis = -transform.up;
                        lookHitPoint = hit.point - transform.position - Vector3.Project(hit.point - transform.position, transform.up);
                        //Debug.DrawLine(transform.position, transform.position + lookHitPoint, Color.cyan, 10.0f);
                        dragplane.SetNormalAndPosition(lookHitPoint, hit.point);
                        //DrawPlane(lookHitPoint, transform.position + lookHitPoint, 10.0f);
                        rotating = true;
                    }
                    else if (hit.collider == yzRotate)
                    {
                        selectedAxis = GizmoAxis.YZRotate;
                        rotationAxis = -transform.right;
                        lookHitPoint = hit.point - transform.position - Vector3.Project(hit.point - transform.position, transform.right);
                        //Debug.DrawLine(transform.position, transform.position + lookHitPoint, Color.cyan, 10.0f);
                        dragplane.SetNormalAndPosition(lookHitPoint, hit.point);
                        //DrawPlane(lookHitPoint, transform.position + lookHitPoint, 10.0f);
                        rotating = true;
                    }
                    else if (hit.collider == sphereRotate)
                    {
                        selectedAxis = GizmoAxis.none;
                        lookCamera = Camera.main.transform.position - transform.position;
                        startDragRot = transform.position + lookCamera.normalized * sphereRadius;
                        dragplane.SetNormalAndPosition(lookCamera.normalized, startDragRot);
                        //DrawPlane(lookCamera.normalized, startDragRot, 10.0f);
                        rotating = true;
                    }
                    else
                    {
                        Debug.Log(hit.collider.name);
                        return;
                    }

                    if (rotating)
                    {
                        if (dragplane.Raycast(ray, out rayDistance))
                        {
                            startDragRot = ray.GetPoint(rayDistance);
                        }
                        rotationParent = new GameObject();
                        rotationParent.transform.position = transform.position;
                        transform.SetParent(rotationParent.transform);
                    }

                    distance = hit.distance;
                    startDrag = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
                    startPos = transform.position;
                    dragging = true;
                }
            }

            if (dragging || rotating)
            {



                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (dragplane.Raycast(ray, out rayDistance))
                {

                    mousePos = ray.GetPoint(rayDistance);
                }


                Vector3 onDrag = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
                Vector3 translation = onDrag - startDrag;
                Vector3 projectedTranslation = Vector3.zero;

                if (dragging)
                {

                    switch (selectedAxis)
                    {

                        case GizmoAxis.X:
                            {
                                projectedTranslation = Vector3.Project(translation, transform.right);
                                transform.position = startPos + projectedTranslation.normalized * translation.magnitude;
                                break;
                            }
                        case GizmoAxis.Y:
                            {
                                projectedTranslation = Vector3.Project(translation, transform.up);
                                transform.position = startPos + projectedTranslation.normalized * translation.magnitude;
                                break;
                            }
                        case GizmoAxis.Z:
                            {
                                projectedTranslation = Vector3.Project(translation, transform.forward);
                                transform.position = startPos + projectedTranslation.normalized * translation.magnitude;
                                break;
                            }
                        case GizmoAxis.XY:
                        case GizmoAxis.XZ:
                        case GizmoAxis.YZ:
                            {
                                transform.position = mousePos - hitvector;
                                break;
                            }

                    }
                }

                if (rotating)
                {
                    translation = mousePos - startDragRot;
                    //Debug.DrawLine(startDragRot, mousePos, Color.white, 6.0f);

                    Vector3 rotationAxis2 = Vector3.zero;

                    switch (selectedAxis)
                    {
                        case GizmoAxis.XYRotate:
                            {
                                projectedTranslation = translation - Vector3.Project(translation, rotationAxis);
                                //Debug.DrawLine(startDragRot, startDragRot + projectedTranslation, Color.yellow, 1.0f);
                                rotationAxis2 = Vector3.Cross(projectedTranslation, lookHitPoint);
                                //Debug.DrawLine(transform.position, transform.position + 5 * rotationAxis2.normalized, Color.magenta, 1.0f);
                                //Debug.DrawLine(transform.position, transform.position + 5 * rotationAxis.normalized, Color.cyan, 1.0f);
                                break;
                            }
                        case GizmoAxis.XZRotate:
                            {
                                projectedTranslation = translation - Vector3.Project(translation, rotationAxis);
                                //Debug.DrawLine(startDragRot, startDragRot + projectedTranslation, Color.yellow, 1.0f);
                                rotationAxis2 = Vector3.Cross(projectedTranslation, lookHitPoint);
                                //Debug.DrawLine(transform.position, transform.position + 5 * rotationAxis2.normalized, Color.magenta, 1.0f);
                                //Debug.DrawLine(transform.position, transform.position + 5 * rotationAxis.normalized, Color.magenta, 1.0f);
                                break;
                            }
                        case GizmoAxis.YZRotate:
                            {
                                projectedTranslation = translation - Vector3.Project(translation, rotationAxis);
                                //Debug.DrawLine(startDragRot, startDragRot + projectedTranslation, Color.yellow, 1.0f);
                                rotationAxis2 = Vector3.Cross(projectedTranslation, lookHitPoint);

                                //Debug.DrawLine(transform.position, transform.position + 5 * rotationAxis.normalized, Color.gray, 1.0f);
                                break;
                            }
                        case GizmoAxis.none:
                            {
                                rotationAxis2 = rotationAxis;
                                projectedTranslation = translation;
                                rotationAxis = Vector3.Cross(translation, lookCamera);
                                break;
                            }
                    }
                    float angle;
                    Quaternion delta;

                    //Debug.DrawLine(transform.position, transform.position + 5 * rotationAxis.normalized, Color.red, 1.0f);
                    angle = -Mathf.Rad2Deg * projectedTranslation.magnitude / sphereRadius;
                    delta = Quaternion.AngleAxis(angle, rotationAxis2);

                    if (selectedAxis == GizmoAxis.none)
                    {

                        rotationParent.transform.rotation = delta;

                    }
                    else
                    {

                        rotationParent.transform.rotation = delta;
                    }

                }

                if (Input.GetMouseButtonUp(0)) SetFalse();

            }

        }

        public void ChangeMode()
        {
            i = (i + 1) % 2;
            // i = 0 / translation;  i = 1 / rotation 
            bool val = (i == 1);

            xyRotate.gameObject.SetActive(val);
            xzRotate.gameObject.SetActive(val);
            yzRotate.gameObject.SetActive(val);
            sphereRotate.gameObject.SetActive(val);

            xAxis.gameObject.SetActive(!val);
            yAxis.gameObject.SetActive(!val);
            zAxis.gameObject.SetActive(!val);
            xyPlane.gameObject.SetActive(!val);
            xzPlane.gameObject.SetActive(!val);
            yzPlane.gameObject.SetActive(!val);
        }


        void SetFalse()
        {
            transform.SetParent(null);
            if (rotationParent) Destroy(rotationParent);
            rotating = false;
            dragging = false;
        }

        void DrawPlane(Vector3 normal, Vector3 position, float t)
        {

            Vector3 v3;

            if (normal.normalized != Vector3.forward)
                v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
            else
                v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

            var corner0 = position + v3;
            var corner2 = position - v3;
            Quaternion q = Quaternion.AngleAxis(90.0f, normal);
            v3 = q * v3;
            var corner1 = position + v3;
            var corner3 = position - v3;

            Debug.DrawLine(corner0, corner2, Color.green, t);
            Debug.DrawLine(corner1, corner3, Color.green, t);
            Debug.DrawLine(corner0, corner1, Color.green, t);
            Debug.DrawLine(corner1, corner2, Color.green, t);
            Debug.DrawLine(corner2, corner3, Color.green, t);
            Debug.DrawLine(corner3, corner0, Color.green, t);
            Debug.DrawRay(position, normal, Color.red, t);
        }
    }
}