using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


namespace AdvancedGizmo
{
    public class Hover : MonoBehaviour
    {

        public Color hovercolor;
        private Color original;
        private Renderer rend;
        private bool selected;
        private float t = 0;

        private EventSystem ES;

        void Start()
        {
            ES = EventSystem.current;
            rend = transform.GetComponent<Renderer>();
            original = rend.material.color;
        }

        void OnMouseEnter()
        {

            if (ES) if (EventSystem.current.IsPointerOverGameObject()) return;
            SetHovered();
        }

        void OnMouseExit()
        {
            if (ES) if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!selected)
                SetOriginal();
        }

        void SetHovered()
        {

            rend.material.color = hovercolor;
        }

        void SetOriginal()
        {

            rend.material.color = original;
        }

        void OnMouseDown()
        {
            if (ES) if (EventSystem.current.IsPointerOverGameObject()) return;
            selected = true;
            if (Time.time - t < 0.3f)
            {
                SendMessageUpwards("ChangeMode");
                SetOriginal();
            }
            t = Time.time;
        }

        void Update()
        {

            if (selected && Input.GetMouseButtonUp(0))
            {
                SetOriginal();
                selected = false;
            }
        }

    }
}
