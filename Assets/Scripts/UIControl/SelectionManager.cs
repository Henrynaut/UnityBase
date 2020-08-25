using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    private Transform _selection;
    public Transform rayOrigin;
    public string objectName;
    public string buttonName1;
    public string buttonName2;


    public GameObject instructions_button1;
    public GameObject instructions_button2;




    private void Update()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }
        
        rayOrigin = Camera.main.transform;
        // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // if (Physics.Raycast(ray, out hit, 2))
        if(Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000))
        {
            // Debug.DrawLine(ray.origin, hit.point);
            var selection = hit.transform;
            Debug.Log("Hit");
            Debug.Log(rayOrigin);
            Debug.Log(hit.collider.gameObject.name);
            objectName = hit.collider.gameObject.name;

            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterial;
                }

                _selection = selection;
            }
        }

        // If the raycast points at Button1, toggle on the canvas text instructions
        if(objectName == buttonName1)
        {
            instructions_button1.SetActive(true);
        }
        else
        {
            instructions_button1.SetActive(false);
        }

        // If the raycast points at Button2, toggle on the canvas text instructions
        if(objectName == buttonName2)
        {
            instructions_button2.SetActive(true);
        }
        else
        {
            instructions_button2.SetActive(false);
        }
    }

}