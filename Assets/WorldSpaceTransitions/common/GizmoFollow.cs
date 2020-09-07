using UnityEngine;
using System.Collections;

namespace WorldSpaceTransitions
{
    public class GizmoFollow : MonoBehaviour
    {
        private static int m_referenceCount = 0;

        private static GizmoFollow m_instance;
        private Vector3 tempPos;
        private Quaternion tempRot;

        public bool followPosition = true;
        public bool followRotation = true;

        public static GizmoFollow Instance
        {
            get
            {
                return m_instance;
            }
        }

        void Awake()
        {
            m_referenceCount++;
            if (m_referenceCount > 1)
            {
                DestroyImmediate(this.gameObject);
                return;
            }

            m_instance = this;
            // Use this line if you need the object to persist across scenes
            //DontDestroyOnLoad(this.gameObject);
        }


        void Update()
        {


            if (tempPos != transform.position || tempRot != transform.rotation)
            {

                tempPos = transform.position;
                tempRot = transform.rotation;
                SetSection();
            }
        }


        void OnEnable()
        {
            SetSection();
        }

        void OnDestroy()
        {
            m_referenceCount--;
            if (m_referenceCount == 0)
            {
                m_instance = null;
            }
        }

        void SetSection()
        {

            if (followPosition) Shader.SetGlobalVector("_SectionPoint", transform.position);
            if (followRotation)
            {
                Shader.SetGlobalVector("_SectionPlane", transform.forward);
                Shader.SetGlobalVector("_SectionPlane2", transform.right);
            }
        }

        void OnDrawGizmos()
        {
            Vector3 a = Camera.main.transform.position;
            Plane sPlane = new Plane(transform.forward, transform.position);
            Ray cameraRay = new Ray(a, Camera.main.transform.forward);
            Vector3 b = Vector3.zero;
            float planeDist = 0;
            if (sPlane.Raycast(cameraRay, out planeDist))
                b = a + planeDist * Camera.main.transform.forward;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(transform.position, b);
        }
    }
}